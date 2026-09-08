using System.Security.Cryptography;
using System.Text;
using SecurePasswordManager.Core.Interfaces;
using SecurePasswordManager.Core.Models;

namespace SecurePasswordManager.Core.Services;

public class PasswordGenerator : IPasswordGenerator
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string Special = "!@#$%^&*()_-+=[]{}|;:,.<>?";

    public string Generate(PasswordGenerationOptions options)
    {
        var charSets = new StringBuilder();
        if (options.IncludeLowercase) charSets.Append(Lowercase);
        if (options.IncludeUppercase) charSets.Append(Uppercase);
        if (options.IncludeNumbers) charSets.Append(Numbers);
        if (options.IncludeSpecial) charSets.Append(Special);

        if (charSets.Length == 0)
            throw new ArgumentException("At least one character set must be selected.");

        string availableChars = charSets.ToString();
        var result = new char[options.Length];
        byte[] randomBytes = new byte[options.Length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < options.Length; i++)
        {
            result[i] = availableChars[randomBytes[i] % availableChars.Length];
        }

        return new string(result);
    }
}
