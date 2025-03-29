using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task ChatNotification(ChatSessionMessage chatMessage)
        {
            // Logic to handle the incoming message from client, broadcast, or process
            await Clients.All.SendAsync("ChatNotification", chatMessage);
        }

        public async Task NotifyChatAssignment(string chatSessionId, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", chatSessionId, message);
        }
    }
}
