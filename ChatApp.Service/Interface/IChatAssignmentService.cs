using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Service.Interface
{
    public interface IChatAssignmentService
    {
        Task ProcessChatsAsync();
        Task AssignChatToAgent(ChatSession chat, Agent agent, IChatRepository chatRepository, IAgentService agentService);
        Task NotifyClients(string message);
    }
}
