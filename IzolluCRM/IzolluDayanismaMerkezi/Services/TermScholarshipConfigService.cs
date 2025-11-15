using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class TermScholarshipConfigService
{
    private readonly ApplicationDbContext _context;

    public TermScholarshipConfigService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the scholarship configuration for a specific term.
    /// If it doesn't exist, creates one with default values.
    /// </summary>
    public async Task<TermScholarshipConfig> GetOrCreateForTermAsync(int termId)
    {
        var config = await _context.TermScholarshipConfigs
            .Include(c => c.Term)
            .FirstOrDefaultAsync(c => c.TermId == termId);

        if (config == null)
        {
            // Create default configuration
            config = new TermScholarshipConfig
            {
                TermId = termId,
                YearlyAmount = 36000m, // Default 36000 TL per year
                MonthlyAmount = 3000m,  // Default 3000 TL per month
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            _context.TermScholarshipConfigs.Add(config);
            await _context.SaveChangesAsync();

            // Load the term navigation property
            config = await _context.TermScholarshipConfigs
                .Include(c => c.Term)
                .FirstAsync(c => c.Id == config.Id);
        }

        return config;
    }

    /// <summary>
    /// Gets all term scholarship configurations ordered by term start date (newest first)
    /// </summary>
    public async Task<List<TermScholarshipConfig>> GetAllAsync()
    {
        return await _context.TermScholarshipConfigs
            .Include(c => c.Term)
            .OrderByDescending(c => c.Term.Start)
            .ToListAsync();
    }

    /// <summary>
    /// Updates the scholarship amount for a specific term
    /// </summary>
    public async Task<TermScholarshipConfig> UpdateAsync(int termId, decimal yearlyAmount)
    {
        var config = await GetOrCreateForTermAsync(termId);

        config.YearlyAmount = yearlyAmount;
        config.MonthlyAmount = yearlyAmount / 12m;
        config.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return config;
    }

    /// <summary>
    /// Updates the scholarship amount using monthly amount
    /// </summary>
    public async Task<TermScholarshipConfig> UpdateByMonthlyAmountAsync(int termId, decimal monthlyAmount)
    {
        var config = await GetOrCreateForTermAsync(termId);

        config.MonthlyAmount = monthlyAmount;
        config.YearlyAmount = monthlyAmount * 12m;
        config.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return config;
    }

    /// <summary>
    /// Deletes a term scholarship configuration
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var config = await _context.TermScholarshipConfigs.FindAsync(id);
        if (config != null)
        {
            _context.TermScholarshipConfigs.Remove(config);
            await _context.SaveChangesAsync();
        }
    }
}
