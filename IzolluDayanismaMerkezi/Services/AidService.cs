using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

public class AidService
{
    private readonly ApplicationDbContext _context;

    public AidService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Aid>> GetAllAsync()
    {
        return await _context.Aids
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Aid?> GetByIdAsync(int id)
    {
        return await _context.Aids.FindAsync(id);
    }

    public async Task CreateAsync(Aid aid)
    {
        aid.OlusturmaTarihi = DateTime.Now;
        _context.Aids.Add(aid);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Aid aid)
    {
        aid.GuncellemeTarihi = DateTime.Now;
        _context.Aids.Update(aid);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var aid = await _context.Aids.FindAsync(id);
        if (aid != null)
        {
            _context.Aids.Remove(aid);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Aids.CountAsync();
    }

    public async Task<int> GetIzolluluCountAsync()
    {
        return await _context.Aids.CountAsync(a => a.IsIzollulu);
    }

    public async Task<int> GetNonIzolluluCountAsync()
    {
        return await _context.Aids.CountAsync(a => !a.IsIzollulu);
    }
}
