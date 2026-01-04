using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Represents a Member's scholarship pledge/commitment.
/// This tracks how many scholarships a member pledged to provide and how many were actually given.
/// </summary>
public class MemberScholarshipCommitment
{
    public int Id { get; set; }

    // Foreign Keys
    public int MemberId { get; set; }

    /// <summary>
    /// The term this commitment belongs to (optional for legacy data)
    /// </summary>
    public int? TermId { get; set; }

    // Navigation properties
    public virtual Member Member { get; set; } = default!;

    /// <summary>
    /// Number of scholarships this member pledged to provide
    /// </summary>
    [Required]
    public int PledgedCount { get; set; }

    /// <summary>
    /// Number of scholarships actually given/assigned
    /// </summary>
    public int GivenCount { get; set; }

    /// <summary>
    /// Yearly amount per scholarship
    /// (e.g., 36000 TL per year per scholarship)
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal YearlyAmountPerScholarship { get; set; }

    /// <summary>
    /// Academic year/period (e.g., "2025-2026")
    /// </summary>
    [StringLength(50)]
    public string? AcademicYear { get; set; }

    /// <summary>
    /// Computed: Remaining scholarships not yet assigned
    /// </summary>
    [NotMapped]
    public int RemainingCount => PledgedCount - GivenCount;

    /// <summary>
    /// Computed: Total yearly amount for all pledged scholarships
    /// </summary>
    [NotMapped]
    public decimal TotalYearlyAmount => PledgedCount * YearlyAmountPerScholarship;

    /// <summary>
    /// Computed: Total monthly amount for all pledged scholarships (8 months period)
    /// </summary>
    [NotMapped]
    public decimal TotalMonthlyAmount => (YearlyAmountPerScholarship / 8m) * PledgedCount;

    /// <summary>
    /// Optional notes about this commitment
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
