using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

/// <summary>
/// Represents the state of a Student for a specific Term.
/// This is a snapshot model that allows viewing student data across different periods.
/// </summary>
public class StudentTerm
{
    public int Id { get; set; }

    // Foreign Keys
    public int StudentId { get; set; }
    public int TermId { get; set; }

    // Navigation properties
    public virtual Student Student { get; set; } = default!;
    public virtual Term Term { get; set; } = default!;

    // TERM-DEPENDENT FIELDS (snapshot for this specific term)
    
    /// <summary>
    /// Is the scholarship active for this student in this term?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Has the student graduated as of this term?
    /// </summary>
    public bool IsGraduated { get; set; }

    /// <summary>
    /// Monthly scholarship amount for this term
    /// </summary>
    public decimal MonthlyAmount { get; set; }

    /// <summary>
    /// When the scholarship started in this term
    /// </summary>
    public DateTime? ScholarshipStart { get; set; }

    /// <summary>
    /// When the scholarship ended in this term (if applicable)
    /// </summary>
    public DateTime? ScholarshipEnd { get; set; }

    /// <summary>
    /// Grade Point Average for this term
    /// </summary>
    public double? Gpa { get; set; }

    /// <summary>
    /// Current class/year level in this term
    /// </summary>
    public int? ClassLevel { get; set; }

    /// <summary>
    /// Donor name for this term (can change between terms)
    /// </summary>
    [StringLength(200)]
    public string? DonorName { get; set; }

    /// <summary>
    /// Department/Major for this term
    /// </summary>
    [StringLength(200)]
    public string? Department { get; set; }

    /// <summary>
    /// University for this term
    /// </summary>
    [StringLength(200)]
    public string? University { get; set; }

    /// <summary>
    /// Total scholarship received up to and including this term
    /// </summary>
    public decimal TotalScholarshipReceived { get; set; }

    /// <summary>
    /// Notes specific to this term
    /// </summary>
    [StringLength(1000)]
    public string? TermNotes { get; set; }

    /// <summary>
    /// Transcript notes/grades for this term
    /// </summary>
    [StringLength(500)]
    public string? TranscriptNotes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
