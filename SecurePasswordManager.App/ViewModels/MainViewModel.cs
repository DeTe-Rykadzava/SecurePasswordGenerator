using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using SecurePasswordManager.Core.Interfaces;
using SecurePasswordManager.Core.Models;
using SecurePasswordManager.Database.Context;
using SecurePasswordManager.Database.Models;
using SecurePasswordManager.App.Infrastructure;

namespace SecurePasswordManager.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEncryptionService _encryptionService;
    private readonly AppDbContext _dbContext;
    private readonly IClipboardService _clipboardService;
    private readonly IPasswordStrengthEvaluator _strengthEvaluator;

    private string _masterPassword = string.Empty;
    private string _title = string.Empty;
    private string _generatedPassword = string.Empty;
    private string _decryptedPassword = string.Empty;
    private int _passwordLength = 16;
    private bool _includeUppercase = true;
    private bool _includeLowercase = true;
    private bool _includeNumbers = true;
    private bool _includeSpecial = true;
    private string _statusMessage = string.Empty;
    private string _passwordStrengthText = "Неизвестно";
    private string _passwordStrengthColor = "Gray";


    public ObservableCollection<PasswordEntry> Passwords { get; } = new();

    public string MasterPassword
    {
        get => _masterPassword;
        set => SetProperty(ref _masterPassword, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string GeneratedPassword
    {
        get => _generatedPassword;
        set => SetProperty(ref _generatedPassword, value);
    }

    public int PasswordLength
    {
        get => _passwordLength;
        set => SetProperty(ref _passwordLength, value);
    }

    public bool IncludeUppercase
    {
        get => _includeUppercase;
        set => SetProperty(ref _includeUppercase, value);
    }

    public bool IncludeLowercase
    {
        get => _includeLowercase;
        set => SetProperty(ref _includeLowercase, value);
    }

    public bool IncludeNumbers
    {
        get => _includeNumbers;
        set => SetProperty(ref _includeNumbers, value);
    }

    public bool IncludeSpecial
    {
        get => _includeSpecial;
        set => SetProperty(ref _includeSpecial, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    // Новое свойство для отображения расшифрованного пароля
    public string DecryptedPassword
    {
        get => _decryptedPassword;
        set => SetProperty(ref _decryptedPassword, value);
    }
    public string PasswordStrengthText
    {
        get => _passwordStrengthText;
        set => SetProperty(ref _passwordStrengthText, value);
    }

    public string PasswordStrengthColor
    {
        get => _passwordStrengthColor;
        set => SetProperty(ref _passwordStrengthColor, value);
    }


    public ICommand GeneratePasswordCommand { get; }
    public ICommand SavePasswordCommand { get; }
    public ICommand LoadPasswordsCommand { get; }
    public ICommand ShowPasswordCommand { get; } // Новая команда
    public ICommand CopyPasswordCommand { get; } // Новая команда
    public ICommand DeletePasswordCommand { get; }

    public MainViewModel()
    {
        _passwordGenerator = Program.Services.GetRequiredService<IPasswordGenerator>();
        _encryptionService = Program.Services.GetRequiredService<IEncryptionService>();
        _dbContext = Program.Services.GetRequiredService<AppDbContext>();
        _clipboardService = Program.Services.GetRequiredService<IClipboardService>();
        _strengthEvaluator = Program.Services.GetRequiredService<IPasswordStrengthEvaluator>();

        GeneratePasswordCommand = new RelayCommand(GeneratePassword);
        SavePasswordCommand = new RelayCommand(SavePassword);
        LoadPasswordsCommand = new RelayCommand(LoadPasswords);
        ShowPasswordCommand = new RelayCommand<PasswordEntry>(ShowPassword);
        CopyPasswordCommand = new RelayCommand(CopyPassword);
        DeletePasswordCommand = new RelayCommand<PasswordEntry>(DeletePassword);

        LoadPasswords();
    }

    private void GeneratePassword()
    {
        var options = new PasswordGenerationOptions
        {
            Length = PasswordLength,
            IncludeUppercase = IncludeUppercase,
            IncludeLowercase = IncludeLowercase,
            IncludeNumbers = IncludeNumbers,
            IncludeSpecial = IncludeSpecial
        };

        GeneratedPassword = _passwordGenerator.Generate(options);

        PasswordStrengthText = _strengthEvaluator.GetStrengthText(GeneratedPassword);
        PasswordStrengthColor = _strengthEvaluator.GetStrengthColor(GeneratedPassword);

        StatusMessage = "Пароль сгенерирован";
    }

    private void SavePassword()
    {
        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            StatusMessage = "Введите мастер-пароль";
            return;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            StatusMessage = "Введите название";
            return;
        }

        if (string.IsNullOrWhiteSpace(GeneratedPassword))
        {
            StatusMessage = "Сначала сгенерируйте пароль";
            return;
        }

        try
        {
            var encryptedData = _encryptionService.Encrypt(GeneratedPassword, MasterPassword);

            var entry = new PasswordEntry
            {
                Title = Title,
                EncryptedData = encryptedData,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Passwords.Add(entry);
            _dbContext.SaveChanges();

            StatusMessage = "Пароль сохранен";
            Title = string.Empty;
            GeneratedPassword = string.Empty;

            LoadPasswords();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private void LoadPasswords()
    {
        Passwords.Clear();
        var entries = _dbContext.Passwords.ToList();
        foreach (var entry in entries)
        {
            Passwords.Add(entry);
        }
    }

     // Новая логика: Расшифровка
    private void ShowPassword(PasswordEntry? entry)
    {
        if (entry == null) return;
        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            StatusMessage = "Введите мастер-пароль для расшифровки!";
            DecryptedPassword = string.Empty;
            return;
        }

        try
        {
            DecryptedPassword = _encryptionService.Decrypt(entry.EncryptedData, MasterPassword);
            StatusMessage = $"Пароль '{entry.Title}' расшифрован";
        }
        catch (Exception)
        {
            StatusMessage = "Ошибка расшифровки: неверный мастер-пароль!";
            DecryptedPassword = string.Empty;
        }
    }

    // Новая логика: Копирование в буфер обмена
    private async void CopyPassword()
    {
        if (string.IsNullOrWhiteSpace(DecryptedPassword))
        {
            StatusMessage = "Нечего копировать";
            return;
        }

        await _clipboardService.SetTextAsync(DecryptedPassword);
        StatusMessage = "Пароль скопирован в буфер обмена!";
    }

    private void DeletePassword(PasswordEntry? entry)
    {
        if (entry == null) return;
    
        // Удаляем из базы данных
        _dbContext.Passwords.Remove(entry);
        _dbContext.SaveChanges();
        
        StatusMessage = $"Запись '{entry.Title}' удалена";
        
        // Если мы просматривали именно этот пароль, очищаем поле расшифровки
        if (DecryptedPassword != string.Empty) 
        {
            DecryptedPassword = string.Empty;
        }
        
        LoadPasswords();
    }

}