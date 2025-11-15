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
            .FirstOrDefaultAsync();

        if (settings == null)
        {
            // Create default settings
            settings = new SystemSettings
            {
                LastUpdated = DateTime.UtcNow,
                AppVersion = "1.0.0"
            };

            _context.SystemSettings.Add(settings);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created initial system settings");
        }

        return settings;
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
