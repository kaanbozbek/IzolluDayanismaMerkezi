using System.ComponentModel.DataAnnotations;

namespace IzolluVakfi.Data.Entities;

public class ActivityLog
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string KullaniciAdi { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string IslemTipi { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Detay { get; set; } = string.Empty;

    public DateTime Tarih { get; set; }
}
