using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

public class Aid
{
    public int Id { get; set; }

    [Required(ErrorMessage = "İsim gereklidir")]
    [StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Adres { get; set; }

    [StringLength(20)]
    public string? Telefon { get; set; }

    [StringLength(100)]
    public string? RefereEden { get; set; }

    public bool IsIzollulu { get; set; }

    [StringLength(100)]
    public string? Koy { get; set; }

    [StringLength(100)]
    public string? Sehir { get; set; }

    [StringLength(500)]
    public string? Notlar { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
}
