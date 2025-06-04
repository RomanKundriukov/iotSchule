using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using Microsoft.Maui.Devices;

namespace MauiClient.Service
{
    public class SignalRService
    {
        private HubConnection _hubConnection;
        private readonly WebRTCService _webrtc;

        public SignalRService(WebRTCService webrtc)
        {
            _webrtc = webrtc;
        }
        //public event Action<bool> LichtEmpfangen;

        public async Task StartAsync()
        {
            //https://aleksssandra-001-site1.anytempurl.com
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44399/notificationHub") // oder lokale IP im LAN
                .WithAutomaticReconnect()
                .Build();

           
            _hubConnection.On<bool>("LichtGeaendert", (isLicht) =>
            {
                // Weiterleiten an UI
                //LichtEmpfangen?.Invoke(isLicht);
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var status = await Permissions.RequestAsync<Permissions.Flashlight>();
                        if (status == PermissionStatus.Granted)
                        {
                            if (isLicht)
                                await Flashlight.Default.TurnOnAsync();
                            else
                                await Flashlight.Default.TurnOffAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Berechtigung", "Taschenlampen-Berechtigung nicht erteilt", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Fehler", ex.Message, "OK");
                    }
                });

            });

            _hubConnection.On<string>("ShowNotification", (message) =>
            {
                try
                {
                    
                    var request = new NotificationRequest
                    {
                        NotificationId = 101,
                        Title = "📢 Hinweis",
                        Description = message,
                        ReturningData = "Dummy", // optional
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddSeconds(10)
                        }
                    };

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LocalNotificationCenter.Current.Show(request);
                    });
                        // MainThread.BeginInvokeOnMainThread(() =>
                        // {
                        //     var request = new NotificationRequest
                        //     {
                        //         Title = "WoW",
                        //         Description = message,
                        //         ReturningData = "",
                        //         Schedule = new NotificationRequestSchedule()
                        //         {
                        //             NotifyTime  = DateTime.Now.AddSeconds(5),
                        //         }
                        //         //NotificationId = 101,
                        //     };
                        //     LocalNotificationCenter.Current.Show(request);
                        // });
                    
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            });

            _hubConnection.On<string>("ReceiveOffer", async (offer) =>
            {
                await _webrtc.SetRemoteDescriptionAsync(offer);
                var answer = await _webrtc.CreateAnswerAsync();
                await _hubConnection.SendAsync("SendAnswer", answer);
            });

            _hubConnection.On<string>("ReceiveAnswer", async (answer) =>
            {
                await _webrtc.SetRemoteDescriptionAsync(answer);
            });

            _webrtc.OnSignal += async signal =>
            {
                if(_hubConnection.State == HubConnectionState.Connected)
                    await _hubConnection.SendAsync("SendOffer", signal);
            };

            try
            {
                await _hubConnection.StartAsync();
               

                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.SendAsync("RegisterClient", $"{DeviceInfo.Name}");
                    await _webrtc.StartConnectionAsync();
                }
                else
                {
                    Console.WriteLine("SignalR-Verbindung konnte nicht hergestellt werden.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }

        public async Task SendLichtAsync(bool status)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("UpdateLichtState", status);
            }
        }

        public async Task SendMessage(string targetConnectionId, string message)
        {
            await _hubConnection.InvokeAsync("SendToClient", targetConnectionId, message);
        }
    }
}
