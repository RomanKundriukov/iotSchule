using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MqttBrokerWebApi.SignalRHub;
using MqttBrokerWebApi.Models;

namespace MqttBrokerWebApi.Controllers
{
    public class IoTController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hub;
        public IoTController(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("licht")]
        public async Task<IActionResult> ReceiveGps([FromBody] bool isLicht)
        {
            // SignalR optional zum Verteilen an Clients
            await _hub.Clients.All.SendAsync("Licht", isLicht);
            return Ok();
        }

    }
}
