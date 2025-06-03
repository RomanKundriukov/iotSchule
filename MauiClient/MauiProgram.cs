using MauiClient.Service;
using Microsoft.Extensions.Logging;

namespace MauiClient
{
    public static class MauiProgram
    {
        private static readonly SignalRService _signalR = new();

        public static MauiApp CreateMauiApp()
        {
            Task.Run(() => _signalR.StartAsync());

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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
