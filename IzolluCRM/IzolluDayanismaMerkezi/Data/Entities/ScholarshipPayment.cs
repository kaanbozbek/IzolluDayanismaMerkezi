using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Represents an actual scholarship payment made to a student,
/// linked to a member's commitment.
/// </summary>
public class ScholarshipPayment
{
    public int Id { get; set; }

    // Foreign Keys
    /// <summary>
    /// The commitment this payment is associated with
    /// </summary>
    public int CommitmentId { get; set; }

    /// <summary>
    /// The student who received this payment
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// The term this payment belongs to (optional for legacy data)
    /// </summary>
    public int? TermId { get; set; }

    // Navigation properties
    public virtual MemberScholarshipCommitment Commitment { get; set; } = default!;
    public virtual Student Student { get; set; } = default!;

    /// <summary>
    /// When the payment was made
    /// </summary>
    [Required]
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Amount paid
    /// </summary>
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment type (e.g., "Monthly", "Annual", "One-time")
    /// </summary>
    [StringLength(50)]
    public string? PaymentType { get; set; }

    /// <summary>
    /// Payment method (e.g., "Bank Transfer", "Cash", "Check")
    /// </summary>
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Transaction reference or receipt number
    /// </summary>
    [StringLength(100)]
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Optional notes about this payment
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Status of the payment (e.g., "Completed", "Pending", "Failed")
    /// </summary>
    [StringLength(50)]
    public string Status { get; set; } = "Completed";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
