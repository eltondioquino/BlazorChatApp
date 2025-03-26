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
                await ProcessQueuedChatsAsync();
                await ProcessChatsAsync();
                await MonitorInactiveChatsAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task ProcessQueuedChatsAsync()
        {
            while (_chatQueue.TryDequeue(out var chatSession))
            {
                var assignedAgent = await AssignAgentAsync(chatSession);
                if (assignedAgent == null)
                {
                    chatSession.Status = ChatStatus.UnAssigned;
                    await _chatRepository.UpdateChatSessionAsync(chatSession);
                    await NotifyClients(chatSession.ChatSessionId.ToString(), "Chat is unassigned.");
                }
                else
                {
                    await NotifyClients(chatSession.ChatSessionId.ToString(), $"Chat assigned to agent {assignedAgent.Name}.");
                }
            }
        }

        private async Task ProcessChatsAsync()
        {
            var pendingChats = await _chatRepository.GetPendingChatsAsync();
            var agents = await _agentService.GetAvailableAgentsAsync();

            if (!pendingChats.Any())
            {
                _logger.LogWarning("No pending chat to process.");
                return;
            }

            int capacity = (int)agents.Sum(a => a.MaxConcurrency * GetSeniorityMultiplier(a.AgentLevel));
            int maxQueueSize = (int)(capacity * 1.5);

            if (pendingChats.Count() > maxQueueSize && IsOfficeHours())
            {
                _logger.LogWarning("Max queue size reached. Assigning overflow team.");

                /// Create new overflow agent and assign the queue
            }

            foreach (var chatSession in pendingChats)
            {
                var assignedAgent = await AssignAgentAsync(chatSession);
                if (assignedAgent == null)
                {
                    _logger.LogWarning($"No available agent for chat {chatSession.ChatSessionId}. Marking as unassigned.");
                    chatSession.Status = ChatStatus.UnAssigned;
                    await _chatRepository.UpdateChatSessionAsync(chatSession);
                    await NotifyClients(chatSession.ChatSessionId.ToString(), "Chat is unassigned.");
                }
                else
                {
                    await NotifyClients(chatSession.ChatSessionId.ToString(), $"Chat assigned to agent {assignedAgent.Name}.");
                }
            }
        }

        private async Task<Agent?> AssignAgentAsync(ChatSession chatSession)
        {
            var agents = await _agentService.GetAvailableAgentsAsync();
            var orderedAgents = agents.OrderBy(a => a.AgentLevel).ThenBy(a => a.CurrentChats).ToList();

            var assignedAgent = orderedAgents.FirstOrDefault(a => a.CurrentChats < a.MaxConcurrency);

            if (assignedAgent != null)
            {
                assignedAgent.CurrentChats++;
                chatSession.Status = ChatStatus.Assigned;
                chatSession.AssignedAgentId = assignedAgent.Id;

                await _agentService.UpdateAgentAsync(assignedAgent);
                await _chatRepository.UpdateChatSessionAsync(chatSession);
                await NotifyClients(chatSession.ChatSessionId.ToString(), $"Chat assigned to agent {assignedAgent.Name}.");
            }
            else
            {
                chatSession.Status = ChatStatus.UnAssigned;
                await _chatRepository.UpdateChatSessionAsync(chatSession);
                await NotifyClients(chatSession.ChatSessionId.ToString(), "Chat is unassigned.");
            }

            return assignedAgent;
        }

        private async Task MonitorInactiveChatsAsync()
        {
            var pendingChats = await _chatRepository.GetPendingChatsAsync();
            foreach (var chat in pendingChats)
            {
                if ((DateTime.UtcNow - chat.LastActiveTime).TotalSeconds > 3)
                {
                    chat.Status = ChatStatus.Inactive;
                    await _chatRepository.UpdateChatSessionAsync(chat);
                    await NotifyClients(chat.ChatSessionId.ToString(), "Chat marked as inactive due to inactivity.");
                }
            }
        }

        private bool IsOfficeHours()
        {
            var currentHour = DateTime.UtcNow.Hour;
            return currentHour >= 8 && currentHour < 16;
        }

        private double GetSeniorityMultiplier(AgentLevel level)
        {
            return level switch
            {
                AgentLevel.Junior => 0.4,
                AgentLevel.MidLevel => 0.6,
                AgentLevel.Senior => 0.8,
                AgentLevel.TeamLead => 0.5,
                _ => 0.4
            };
        }

        private async Task NotifyClients(string chatSessionId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", chatSessionId, message);
        }
    }
}
