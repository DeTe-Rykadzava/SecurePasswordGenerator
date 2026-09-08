namespace SecurePasswordManager.Database.Models;

public class PasswordEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // Содержит IV + зашифрованный текст в формате Base64
    public string EncryptedData { get; set; } = string.Empty; 
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
