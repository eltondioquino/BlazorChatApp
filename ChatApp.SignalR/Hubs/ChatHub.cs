using Microsoft.AspNetCore.SignalR;

namespace ChatApp.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task NotifyChatAssignment(string chatSessionId, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", chatSessionId, message);
        }
    }
}
