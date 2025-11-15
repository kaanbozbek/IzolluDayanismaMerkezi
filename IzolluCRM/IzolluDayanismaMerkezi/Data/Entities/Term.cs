using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Represents an academic/financial period (term) for the scholarship program.
/// Only one Term can be active at a time.
/// </summary>
public class Term
{
    public int Id { get; set; }

    /// <summary>
    /// Start date of the term period
    /// </summary>
    [Required]
    public DateTime Start { get; set; }

    /// <summary>
    /// End date of the term period
    /// </summary>
    [Required]
    public DateTime End { get; set; }

    /// <summary>
    /// Display name for the term (e.g., "2025-2026")
    /// </summary>
    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is the currently active term
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Optional description or notes about this term
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<StudentTerm> StudentTerms { get; set; } = new List<StudentTerm>();
    public virtual ICollection<MemberTermRole> MemberTermRoles { get; set; } = new List<MemberTermRole>();
}
