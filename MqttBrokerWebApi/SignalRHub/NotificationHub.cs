using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace MqttBrokerWebApi.SignalRHub
{
    public class NotificationHub : Hub
    {
        private static Dictionary<string, string> _connectedClients = new();
        private HubConnection? _hubConnection;

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ClientListUpdated", _connectedClients.Values.ToList());
            await base.OnConnectedAsync();
        }

        public async Task UpdateLichtState(bool isLicht)
        {
            // Weiterverteilung des Status an alle Clients
            await Clients.All.SendAsync("LichtGeaendert", isLicht);
        }

        public async Task RegisterClient(string geraetName)
        {
            var connectionId = Context.ConnectionId;

            _connectedClients[connectionId] = geraetName;
            await Clients.All.SendAsync("ClientListUpdated", _connectedClients);

            //return Task.CompletedTask;
        }

        public async Task SendePushText(string nachricht)
        {
            await Clients.All.SendAsync("ShowNotification", nachricht);
        }

        public async Task SendOffer(string offer)
        {
            await Clients.Others.SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string answer)
        {
            await Clients.Others.SendAsync("ReceiveAnswer", answer);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectedClients.ContainsKey(Context.ConnectionId))
            {
                _connectedClients.Remove(Context.ConnectionId);
                await Clients.All.SendAsync("ClientListUpdated", _connectedClients);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}

