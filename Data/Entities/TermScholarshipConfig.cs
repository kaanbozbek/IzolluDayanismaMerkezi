using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Stores the scholarship amount configuration for a specific term.
/// This allows different scholarship amounts for different academic/financial periods.
/// </summary>
public class TermScholarshipConfig
{
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the Term this configuration applies to
    /// </summary>
    [Required]
    public int TermId { get; set; }

    /// <summary>
    /// Navigation property to the Term
    /// </summary>
    public virtual Term Term { get; set; } = default!;

    /// <summary>
    /// Full academic year scholarship amount (e.g., 36000 TL per year)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal YearlyAmount { get; set; }

    /// <summary>
    /// Monthly scholarship amount (typically YearlyAmount / 12)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyAmount { get; set; }

    /// <summary>
    /// When this configuration was last updated
    /// </summary>
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional notes about this scholarship configuration
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
