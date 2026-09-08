using SecurePasswordManager.Core.Models;

namespace SecurePasswordManager.Core.Interfaces;

public interface IPasswordGenerator
{
    string Generate(PasswordGenerationOptions options);
}
