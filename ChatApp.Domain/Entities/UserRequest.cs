
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Domain.Entities
{
    public class UserRequest
    {
        public Guid ChatSessionId { get; set; }  // Unique identifier for external use
        public int UserId { get; set; }
        //public string Status { get; set; } = "Pending"; // Pending, Assigned, Completed 
        public ChatStatus Status { get; set; }
        public string Content { get; set; }
        public List<RequestMessage> Messages { get; set; }
        
        public int? AssignedAgentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastPolledAt { get; set; }
        public DateTime LastActiveTime { get; set; } = DateTime.UtcNow;

        

    }

    public class RequestMessage
    {
        public Guid ChatSessionId { get; set; }
        public int MessageId { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; } //// User, Agent
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
