using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatApp.UI.Services
{
    public class SignalRService
    {
        public HubConnection Connection { get; private set; }
        public event Action<string, string> OnMessageReceived;

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
    }
}
