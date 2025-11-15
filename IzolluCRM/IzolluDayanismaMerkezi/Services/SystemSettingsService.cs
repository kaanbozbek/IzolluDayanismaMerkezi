using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

/// <summary>
/// Service for managing system-wide settings, particularly the active term.
/// </summary>
public class SystemSettingsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(
        ApplicationDbContext context,
        ILogger<SystemSettingsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets or creates the system settings record (singleton pattern).
    /// </summary>
    public async Task<SystemSettings> GetOrCreateSettingsAsync()
    {
        var settings = await _context.SystemSettings
            .Include(s => s.ActiveTerm)
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            // Create default settings
            var activeTerm = await _context.Terms
                .Where(t => t.IsActive)
                .FirstOrDefaultAsync();

            settings = new SystemSettings
            {
                ActiveTermId = activeTerm?.Id,
                LastUpdated = DateTime.UtcNow,
                AppVersion = "1.0.0"
            };

            _context.SystemSettings.Add(settings);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created initial system settings with ActiveTermId: {TermId}", settings.ActiveTermId);
        }

        return settings;
    }

    /// <summary>
    /// Gets the currently active term ID.
    /// </summary>
    public async Task<int?> GetActiveTermIdAsync()
    {
        var settings = await GetOrCreateSettingsAsync();
        return settings.ActiveTermId;
    }

    /// <summary>
    /// Gets the currently active term (full entity).
    /// </summary>
    public async Task<Term?> GetActiveTermAsync()
    {
        var settings = await _context.SystemSettings
            .Include(s => s.ActiveTerm)
            .FirstOrDefaultAsync();

        return settings?.ActiveTerm;
    }

    /// <summary>
    /// Sets the active term by ID.
    /// This does NOT move or mutate Student/Member data - it only changes the default filter.
    /// </summary>
    public async Task SetActiveTermAsync(int termId)
    {
        // Verify the term exists
        var termExists = await _context.Terms.AnyAsync(t => t.Id == termId);
        if (!termExists)
        {
            throw new ArgumentException($"Term with ID {termId} does not exist.", nameof(termId));
        }

        var settings = await GetOrCreateSettingsAsync();
        settings.ActiveTermId = termId;
        settings.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Active term changed to: {TermId}", termId);
    }

    /// <summary>
    /// Updates the IsActive flag on Terms table to match the SystemSettings.ActiveTermId.
    /// This ensures Term.IsActive stays in sync with the system settings.
    /// </summary>
    public async Task SyncTermActiveStatusAsync()
    {
        var settings = await GetOrCreateSettingsAsync();
        
        if (settings.ActiveTermId == null)
        {
            _logger.LogWarning("No active term set in system settings.");
            return;
        }

        // Set all terms to inactive
        await _context.Terms
            .Where(t => t.IsActive)
            .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsActive, false));

        // Set the active term
        await _context.Terms
            .Where(t => t.Id == settings.ActiveTermId.Value)
            .ExecuteUpdateAsync(t => t.SetProperty(p => p.IsActive, true));

        _logger.LogInformation("Synchronized Term.IsActive flags with ActiveTermId: {TermId}", settings.ActiveTermId);
    }

    /// <summary>
    /// Updates application version in settings.
    /// </summary>
    public async Task UpdateAppVersionAsync(string version)
    {
        var settings = await GetOrCreateSettingsAsync();
        settings.AppVersion = version;
        settings.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates notes in settings.
    /// </summary>
    public async Task UpdateNotesAsync(string? notes)
    {
        var settings = await GetOrCreateSettingsAsync();
        settings.Notes = notes;
        settings.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
