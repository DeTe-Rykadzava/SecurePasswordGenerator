using System;
using System.Threading.Tasks;
using Avalonia.Input.Platform;
using SecurePasswordManager.Core.Interfaces;

namespace SecurePasswordManager.App;

public class AvaloniaClipboardService : IClipboardService
{
    private readonly IClipboard? _clipboard;

    public AvaloniaClipboardService(IClipboard? clipboard)
    {
        _clipboard = clipboard;
    }

    public async Task SetTextAsync(string text)
    {
         if (_clipboard != null)
        {
            await _clipboard.SetTextAsync(text);
        }
    }
}
