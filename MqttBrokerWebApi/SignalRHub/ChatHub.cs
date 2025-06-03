using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace MqttBrokerWebApi.SignalRHub
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> _userToConnectionId = new(); // clientName -> connectionId
        //private static Dictionary<string, string> _connectionIdToUser = new(); // connectionId -> clientName

        public override async Task OnConnectedAsync()
        {
            // Noch nicht registriert? Dann wird erst bei RegisterClient gespeichert
            await Clients.Caller.SendAsync("ClientListUpdated", _userToConnectionId.Keys.ToList());
            await base.OnConnectedAsync();
        }

        public async Task RegisterClient(string clientName)
        {
            var connectionId = Context.ConnectionId;

            _userToConnectionId[clientName] = connectionId;
            //_connectionIdToUser[connectionId] = clientName;

            await Clients.All.SendAsync("ClientListUpdated", _userToConnectionId);
        }

        public async Task SendToUser(string targetUserName, string message)
        {
            if (_userToConnectionId.TryGetValue(targetUserName, out var connectionId))
            {
                var senderName = _userToConnectionId.GetValueOrDefault(Context.ConnectionId, "Unbekannt");
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderName, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;

            if (_userToConnectionId.TryGetValue(connectionId, out var username))
            {
                _userToConnectionId.Remove(connectionId);
                _userToConnectionId.Remove(username);

                await Clients.All.SendAsync("ClientListUpdated", _userToConnectionId.Keys.ToList());
            }

            await base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

}
