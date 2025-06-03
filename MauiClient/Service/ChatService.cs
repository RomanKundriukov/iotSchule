using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiClient.Service
{
    public class ChatService
    {
        private HubConnection? hubConnection;
        public event Action<string, string>? NachrichtEmpfangen;
        public event Action<List<string>>? BenutzerlisteAktualisiert;

        public async Task StartAsync(string username)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://aleksssandra-001-site1.anytempurl.com/chathub")
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
            {
                NachrichtEmpfangen?.Invoke(sender, message);
            });

            hubConnection.On<List<string>>("ClientListUpdated", (users) =>
            {
                BenutzerlisteAktualisiert?.Invoke(users);
            });

            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("RegisterClient", username);
        }

        public async Task SendMessageToUser(string targetUsername, string message)
        {
            if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.InvokeAsync("SendToUser", targetUsername, message);
            }
        }
    }

}
