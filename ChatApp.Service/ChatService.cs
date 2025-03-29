using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using ChatApp.Repository.Interface;
using ChatApp.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Service
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IChatRepository chatRepository, 
            IHubContext<ChatHub> hubContext)
        {
            _chatRepository = chatRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> AddChatSessionAsync(ChatSession chatSession)
        {
            await _chatRepository.AddChatSessionAsync(chatSession);

            await NotifyClients(chatSession);

            return true;
        }

        public async Task<List<ChatSession>> GetPendingChatsAsync()
        {
            return await _chatRepository.GetPendingChatsAsync();
        }

        public async Task<List<ChatSession>> GetAllChatsAsync()
        {
            return await _chatRepository.GetAllChatsAsync();
        }

        public async Task<bool> AssignChatToAgentAsync(ChatSession chatSession, Agent agent)
        {
            chatSession.AssignedAgentId = agent.Id;
            chatSession.Status = ChatStatus.Assigned;
            await _chatRepository.UpdateChatSessionAsync(chatSession);
            return true;
        }

        public async Task<ChatSession?> GetChatSessionByGuidAsync(Guid sessionGuid)
        {
            var allChats = await _chatRepository.GetAllChatsAsync();
            return allChats.FirstOrDefault(chat => chat.ChatSessionId == sessionGuid);
        }

        public async Task<bool> UpdateChatSessionAsync(ChatSession chatSession)
        {
            await _chatRepository.UpdateChatSessionAsync(chatSession);
            return true;
        }

        public async Task<bool> UpdateChatSessionStatusAsync(ChatSession chatSession)
        {
            await _chatRepository.UpdateChatSessionStatusAsync(chatSession);
            return true;
        }

        /// Below code using User Request

        public async Task<ChatSessionMessage> CreateChatSessionMessageAsync(ChatSessionMessage message)
        {
            return await _chatRepository.CreateChatSessionMessageAsync(message);
        }

        public Task<List<ChatSessionMessage?>> GetChatSessionMessageIdAsync(Guid chatSessionId)
        {
            return _chatRepository.GetChatSessionMessageIdAsync(chatSessionId);
        }

        private async Task NotifyClients(ChatSession chatSession)
        {
            await _hubContext.Clients.All.SendAsync("ChatNotification", chatSession);
        }
    }
}

