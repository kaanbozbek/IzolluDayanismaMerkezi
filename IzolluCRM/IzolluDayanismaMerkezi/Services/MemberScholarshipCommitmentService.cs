using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

public class MemberScholarshipCommitmentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MemberScholarshipCommitmentService> _logger;

    public MemberScholarshipCommitmentService(
        ApplicationDbContext context,
        ILogger<MemberScholarshipCommitmentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all commitments for a specific term
    /// </summary>
    public async Task<List<MemberScholarshipCommitment>> GetByTermAsync(int termId)
    {
        return await _context.MemberScholarshipCommitments
            .Include(c => c.Member)
            .Include(c => c.Term)
            .Where(c => c.TermId == termId)
            .OrderBy(c => c.Member.AdSoyad)
            .ToListAsync();
    }

    /// <summary>
    /// Get commitment for a specific member in a specific term
    /// </summary>
    public async Task<MemberScholarshipCommitment?> GetByMemberAndTermAsync(int memberId, int termId)
    {
        return await _context.MemberScholarshipCommitments
            .Include(c => c.Member)
            .Include(c => c.Term)
            .FirstOrDefaultAsync(c => c.MemberId == memberId && c.TermId == termId);
    }

    /// <summary>
    /// Get all commitments for a specific member across all terms
    /// </summary>
    public async Task<List<MemberScholarshipCommitment>> GetByMemberAsync(int memberId)
    {
        return await _context.MemberScholarshipCommitments
            .Include(c => c.Member)
            .Include(c => c.Term)
            .Where(c => c.MemberId == memberId)
            .OrderByDescending(c => c.Term.Start)
            .ToListAsync();
    }

    /// <summary>
    /// Create a new commitment
    /// </summary>
    public async Task<MemberScholarshipCommitment> CreateAsync(MemberScholarshipCommitment commitment)
    {
        // Check if commitment already exists for this member+term
        var existing = await _context.MemberScholarshipCommitments
            .FirstOrDefaultAsync(c => c.MemberId == commitment.MemberId && c.TermId == commitment.TermId);

        if (existing != null)
        {
            throw new InvalidOperationException("Bu üye için bu dönemde zaten bir taahhüt kaydı mevcut.");
        }

        commitment.CreatedAt = DateTime.UtcNow;
        _context.MemberScholarshipCommitments.Add(commitment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created commitment: MemberId={MemberId}, TermId={TermId}, PledgedCount={Count}",
            commitment.MemberId, commitment.TermId, commitment.PledgedCount);

        return commitment;
    }

    /// <summary>
    /// Update an existing commitment
    /// </summary>
    public async Task<MemberScholarshipCommitment> UpdateAsync(MemberScholarshipCommitment commitment)
    {
        var existing = await _context.MemberScholarshipCommitments.FindAsync(commitment.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Taahhüt kaydı bulunamadı.");
        }

        existing.PledgedCount = commitment.PledgedCount;
        existing.GivenCount = commitment.GivenCount;
        existing.YearlyAmountPerScholarship = commitment.YearlyAmountPerScholarship;
        existing.Notes = commitment.Notes;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated commitment: Id={Id}, PledgedCount={Pledged}, GivenCount={Given}",
            commitment.Id, commitment.PledgedCount, commitment.GivenCount);

        return existing;
    }

    /// <summary>
    /// Delete a commitment
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var commitment = await _context.MemberScholarshipCommitments.FindAsync(id);
        if (commitment != null)
        {
            _context.MemberScholarshipCommitments.Remove(commitment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted commitment: Id={Id}", id);
        }
    }

    /// <summary>
    /// Get total scholarship statistics for a term
    /// </summary>
    public async Task<(int DonorCount, decimal TotalMonthly, decimal TotalYearly, int TotalPledged, int TotalGiven)> GetTermStatisticsAsync(int termId)
    {
        var commitments = await _context.MemberScholarshipCommitments
            .Where(c => c.TermId == termId)
            .ToListAsync();

        if (!commitments.Any())
        {
            return (0, 0, 0, 0, 0);
        }

        var donorCount = commitments.Select(c => c.MemberId).Distinct().Count();
        var totalYearly = commitments.Sum(c => c.TotalYearlyAmount);
        var totalMonthly = commitments.Sum(c => c.TotalMonthlyAmount);
        var totalPledged = commitments.Sum(c => c.PledgedCount);
        var totalGiven = commitments.Sum(c => c.GivenCount);

        return (donorCount, totalMonthly, totalYearly, totalPledged, totalGiven);
    }

    /// <summary>
    /// Check if a member has any commitment for a specific term
    /// </summary>
    public async Task<bool> HasCommitmentForTermAsync(int memberId, int termId)
    {
        return await _context.MemberScholarshipCommitments
            .AnyAsync(c => c.MemberId == memberId && c.TermId == termId);
    }
}
