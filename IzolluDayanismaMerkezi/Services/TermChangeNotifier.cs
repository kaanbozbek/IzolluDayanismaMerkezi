namespace IzolluVakfi.Services;

/// <summary>
/// Singleton service for notifying components when the active term changes.
/// This allows real-time updates across the application.
/// </summary>
public class TermChangeNotifier
{
    /// <summary>
    /// Event raised when the active term changes.
    /// Subscribe to this to refresh data when term selection changes.
    /// </summary>
    public event Action? OnTermChanged;

    /// <summary>
    /// Notifies all subscribers that the active term has changed.
    /// Call this after changing the active term via SystemSettingsService.
    /// </summary>
    public void NotifyTermChanged()
    {
        OnTermChanged?.Invoke();
    }
}
