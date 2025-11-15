using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

public class Member
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string AdSoyad { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Meslek { get; set; }

    [StringLength(20)]
    public string? Telefon { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Adres { get; set; }

    [StringLength(50)]
    public string? SicilNumarasi { get; set; }

    [StringLength(11)]
    public string? TCNo { get; set; }

    public DateTime? DogumTarihi { get; set; }

    public int? Yas { get; set; }

    public bool IsIzollulu { get; set; }

    [StringLength(100)]
    public string? Koy { get; set; }

    [StringLength(100)]
    public string? UyelikTuru { get; set; }

    public DateTime? UyelikBaslangicTarihi { get; set; }

    [StringLength(50)]
    public string? Durum { get; set; }

    public bool IsMutevelli { get; set; }

    public bool IsYonetimKurulu { get; set; }

    public bool IsDenetimKurulu { get; set; }

    public bool BursVeriyor { get; set; }

    public bool AktifMi { get; set; } = true;

    [StringLength(500)]
    public string? Notlar { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    // Navigation properties
    /// <summary>
    /// Collection of term roles for this member.
    /// Each MemberTermRole represents this member's role/position for a specific term.
    /// </summary>
    public virtual ICollection<MemberTermRole> TermRoles { get; set; } = new List<MemberTermRole>();

    /// <summary>
    /// Collection of scholarship commitments for this member.
    /// Each commitment represents pledged scholarships for a specific term.
    /// </summary>
    public virtual ICollection<MemberScholarshipCommitment> ScholarshipCommitments { get; set; } = new List<MemberScholarshipCommitment>();
}
