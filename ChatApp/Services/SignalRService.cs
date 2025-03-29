using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.UI.Services
{
    public class SignalRService
    {
        public HubConnection Connection { get; private set; }
        public event Action<string, string> OnMessageReceived;
        public event Action<string, string>? OnNotificationReceived;
        public event Action<ChatSessionMessage>? OnChatReceived;

        public async Task StartConnection()
        {
            if (Connection == null || Connection.State == HubConnectionState.Disconnected)
            {
                Connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:5001/chathub")
                    .Build();

                Connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    OnMessageReceived?.Invoke(user, message);
                });

                Connection.On<string, string>("ReceiveNotification", (chatSessionId, message) =>
                {
                    OnNotificationReceived?.Invoke(chatSessionId, message);
                });

                Connection.On<ChatSessionMessage>("ChatNotification", (message) =>
                {
                    OnChatReceived?.Invoke(message);
                });

                await Connection.StartAsync();
            }
        }

        public async Task SendMessage(string user, string message)
        {
            if (Connection != null && Connection.State == HubConnectionState.Connected)
            {
                await Connection.SendAsync("SendMessage", user, message);
            }
        }

        public async Task SendChatMessage(ChatSessionMessage chatMessage)
        {
            //if (Connection != null && Connection.State == HubConnectionState.Connected)
            //{
            //    await Connection.SendAsync("ChatNotification", chatMessage);
            //}

            await Connection.SendAsync("ChatNotification", chatMessage);
        }
    }
}
