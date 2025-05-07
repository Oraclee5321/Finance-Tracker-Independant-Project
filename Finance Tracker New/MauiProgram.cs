using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;


namespace Finance_Tracker_New;

public static class MauiProgram
{

    
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<MainPage>();
        
        builder.Services.AddTransient<Dev>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}