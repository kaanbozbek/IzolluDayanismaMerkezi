using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

/// <summary>
/// Service for generating term-based reports using ScholarshipPayment data.
/// This service has been refactored to use the new architecture where Students and Members
/// are term-independent, and all term-based scholarship tracking happens through ScholarshipPayment.
/// </summary>
public class TermReportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TermReportService> _logger;

    public TermReportService(ApplicationDbContext context, ILogger<TermReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ====================================================================================
    // STUDENT SCHOLARSHIP QUERIES (Using ScholarshipPayment)
    // ====================================================================================

    /// <summary>
    /// Gets count of students receiving scholarships in the specified term.
    /// </summary>
    public async Task<int> GetActiveScholarshipCountByTermAsync(int termId)
    {
        return await _context.ScholarshipPayments
            .Where(sp => sp.TermId == termId)
            .Select(sp => sp.StudentId)
            .Distinct()
            .CountAsync();
    }

    /// <summary>
    /// Gets count of graduated students in the specified term.
    /// </summary>
    public async Task<int> GetGraduatedCountByTermAsync(int termId)
    {
        // Get students who received payments in this term and are now graduated
        var studentIdsInTerm = await _context.ScholarshipPayments
            .Where(sp => sp.TermId == termId)
            .Select(sp => sp.StudentId)
            .Distinct()
            .ToListAsync();

        return await _context.Students
            .Where(s => studentIdsInTerm.Contains(s.Id) && s.MezunMu)
            .CountAsync();
    }

    /// <summary>
    /// Gets total student count participating in the specified term.
    /// </summary>
    public async Task<int> GetTotalStudentCountByTermAsync(int termId)
    {
        return await _context.ScholarshipPayments
            .Where(sp => sp.TermId == termId)
            .Select(sp => sp.StudentId)
            .Distinct()
            .CountAsync();
    }

    /// <summary>
    /// Gets detailed list of students receiving active scholarships in the term.
    /// </summary>
    public async Task<List<StudentTermDetailDto>> GetActiveScholarshipStudentsAsync(int termId)
    {
        var termConfig = await _context.TermScholarshipConfigs
            .FirstOrDefaultAsync(c => c.TermId == termId);

        var paymentsQuery = _context.ScholarshipPayments
            .Where(sp => sp.TermId == termId)
            .Include(sp => sp.Student)
            .Include(sp => sp.Commitment)
                .ThenInclude(c => c.Member)
            .AsQueryable();

        var payments = await paymentsQuery.ToListAsync();

        var studentDetails = payments
            .GroupBy(sp => sp.StudentId)
            .Select(g =>
            {
                var firstPayment = g.First();
                var student = firstPayment.Student;
                var member = firstPayment.Commitment?.Member;

                return new StudentTermDetailDto
                {
                    StudentId = student.Id,
                    StudentName = student.AdSoyad,
                    University = student.Universite,
                    Department = student.Bolum,
                    ClassLevel = student.Sinif,
                    MonthlyAmount = termConfig?.MonthlyAmount ?? 0,
                    YearlyAmount = termConfig?.YearlyAmount ?? 0,
                    DonorName = member?.AdSoyad ?? student.BagisciAdi,
                    IsActive = student.AktifBursMu,
                    IsGraduated = student.MezunMu,
                    GPA = null, // GPA calculation would need separate logic
                    PaymentCount = g.Count(),
                    TotalPaid = g.Sum(sp => sp.Amount)
                };
            })
            .OrderBy(s => s.StudentName)
            .ToList();

        return studentDetails;
    }

    /// <summary>
    /// Gets detailed list of graduated students in the term.
    /// </summary>
    public async Task<List<StudentTermDetailDto>> GetGraduatedStudentsAsync(int termId)
    {
        var allStudents = await GetActiveScholarshipStudentsAsync(termId);
        return allStudents.Where(s => s.IsGraduated).ToList();
    }

    // ====================================================================================
    // MEMBER/DONOR QUERIES (Using MemberScholarshipCommitment)
    // ====================================================================================

    /// <summary>
    /// Gets count of active members providing scholarships in the term.
    /// </summary>
    public async Task<int> GetScholarshipProvidersCountByTermAsync(int termId)
    {
        return await _context.MemberScholarshipCommitments
            .Where(c => c.TermId == termId && c.PledgedCount > 0)
            .CountAsync();
    }

    /// <summary>
    /// Gets count of executive board members (from Member entity, not term-specific).
    /// </summary>
    public async Task<int> GetExecutiveBoardCountAsync()
    {
        return await _context.Members
            .Where(m => m.IsYonetimKurulu && m.AktifMi)
            .CountAsync();
    }

    // ====================================================================================
    // SCHOLARSHIP SUMMARY
    // ====================================================================================

    /// <summary>
    /// Gets scholarship summary for a specific term.
    /// </summary>
    public async Task<ScholarshipSummaryDto> GetScholarshipSummaryByTermAsync(int termId)
    {
        var termConfig = await _context.TermScholarshipConfigs
            .FirstOrDefaultAsync(c => c.TermId == termId);

        var activeCount = await GetActiveScholarshipCountByTermAsync(termId);
        var graduatedCount = await GetGraduatedCountByTermAsync(termId);

        // Get total commitments for this term
        var totalCommitted = await _context.MemberScholarshipCommitments
            .Where(c => c.TermId == termId)
            .SumAsync(c => c.PledgedCount);

        // Get realized count from actual payments
        var realizedCount = await _context.ScholarshipPayments
            .Where(sp => sp.TermId == termId)
            .Select(sp => sp.StudentId)
            .Distinct()
            .CountAsync();

        var monthlyAmount = termConfig?.MonthlyAmount ?? 0;
        var yearlyAmount = termConfig?.YearlyAmount ?? 0;

        return new ScholarshipSummaryDto
        {
            TermId = termId,
            ActiveScholarshipStudents = activeCount,
            GraduatedStudents = graduatedCount,
            TotalStudents = activeCount + graduatedCount,
            TotalCommitted = totalCommitted,
            TotalRealized = realizedCount,
            MonthlyAmountPerStudent = monthlyAmount,
            YearlyAmountPerStudent = yearlyAmount,
            TotalMonthlyAmount = monthlyAmount * realizedCount,
            TotalYearlyAmount = yearlyAmount * realizedCount,
            ExecutiveBoardMembers = await GetExecutiveBoardCountAsync(),
            ScholarshipProviders = await GetScholarshipProvidersCountByTermAsync(termId)
        };
    }
}

// ====================================================================================
// DTOs
// ====================================================================================

public class StudentTermDetailDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string? University { get; set; }
    public string? Department { get; set; }
    public int? ClassLevel { get; set; }
    public decimal MonthlyAmount { get; set; }
    public decimal YearlyAmount { get; set; }
    public string? DonorName { get; set; }
    public bool IsActive { get; set; }
    public bool IsGraduated { get; set; }
    public decimal? GPA { get; set; }
    public int PaymentCount { get; set; }
    public decimal TotalPaid { get; set; }
}

public class ScholarshipSummaryDto
{
    public int TermId { get; set; }
    public int ActiveScholarshipStudents { get; set; }
    public int GraduatedStudents { get; set; }
    public int TotalStudents { get; set; }
    public int TotalCommitted { get; set; }
    public int TotalRealized { get; set; }
    public decimal MonthlyAmountPerStudent { get; set; }
    public decimal YearlyAmountPerStudent { get; set; }
    public decimal TotalMonthlyAmount { get; set; }
    public decimal TotalYearlyAmount { get; set; }
    public int ExecutiveBoardMembers { get; set; }
    public int ScholarshipProviders { get; set; }
}
