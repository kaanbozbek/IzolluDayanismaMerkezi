using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzolluVakfi.Data.Entities;

public class TranscriptRecord
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    [Column(TypeName = "decimal(3,2)")]
    public decimal GNO { get; set; }

    public DateTime GirisTarihi { get; set; }

    [StringLength(500)]
    public string? Notlar { get; set; }

    [StringLength(500)]
    public string? PdfDosyaYolu { get; set; }

    // Navigation property
    [ForeignKey("StudentId")]
    public virtual Student? Student { get; set; }
}
