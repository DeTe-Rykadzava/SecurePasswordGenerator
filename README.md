# 🔐 Secure Password Manager



[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)

![Avalonia](https://img.shields.io/badge/Framework-.NET%2010%20|%20Avalonia%20UI-141A16?style=flat-square)

![License](https://img.shields.io/badge/License-GPL--3.0-green)

![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)

Современный, кроссплатформенный десктопный менеджер паролей с генератором криптостойких паролей и локальным зашифрованным хранилищем. Ваши данные никогда не покидают ваше устройство.

## ✨ Особенности
- 🛡️ **Криптографически стойкая генерация:** Использование `RandomNumberGenerator` (CSPRNG) вместо стандартных генераторов псевдослучайных чисел.

- 🔒 **Военный уровень шифрования:** AES-256-CBC с выводом ключа через PBKDF2 (SHA-256, 10 000 итераций) для надежной защиты каждой записи.

- 💻 **Истинная кроссплатформенность:** Нативная работа на Windows, macOS и Linux благодаря **Avalonia UI**.

- 🗄️ **Локальное хранение:** Данные хранятся в зашифрованной локальной базе данных SQLite. Никаких облаков и подписок.

- 📊 **Оценка надежности:** Встроенный анализатор сложности пароля в реальном времени.

- 🚀 **Оптимизированная сборка:** Поддержка Single-File публикации с обрезкой (trimming) и сжатием для минимального размера исполняемого файла.

## 🛠️ Стек технологий

| Категория | Технология |
| :--- | :--- |
| **Язык** | C# 13 (.NET 10) |
| **UI фреймворк** | Avalonia UI 12.x |
| **Архитектура** | MVVM (`CommunityToolkit.Mvvm`) |
| **База данных** | Entity Framework Core 10 + SQLite |
| **Безопасность** | `System.Security.Cryptography` (AES-256, PBKDF2, CSPRNG) |
| **DI Контейнер** | `Microsoft.Extensions.DependencyInjection` |

## 📂 Структура проекта

Проект разделен на три логических слоя для обеспечения чистой архитектуры и тестируемости:

- 📁 **`SecurePasswordManager.App`** – Слой представления (UI на Avalonia, ViewModels, точка входа, настройка DI).

- 📁 **`SecurePasswordManager.Core`** – Бизнес-логика (генерация паролей, сервис шифрования `AesEncryptionService`, оценка надежности).

- 📁 **`SecurePasswordManager.Database`** – Слой данных (EF Core `DbContext`, миграции, модель `PasswordEntry`).

## 🚀 Начало работы

### Предварительные требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) или новее.

- IDE: Visual Studio 2022, JetBrains Rider или VS Code с расширением \*C# Dev Kit\*.

### Запуск в режиме разработки

```bash
# 1. Клонируйте репозиторий

git clone https://github.com/DeTe-Rykadzava/SecurePasswordGenerator.git

cd SecurePasswordGenerator

# 2. Восстановите зависимости

dotnet restore

# 3. Запустите приложение

dotnet run --project SecurePasswordManager.App
```

### Публикация (Single-File)
Проект уже настроен для создания компактного автономного исполняемого файла. Для публикации под вашу ОС используйте:
```bash
# Для Windows (x64)

dotnet publish SecurePasswordManager.App -c Release -r win-x64 --self-contained true

# Для Linux (x64)

dotnet publish SecurePasswordManager.App -c Release -r linux-x64 --self-contained true

# Для macOS (Apple Silicon / arm64)

dotnet publish SecurePasswordManager.App -c Release -r osx-arm64 --self-contained true
```

*Собранный исполняемый файл будет находиться в папке `SecurePasswordManager.App/bin/Release/net10.0/<runtime>/publish/`.*

## 🛡️ Детали безопасности

1. **Генерация:** Пароли создаются с помощью `System.Security.Cryptography.RandomNumberGenerator`, что гарантирует криптографическую стойкость.

2. **Шифрование:** Каждый пароль шифруется индивидуально. Вектор инициализации (IV) генерируется случайно для каждой записи и хранится вместе с зашифрованными данными в формате Base64.

3. **Защита мастер-пароля:** Ключ шифрования выводится из мастер-пароля пользователя с использованием алгоритма PBKDF2 с 10 000 итераций и хэш-функцией SHA-256, что надежно защищает от атак перебором (brute-force) и радужных таблиц.

## 📸 Скриншоты

*(Здесь можно добавить скриншоты интерфейса приложения)*

![Главный экран](https://github.com/DeTe-Rykadzava/SecurePasswordGenerator/blob/main/Assets/Main%20Window.png)

## 🤝 Вклад в проект
Pull Request'ы приветствуются! Для серьезных изменений, пожалуйста, сначала откройте [Issue](https://github.com/DeTe-Rykadzava/SecurePasswordGenerator/issues), чтобы мы могли обсудить, что именно вы хотите изменить или добавить.

## 📄 Лицензия

Этот проект распространяется под лицензией [GNU General Public License v3.0](LICENSE). Это означает, что вы можете свободно использовать, изучать и модифицировать код, при условии сохранения открытости производных работ.


