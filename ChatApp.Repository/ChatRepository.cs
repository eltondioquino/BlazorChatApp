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
        private static readonly ConcurrentBag<UserRequest> UserRequests = new();
        private static readonly ConcurrentBag<RequestMessage> Messages = new();

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

        public Task UpdateChatSessionStatusAsync(ChatSession chatSession)
        {
            var existingChat = ChatSessions.FirstOrDefault(c => c.ChatSessionId == chatSession.ChatSessionId);

            if (existingChat != null)
            {
                // Update the existing chat session
                existingChat.Status = chatSession.Status;
                existingChat.LastPolledAt = chatSession.LastPolledAt;
            }

            return Task.CompletedTask;
        }


        /// Below code using the UserRequest

        /// <summary>
        /// CreateChatSessionAsync
        /// </summary>
        /// <param name="chatSession"></param>
        /// <returns></returns>
        public Task<UserRequest> CreateChatSessionAsync(UserRequest chatSession)
        {
            UserRequests.Add(chatSession);
            return Task.FromResult(chatSession);
        }

        public Task<RequestMessage> CreateChatSessionMessageAsync(RequestMessage message)
        {
            Messages.Add(message);
            return Task.FromResult(message);
        }

        public Task<UserRequest?> GetChatSessionByIdAsync(Guid chatSessionId)
        {
            var session = UserRequests.FirstOrDefault(s => s.ChatSessionId == chatSessionId);
            return Task.FromResult(session);
        }
    }
}
