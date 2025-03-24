using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using ChatApp.Repository.Interface;
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

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<bool> AddChatSessionAsync(ChatSession chatSession)
        {
            await _chatRepository.AddChatSessionAsync(chatSession);
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
    }
}
