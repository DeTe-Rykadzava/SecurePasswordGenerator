namespace SecurePasswordManager.Core.Interfaces;

public interface IClipboardService
{
    Task SetTextAsync(string text);
}
