using System.Text.RegularExpressions;
using SecurePasswordManager.Core.Interfaces;

namespace SecurePasswordManager.Core.Services;

public class PasswordStrengthEvaluator : IPasswordStrengthEvaluator
{
    public string GetStrengthColor(string password)
    {
        if (string.IsNullOrEmpty(password)) return "Gray";
        
        int score = 0;
        if (password.Length >= 8) score++;
        if (password.Length >= 12) score++;
        if (Regex.IsMatch(password, "[A-Z]")) score++;
        if (Regex.IsMatch(password, "[0-9]")) score++;
        if (Regex.IsMatch(password, "[^a-zA-Z0-9]")) score++;

        if (score <= 2) return "Red";
        if (score <= 4) return "Orange";
        return "Green";
    }

    public string GetStrengthText(string password)
    {
         if (string.IsNullOrEmpty(password)) return "Неизвестно";
        
        int score = 0;
        if (password.Length >= 8) score++;
        if (password.Length >= 12) score++;
        if (Regex.IsMatch(password, "[A-Z]")) score++;
        if (Regex.IsMatch(password, "[0-9]")) score++;
        if (Regex.IsMatch(password, "[^a-zA-Z0-9]")) score++; // Спецсимволы

        if (score <= 2) return "Слабый";
        if (score <= 4) return "Средний";
        return "Сильный";
    }
}
