using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

namespace MauiClient.Service
{
    public class SignalRService
    {
        private HubConnection? _hubConnection;
        public event Action<bool> LichtEmpfangen;

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://aleksssandra-001-site1.anytempurl.com/notificationHub") // oder lokale IP im LAN
                .WithAutomaticReconnect()
                .Build();

           
            _hubConnection.On<bool>("LichtGeaendert", async (isLicht) =>
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

           

            try
            {
                await _hubConnection.StartAsync();
               

                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.SendAsync("RegisterClient", $"{DeviceInfo.Name}"); // Optional: Client-Registrierung
                   
                }
                else
                {
                    Console.WriteLine("SignalR-Verbindung konnte nicht hergestellt werden.");
                }
            }
            catch (Exception ex)
            {

                throw;
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
