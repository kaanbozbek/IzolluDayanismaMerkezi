using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

public class Village
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Ad { get; set; } = string.Empty;

    public int? Nufus { get; set; }

    public int? IlkokulOgrenciSayisi { get; set; }

    public int? OrtaokulOgrenciSayisi { get; set; }

    public int? LiseOgrenciSayisi { get; set; }

    public int? UniversiteOgrenciSayisi { get; set; }

    [StringLength(100)]
    public string? MuhtarAdi { get; set; }

    [StringLength(20)]
    public string? MuhtarTelefon { get; set; }

    [StringLength(200)]
    public string? Notlar { get; set; }

    public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

    public DateTime? GuncellemeTarihi { get; set; }
}
