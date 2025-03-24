using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using ChatApp.Repository.Interface;
using ChatApp.Service.Interface;
using ChatApp.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Service.Implementations
{
    public class ChatAssignmentService : BackgroundService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IAgentService _agentService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatAssignmentService> _logger;
        private static readonly ConcurrentQueue<ChatSession> _chatQueue = new();

        public ChatAssignmentService(
            IChatRepository chatRepository,
            IAgentService agentService,
            IHubContext<ChatHub> hubContext,
            ILogger<ChatAssignmentService> logger)
        {
            _chatRepository = chatRepository;
            _agentService = agentService;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessChatsAsync();
                await MonitorInactiveChatsAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessChatsAsync()
        {
            var pendingChats = await _chatRepository.GetPendingChatsAsync();
            var agents = await _agentService.GetAvailableAgentsAsync();

            if (!pendingChats.Any())
            {
                _logger.LogInformation("No pending chats to process.");
                return;
            }

            if (!agents.Any())
            {
                _logger.LogWarning("No available agents found. Checking overflow handling...");

                // Overflow Handling - Check if it's office hours
                if (!IsOfficeHours())
                {
                    _logger.LogWarning("No agents available and it's outside office hours. Skipping chat assignment.");
                    return;
                }

                // If during office hours, use overflow agents
                agents = agents.Where(a => a.AgentLevel == AgentLevel.Junior).ToList();

                if (!agents.Any())
                {
                    _logger.LogWarning("No overflow agents available. Unable to process chats.");
                    return;
                }
            }

            foreach (var chatSession in pendingChats)
            {
                var orderedAgents = agents
                    .OrderBy(agent => agent.AgentLevel)
                    .ThenBy(agent => agent.CurrentChats)
                    .ToList();

                var assignedAgent = orderedAgents.FirstOrDefault(agent => agent.CurrentChats < agent.MaxConcurrency);

                if (assignedAgent != null)
                {
                    assignedAgent.CurrentChats++;
                    chatSession.Status = ChatStatus.Assigned;
                    await _agentService.UpdateAgentAsync(assignedAgent);
                    await _chatRepository.UpdateChatSessionAsync(chatSession);

                    _logger.LogInformation($"Assigned chat {chatSession.ChatSessionId} to agent {assignedAgent.Name}.");
                    await NotifyClients($"Chat {chatSession.ChatSessionId} assigned to agent {assignedAgent.Name}.");
                }
                else
                {
                    _logger.LogWarning($"No available agent found for chat {chatSession.ChatSessionId}. Adding back to the queue.");
                    _chatQueue.Enqueue(chatSession);  // Add back to queue for retry
                }
            }
        }


        private bool IsOfficeHours()
        {
            var currentHour = DateTime.UtcNow.Hour;
            return currentHour >= 9 && currentHour < 17;
        }

        private async Task MonitorInactiveChatsAsync()
        {
            var pendingChats = await _chatRepository.GetPendingChatsAsync();

            foreach (var chat in pendingChats)
            {
                // Check if chat is inactive (No polling within 3 seconds)
                if ((DateTime.UtcNow - chat.LastActiveTime).TotalSeconds > 3)
                {
                    _logger.LogWarning($"Chat {chat.ChatSessionId} marked as inactive due to inactivity.");
                    chat.Status = ChatStatus.Inactive;
                    await _chatRepository.UpdateChatSessionAsync(chat);
                    await NotifyClients($"Chat {chat.ChatSessionId} marked as inactive due to inactivity.");
                }
            }
        }


        private async Task NotifyClients(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
