using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

/// <summary>
/// Service for generating term-aware reports and statistics.
/// ALL queries in this service MUST filter by TermId to ensure accurate historical data.
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

    // ========================================
    // STUDENT STATISTICS (TERM-AWARE)
    // ========================================

    /// <summary>
    /// Gets the count of active scholarship students for a specific term.
    /// Excludes graduated students.
    /// </summary>
    public async Task<int> GetActiveScholarshipStudentCountAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated)
            .CountAsync();
    }

    /// <summary>
    /// Gets the count of graduated students for a specific term.
    /// </summary>
    public async Task<int> GetGraduatedStudentCountAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsGraduated)
            .CountAsync();
    }

    /// <summary>
    /// Gets the total scholarship amount distributed in a specific term.
    /// This can be computed as sum of MonthlyAmount * months, or TotalScholarshipReceived.
    /// </summary>
    public async Task<decimal> GetTotalScholarshipAmountAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated)
            .SumAsync(st => (decimal?)st.TotalScholarshipReceived) ?? 0m;
    }

    /// <summary>
    /// Gets the total monthly scholarship budget for a specific term.
    /// </summary>
    public async Task<decimal> GetMonthlyScholarshipBudgetAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated)
            .SumAsync(st => (decimal?)st.MonthlyAmount) ?? 0m;
    }

    /// <summary>
    /// Gets the list of active scholarship students for a specific term.
    /// </summary>
    public async Task<List<StudentTermDetailDto>> GetActiveScholarshipStudentsAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated)
            .Include(st => st.Student)
            .Include(st => st.Term)
            .Select(st => new StudentTermDetailDto
            {
                StudentId = st.StudentId,
                StudentName = st.Student.AdSoyad,
                TermId = st.TermId,
                TermName = st.Term.DisplayName,
                MonthlyAmount = st.MonthlyAmount,
                TotalScholarshipReceived = st.TotalScholarshipReceived,
                University = st.University,
                Department = st.Department,
                ClassLevel = st.ClassLevel,
                DonorName = st.DonorName,
                ScholarshipStart = st.ScholarshipStart,
                ScholarshipEnd = st.ScholarshipEnd,
                Gpa = st.Gpa
            })
            .OrderBy(st => st.StudentName)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the list of graduated students for a specific term.
    /// </summary>
    public async Task<List<StudentTermDetailDto>> GetGraduatedStudentsAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsGraduated)
            .Include(st => st.Student)
            .Include(st => st.Term)
            .Select(st => new StudentTermDetailDto
            {
                StudentId = st.StudentId,
                StudentName = st.Student.AdSoyad,
                TermId = st.TermId,
                TermName = st.Term.DisplayName,
                MonthlyAmount = st.MonthlyAmount,
                TotalScholarshipReceived = st.TotalScholarshipReceived,
                University = st.University,
                Department = st.Department,
                ClassLevel = st.ClassLevel,
                DonorName = st.DonorName,
                ScholarshipStart = st.ScholarshipStart,
                ScholarshipEnd = st.ScholarshipEnd,
                Gpa = st.Gpa
            })
            .OrderBy(st => st.StudentName)
            .ToListAsync();
    }

    /// <summary>
    /// Gets scholarship distribution by university for a specific term.
    /// </summary>
    public async Task<List<UniversityStatDto>> GetScholarshipByUniversityAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated 
                      && st.University != null)
            .GroupBy(st => st.University)
            .Select(g => new UniversityStatDto
            {
                University = g.Key!,
                StudentCount = g.Count(),
                TotalMonthlyAmount = g.Sum(st => st.MonthlyAmount),
                TotalScholarship = g.Sum(st => st.TotalScholarshipReceived)
            })
            .OrderByDescending(u => u.StudentCount)
            .ToListAsync();
    }

    /// <summary>
    /// Gets scholarship distribution by class level for a specific term.
    /// </summary>
    public async Task<List<ClassLevelStatDto>> GetScholarshipByClassLevelAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId 
                      && st.IsActive 
                      && !st.IsGraduated 
                      && st.ClassLevel != null)
            .GroupBy(st => st.ClassLevel)
            .Select(g => new ClassLevelStatDto
            {
                ClassLevel = g.Key!.Value,
                StudentCount = g.Count(),
                TotalMonthlyAmount = g.Sum(st => st.MonthlyAmount),
                AverageMonthlyAmount = g.Average(st => st.MonthlyAmount)
            })
            .OrderBy(c => c.ClassLevel)
            .ToListAsync();
    }

    // ========================================
    // MEMBER STATISTICS (TERM-AWARE)
    // ========================================

    /// <summary>
    /// Gets the list of board members (executive board) for a specific term.
    /// </summary>
    public async Task<List<MemberRoleDto>> GetExecutiveBoardMembersAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId 
                       && mtr.IsActive 
                       && mtr.IsExecutiveBoard)
            .Include(mtr => mtr.Member)
            .Include(mtr => mtr.Term)
            .Select(mtr => new MemberRoleDto
            {
                MemberId = mtr.MemberId,
                MemberName = mtr.Member.AdSoyad,
                TermId = mtr.TermId,
                TermName = mtr.Term.DisplayName,
                Role = mtr.Role,
                IsActive = mtr.IsActive,
                IsProvidingScholarship = mtr.IsProvidingScholarship,
                Status = mtr.Status,
                RoleStartDate = mtr.RoleStartDate,
                RoleEndDate = mtr.RoleEndDate
            })
            .OrderBy(m => m.MemberName)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the list of board of trustees members for a specific term.
    /// </summary>
    public async Task<List<MemberRoleDto>> GetBoardOfTrusteesAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId 
                       && mtr.IsActive 
                       && mtr.IsBoardOfTrustees)
            .Include(mtr => mtr.Member)
            .Include(mtr => mtr.Term)
            .Select(mtr => new MemberRoleDto
            {
                MemberId = mtr.MemberId,
                MemberName = mtr.Member.AdSoyad,
                TermId = mtr.TermId,
                TermName = mtr.Term.DisplayName,
                Role = mtr.Role,
                IsActive = mtr.IsActive,
                IsProvidingScholarship = mtr.IsProvidingScholarship,
                Status = mtr.Status,
                RoleStartDate = mtr.RoleStartDate,
                RoleEndDate = mtr.RoleEndDate
            })
            .OrderBy(m => m.MemberName)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the count of active members by role for a specific term.
    /// </summary>
    public async Task<List<MemberRoleCountDto>> GetMemberCountByRoleAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsActive)
            .GroupBy(mtr => mtr.Role)
            .Select(g => new MemberRoleCountDto
            {
                Role = g.Key,
                Count = g.Count(),
                ProvidingScholarshipCount = g.Count(mtr => mtr.IsProvidingScholarship)
            })
            .OrderByDescending(r => r.Count)
            .ToListAsync();
    }

    /// <summary>
    /// Gets the count of members providing scholarships for a specific term.
    /// </summary>
    public async Task<int> GetScholarshipProviderCountAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId 
                       && mtr.IsActive 
                       && mtr.IsProvidingScholarship)
            .CountAsync();
    }

    // ========================================
    // DASHBOARD SUMMARY (TERM-AWARE)
    // ========================================

    /// <summary>
    /// Gets a comprehensive dashboard summary for a specific term.
    /// This includes all key metrics in a single query.
    /// </summary>
    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(int termId)
    {
        var term = await _context.Terms
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == termId);

        if (term == null)
        {
            throw new InvalidOperationException($"Term with ID {termId} not found.");
        }

        var activeStudents = await GetActiveScholarshipStudentCountAsync(termId);
        var graduatedStudents = await GetGraduatedStudentCountAsync(termId);
        var totalScholarshipAmount = await GetTotalScholarshipAmountAsync(termId);
        var monthlyBudget = await GetMonthlyScholarshipBudgetAsync(termId);
        var executiveBoardCount = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsActive && mtr.IsExecutiveBoard)
            .CountAsync();
        var scholarshipProviderCount = await GetScholarshipProviderCountAsync(termId);

        return new DashboardSummaryDto
        {
            TermId = termId,
            TermName = term.DisplayName,
            TermStart = term.Start,
            TermEnd = term.End,
            ActiveScholarshipStudents = activeStudents,
            GraduatedStudents = graduatedStudents,
            TotalStudents = activeStudents + graduatedStudents,
            TotalScholarshipDistributed = totalScholarshipAmount,
            MonthlyScholarshipBudget = monthlyBudget,
            ExecutiveBoardMembers = executiveBoardCount,
            ScholarshipProviders = scholarshipProviderCount
        };
    }

    // ========================================
    // COMPARISON ACROSS TERMS
    // ========================================

    /// <summary>
    /// Gets a comparison of key metrics across all terms.
    /// Useful for trend analysis and historical reporting.
    /// </summary>
    public async Task<List<TermComparisonDto>> GetTermComparisonAsync()
    {
        var terms = await _context.Terms
            .OrderByDescending(t => t.Start)
            .ToListAsync();

        var comparisons = new List<TermComparisonDto>();

        foreach (var term in terms)
        {
            var activeStudents = await GetActiveScholarshipStudentCountAsync(term.Id);
            var graduatedStudents = await GetGraduatedStudentCountAsync(term.Id);
            var monthlyBudget = await GetMonthlyScholarshipBudgetAsync(term.Id);

            comparisons.Add(new TermComparisonDto
            {
                TermId = term.Id,
                TermName = term.DisplayName,
                TermStart = term.Start,
                TermEnd = term.End,
                IsActive = term.IsActive,
                ActiveStudents = activeStudents,
                GraduatedStudents = graduatedStudents,
                MonthlyBudget = monthlyBudget
            });
        }

        return comparisons;
    }
}

