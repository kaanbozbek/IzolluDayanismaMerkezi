namespace IzolluVakfi.Services;

/// <summary>
/// Service to notify components when the active term changes
/// </summary>
public class TermChangeNotifier
{
    public event Action? OnTermChanged;

    public void NotifyTermChanged()
    {
        OnTermChanged?.Invoke();
    }
}
