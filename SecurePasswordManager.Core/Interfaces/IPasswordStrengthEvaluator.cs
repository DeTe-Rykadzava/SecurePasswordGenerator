namespace SecurePasswordManager.Core.Interfaces;

public interface IPasswordStrengthEvaluator
{
    string GetStrengthText(string password);
    string GetStrengthColor(string password); // Возвращает название цвета для UI
}
