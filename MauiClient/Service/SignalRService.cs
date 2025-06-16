using Microsoft.AspNetCore.SignalR.Client;

namespace MauiClient.Service
{
    public class SignalRService
    {
        private HubConnection? _hubConnection;

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44399//notificationHub") 
                .WithAutomaticReconnect()
                .Build();

           
            _hubConnection.On<bool>("LichtGeaendert", (isLicht) =>
            {
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

            try
            {
                await _hubConnection.StartAsync();
               

                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.SendAsync("RegisterClient", $"{DeviceInfo.Name}");
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

    }
}
