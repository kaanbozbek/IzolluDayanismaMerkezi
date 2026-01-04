using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Application-wide system settings. Only one row should exist.
/// </summary>
public class SystemSettings
{
    public int Id { get; set; }

    /// <summary>
    /// The currently active term ID (if any)
    /// </summary>
    public int? ActiveTermId { get; set; }

    /// <summary>
    /// Application version or other metadata
    /// </summary>
    [StringLength(50)]
    public string? AppVersion { get; set; }

    /// <summary>
    /// Last time settings were updated
    /// </summary>
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional notes about system configuration
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }
}
