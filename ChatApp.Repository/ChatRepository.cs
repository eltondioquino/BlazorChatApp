using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using ChatApp.Repository.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Repository
{
    public class ChatRepository : IChatRepository
    {
        private static readonly ConcurrentBag<ChatSession> ChatSessions = new();

        public Task AddChatSessionAsync(ChatSession chatSession)
        {
            ChatSessions.Add(chatSession);
            return Task.CompletedTask;
        }

        public Task<List<ChatSession>> GetPendingChatsAsync()
        {
            var pendingChats = ChatSessions.Where(c => c.Status == ChatStatus.Pending).ToList();
            return Task.FromResult(pendingChats);
        }

        public Task<List<ChatSession>> GetAllChatsAsync()
        {
            return Task.FromResult(ChatSessions.ToList());
        }

        public Task UpdateChatSessionAsync(ChatSession chatSession)
        {
            var existingChat = ChatSessions.FirstOrDefault(c => c.ChatSessionId == chatSession.ChatSessionId);

            if (existingChat != null)
            {
                // Update the existing chat session
                existingChat.Status = chatSession.Status;
                existingChat.LastActiveTime = chatSession.LastActiveTime;
            }

            return Task.CompletedTask;
        }
    }
}
