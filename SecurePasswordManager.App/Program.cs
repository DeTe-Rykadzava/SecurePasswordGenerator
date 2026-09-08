using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SecurePasswordManager.App;

sealed class Program
{
    
     // Статический контейнер сервисов, доступный из любой точки приложения
    public static ServiceProvider Services { get; private set; } = null!;

    [STAThread]
    public static void Main(string[] args)
    { 
        Services = Infrastructure.ServiceCollection.ConfigureServices();

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        finally
        {
            Services.Dispose();
        }

    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
