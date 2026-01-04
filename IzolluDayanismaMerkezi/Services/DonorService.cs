using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class DonorService
{
    private readonly ApplicationDbContext _context;
    private readonly ActivityLogService _logService;

    public DonorService(ApplicationDbContext context, ActivityLogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public async Task<List<Donor>> GetAllAsync()
    {
        return await _context.Donors
            .OrderBy(d => d.AdSoyad)
            .ToListAsync();
    }

    public async Task<Donor?> GetByIdAsync(int id)
    {
        return await _context.Donors.FindAsync(id);
    }

    public async Task<Donor> CreateAsync(Donor donor)
    {
        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();

        await _logService.LogAsync("IsAdamiEkle", $"Yeni iş adamı eklendi: {donor.AdSoyad}");

        return donor;
    }

    public async Task<Donor> UpdateAsync(Donor donor)
    {
        _context.Donors.Update(donor);
        await _context.SaveChangesAsync();

        await _logService.LogAsync("IsAdamiGuncelle", $"İş adamı güncellendi: {donor.AdSoyad}");

        return donor;
    }

    public async Task DeleteAsync(int id)
    {
        var donor = await _context.Donors.FindAsync(id);
        if (donor != null)
        {
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();

            await _logService.LogAsync("IsAdamiSil", $"İş adamı silindi: {donor.AdSoyad}");
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Donors.CountAsync();
    }

    public async Task<Dictionary<string, decimal>> GetTotalBySectorAsync()
    {
        // SQLite doesn't support Sum on decimal, so we load into memory first
        var donors = await _context.Donors.ToListAsync();
        
        return donors
            .GroupBy(d => d.Sektor ?? "Diğer")
            .ToDictionary(
                g => g.Key, 
                g => g.Sum(d => d.BursAdedi * d.BirimBursTutari)
            );
    }
}