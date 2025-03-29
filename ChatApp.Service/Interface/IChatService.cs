using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository.Interface
{
    public interface IChatService
    {
        Task<bool> AddChatSessionAsync(ChatSession chatSession);
        Task<List<ChatSession>> GetPendingChatsAsync();
        Task<List<ChatSession>> GetAllChatsAsync();
        Task<bool> AssignChatToAgentAsync(ChatSession chatSession, Agent agent);

        Task<ChatSession?> GetChatSessionByGuidAsync(Guid sessionGuid);

        Task<bool> UpdateChatSessionAsync(ChatSession chatSession);
        Task<bool> UpdateChatSessionStatusAsync(ChatSession chatSession);

        /// Below code using UserRequests
        Task<ChatSessionMessage> CreateChatSessionMessageAsync(ChatSessionMessage message);
        Task<List<ChatSessionMessage?>> GetChatSessionMessageIdAsync(Guid chatSessionId);

    }
}
