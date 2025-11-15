using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Represents a Member's role for a specific Term.
/// This allows tracking board positions and roles across different periods.
/// </summary>
public class MemberTermRole
{
    public int Id { get; set; }

    // Foreign Keys
    public int MemberId { get; set; }
    public int TermId { get; set; }

    // Navigation properties
    public virtual Member Member { get; set; } = default!;
    public virtual Term Term { get; set; } = default!;

    // TERM-DEPENDENT FIELDS
    
    /// <summary>
    /// The role/position of the member in this term
    /// Examples: "Yönetim Kurulu", "Mütevelli", "Denetim Kurulu", "Üye"
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Role { get; set; } = default!;

    /// <summary>
    /// Is this role active for this member in this term?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Is this member on the board of trustees (Mütevelli) in this term?
    /// </summary>
    public bool IsBoardOfTrustees { get; set; }

    /// <summary>
    /// Is this member on the executive board (Yönetim Kurulu) in this term?
    /// </summary>
    public bool IsExecutiveBoard { get; set; }

    /// <summary>
    /// Is this member on the audit committee (Denetim Kurulu) in this term?
    /// </summary>
    public bool IsAuditCommittee { get; set; }

    /// <summary>
    /// Is this member providing scholarships in this term?
    /// </summary>
    public bool IsProvidingScholarship { get; set; }

    /// <summary>
    /// Membership status in this term (e.g., "Aktif", "Pasif", "İstifa Etti")
    /// </summary>
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// When this role started
    /// </summary>
    public DateTime? RoleStartDate { get; set; }

    /// <summary>
    /// When this role ended (if applicable)
    /// </summary>
    public DateTime? RoleEndDate { get; set; }

    /// <summary>
    /// Notes specific to this role in this term
    /// </summary>
    [StringLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
