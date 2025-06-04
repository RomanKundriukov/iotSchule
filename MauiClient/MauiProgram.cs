using MauiClient.Service;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace MauiClient
{
    //test

    public static class MauiProgram
    {
        private static readonly SignalRService _signalR = new();
        //private static readonly ChatService _chat = new();

        public static MauiApp CreateMauiApp()
        {
            Task.Run(() => _signalR.StartAsync());
            //Task.Run(() => _chat.StartAsync());

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



           
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
