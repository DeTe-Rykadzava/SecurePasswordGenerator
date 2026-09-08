namespace SecurePasswordManager.Core.Models;

public class PasswordGenerationOptions
{

    public int Length { get; set; } = 16;
    public bool IncludeUppercase { get; set; } = true;
    public bool IncludeLowercase { get; set; } = true;
    public bool IncludeNumbers { get; set; } = true;
    public bool IncludeSpecial { get; set; } = true;

}
