using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecurePasswordManager.Core.Interfaces;
using SecurePasswordManager.Core.Services;
using SecurePasswordManager.Database.Context;

namespace SecurePasswordManager.App.Infrastructure;

public class ServiceCollection
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        // Регистрируем сервисы ядра
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.AddSingleton<IEncryptionService, AesEncryptionService>();

        // Регистрируем DbContext
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SecurePasswordManager",
            "passwords.db"
        );

        // Создаем папку для БД, если её нет
        var dbDirectory = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // Регистрируем сервис буфера обмена как Singleton, но с фабрикой, 
        // чтобы получить доступ к Clipboard текущего TopLevel (окна)
        services.AddSingleton<IClipboardService>(provider => 
        {
            if(App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var topLevel = TopLevel.GetTopLevel(desktop?.MainWindow);
                return new AvaloniaClipboardService(topLevel?.Clipboard);
            }
            return new AvaloniaClipboardService(null);
        });

        services.AddSingleton<IPasswordStrengthEvaluator, PasswordStrengthEvaluator>();

        // Собираем контейнер
        return services.BuildServiceProvider();
    }
}