// ========================================
// DTOs (Data Transfer Objects)
// ========================================

public class StudentTermDetailDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public decimal MonthlyAmount { get; set; }
    public decimal TotalScholarshipReceived { get; set; }
    public string? University { get; set; }
    public string? Department { get; set; }
    public int? ClassLevel { get; set; }
    public string? DonorName { get; set; }
    public DateTime? ScholarshipStart { get; set; }
    public DateTime? ScholarshipEnd { get; set; }
    public double? Gpa { get; set; }
}

public class UniversityStatDto
{
    public string University { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public decimal TotalMonthlyAmount { get; set; }
    public decimal TotalScholarship { get; set; }
}

public class ClassLevelStatDto
{
    public int ClassLevel { get; set; }
    public int StudentCount { get; set; }
    public decimal TotalMonthlyAmount { get; set; }
    public decimal AverageMonthlyAmount { get; set; }
}

public class MemberRoleDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int TermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsProvidingScholarship { get; set; }
    public string? Status { get; set; }
    public DateTime? RoleStartDate { get; set; }
    public DateTime? RoleEndDate { get; set; }
}

public class MemberRoleCountDto
{
    public string Role { get; set; } = string.Empty;
    public int Count { get; set; }
    public int ProvidingScholarshipCount { get; set; }
}

public class DashboardSummaryDto
{
    public int TermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public DateTime TermStart { get; set; }
    public DateTime TermEnd { get; set; }
    public int ActiveScholarshipStudents { get; set; }
    public int GraduatedStudents { get; set; }
    public int TotalStudents { get; set; }
    public decimal TotalScholarshipDistributed { get; set; }
    public decimal MonthlyScholarshipBudget { get; set; }
    public int ExecutiveBoardMembers { get; set; }
    public int ScholarshipProviders { get; set; }
}

public class TermComparisonDto
{
    public int TermId { get; set; }
    public string TermName { get; set; } = string.Empty;
    public DateTime TermStart { get; set; }
    public DateTime TermEnd { get; set; }
    public bool IsActive { get; set; }
    public int ActiveStudents { get; set; }
    public int GraduatedStudents { get; set; }
    public decimal MonthlyBudget { get; set; }
}
