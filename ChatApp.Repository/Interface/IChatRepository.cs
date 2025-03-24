using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Repository.Interface
{
    public interface IChatRepository
    {
        Task AddChatSessionAsync(ChatSession chatSession);
        Task<List<ChatSession>> GetPendingChatsAsync();
        Task<List<ChatSession>> GetAllChatsAsync();
        Task UpdateChatSessionAsync(ChatSession chatSession);
        Task UpdateChatSessionStatusAsync(ChatSession chatSession);
    }
}
