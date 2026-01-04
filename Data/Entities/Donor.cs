using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

public class Donor
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string AdSoyad { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Meslek { get; set; }

    [StringLength(100)]
    public string? Sektor { get; set; }

    [StringLength(200)]
    public string? Firma { get; set; }

    [StringLength(200)]
    public string? Sirket { get; set; }

    [StringLength(20)]
    public string? Telefon { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Adres { get; set; }

    public int BursAdedi { get; set; }

    public decimal BirimBursTutari { get; set; }

    public decimal ToplamTutar { get; set; }

    public DateTime? IlkBursVerdigiTarih { get; set; }

    public DateTime? SonBursVerdigiTarih { get; set; }

    public DateTime? BursVermeTarihi { get; set; }

    [StringLength(500)]
    public string? Notlar { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }
}
