using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace MqttBrokerWebApi.SignalRHub
{
    public class NotificationHub : Hub
    {
        private static Dictionary<string, string> _connectedClients = new();

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ClientListUpdated", _connectedClients.Values.ToList());
            await base.OnConnectedAsync();
        }

        public async Task UpdateLichtState(bool isLicht)
        {
            await Clients.All.SendAsync("LichtGeaendert", isLicht);
        }

        public async Task RegisterClient(string geraetName)
        {
            var connectionId = Context.ConnectionId;

            _connectedClients[connectionId] = geraetName;
            await Clients.All.SendAsync("ClientListUpdated", _connectedClients);

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

