using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class MemberService
{
    private readonly ApplicationDbContext _context;
    private readonly ActivityLogService _logService;
    private readonly TermService _termService;

    public MemberService(ApplicationDbContext context, ActivityLogService logService, TermService termService)
    {
        _context = context;
        _logService = logService;
        _termService = termService;
    }

    // Legacy methods - kept for backward compatibility
    public async Task<List<Member>> GetAllAsync()
    {
        return await _context.Members
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetMutevelliAsync()
    {
        return await _context.Members
            .Where(m => m.IsMutevelli)
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetYonetimKuruluAsync()
    {
        return await _context.Members
            .Where(m => m.IsYonetimKurulu)
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    // Term-based query methods
    public async Task<List<Member>> GetMembersByTermAsync(int termId)
    {
        var memberIds = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId)
            .Select(mtr => mtr.MemberId)
            .Distinct()
            .ToListAsync();

        return await _context.Members
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetActiveMembersByTermAsync(int termId)
    {
        var memberIds = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsActive)
            .Select(mtr => mtr.MemberId)
            .Distinct()
            .ToListAsync();

        return await _context.Members
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetBoardOfTrusteesByTermAsync(int termId)
    {
        var memberIds = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsBoardOfTrustees && mtr.IsActive)
            .Select(mtr => mtr.MemberId)
            .ToListAsync();

        return await _context.Members
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetExecutiveBoardByTermAsync(int termId)
    {
        var memberIds = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsExecutiveBoard && mtr.IsActive)
            .Select(mtr => mtr.MemberId)
            .ToListAsync();

        return await _context.Members
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Member>> GetScholarshipProvidersByTermAsync(int termId)
    {
        var memberIds = await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsProvidingScholarship && mtr.IsActive)
            .Select(mtr => mtr.MemberId)
            .ToListAsync();

        return await _context.Members
            .Where(m => memberIds.Contains(m.Id))
            .OrderBy(m => m.AdSoyad)
            .ToListAsync();
    }

    public async Task<MemberTermRole?> GetMemberTermRoleAsync(int memberId, int termId)
    {
        return await _context.MemberTermRoles
            .Include(mtr => mtr.Member)
            .Include(mtr => mtr.Term)
            .FirstOrDefaultAsync(mtr => mtr.MemberId == memberId && mtr.TermId == termId);
    }

    public async Task<List<MemberTermRole>> GetMemberTermRolesByTermAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Include(mtr => mtr.Member)
            .Where(mtr => mtr.TermId == termId)
            .OrderBy(mtr => mtr.Member.AdSoyad)
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _context.Members.FindAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        member.SicilNumarasi = NormalizeSicil(member.SicilNumarasi);

        if (!string.IsNullOrEmpty(member.SicilNumarasi))
        {
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.SicilNumarasi == member.SicilNumarasi);

            if (existingMember != null)
            {
                throw new InvalidOperationException($"Bu sicil numarası ({member.SicilNumarasi}) zaten kayıtlı!");
            }
        }

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        // Create MemberTermRole record for active term
        var activeTerm = await _termService.GetActiveTermAsync();
        if (activeTerm != null)
        {
            var memberRole = new MemberTermRole
            {
                MemberId = member.Id,
                TermId = activeTerm.Id,
                Role = member.Meslek ?? "Üye",
                IsActive = true,
                IsBoardOfTrustees = member.IsMutevelli,
                IsExecutiveBoard = member.IsYonetimKurulu,
                IsAuditCommittee = member.IsDenetimKurulu,
                IsProvidingScholarship = member.BursVeriyor,
                CreatedAt = DateTime.UtcNow
            };
            _context.MemberTermRoles.Add(memberRole);
            await _context.SaveChangesAsync();
        }

        await _logService.LogAsync("UyeEkle", $"Yeni üye eklendi: {member.AdSoyad}");

        return member;
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        member.SicilNumarasi = NormalizeSicil(member.SicilNumarasi);

        if (!string.IsNullOrEmpty(member.SicilNumarasi))
        {
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.SicilNumarasi == member.SicilNumarasi && m.Id != member.Id);

            if (existingMember != null)
            {
                throw new InvalidOperationException($"Bu sicil numarası ({member.SicilNumarasi}) zaten kayıtlı!");
            }
        }

        _context.Members.Update(member);
        await _context.SaveChangesAsync();

        await _logService.LogAsync("UyeGuncelle", $"Üye güncellendi: {member.AdSoyad}");

        return member;
    }

    public async Task DeleteAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            await _logService.LogAsync("UyeSil", $"Üye silindi: {member.AdSoyad}");
        }
    }

    public async Task ToggleMutevelliAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            member.IsMutevelli = !member.IsMutevelli;
            await _context.SaveChangesAsync();

            await _logService.LogAsync("MutevelliDegisiklik", 
                $"{member.AdSoyad} - Mütevelli: {member.IsMutevelli}");
        }
    }

    public async Task ToggleYonetimKuruluAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            member.IsYonetimKurulu = !member.IsYonetimKurulu;
            await _context.SaveChangesAsync();

            await _logService.LogAsync("YonetimKuruluDegisiklik", 
                $"{member.AdSoyad} - Yönetim Kurulu: {member.IsYonetimKurulu}");
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Members.CountAsync(m => m.AktifMi);
    }

    public async Task<int> GetBursVerenCountAsync()
    {
        return await _context.Members.CountAsync(m => m.BursVeriyor && m.AktifMi);
    }

    /// <summary>
    /// Alias for GetBursVerenCountAsync - counts active members providing scholarships
    /// This matches the "Bağışçılar" tab data in Members page
    /// </summary>
    public async Task<int> GetDonorCountAsync()
    {
        return await GetBursVerenCountAsync();
    }

    // Term-based count methods
    public async Task<int> GetMemberCountByTermAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsActive)
            .Select(mtr => mtr.MemberId)
            .Distinct()
            .CountAsync();
    }

    public async Task<int> GetBoardOfTrusteesCountByTermAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsBoardOfTrustees && mtr.IsActive)
            .CountAsync();
    }

    public async Task<int> GetExecutiveBoardCountByTermAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsExecutiveBoard && mtr.IsActive)
            .CountAsync();
    }

    public async Task<int> GetScholarshipProvidersCountByTermAsync(int termId)
    {
        return await _context.MemberTermRoles
            .Where(mtr => mtr.TermId == termId && mtr.IsProvidingScholarship && mtr.IsActive)
            .CountAsync();
    }

    public async Task<Dictionary<string, decimal>> GetTotalScholarshipByMemberAsync()
    {
        var members = await _context.Members
            .Where(m => m.BursVeriyor && m.AktifMi)
            .ToListAsync();

        var students = await _context.Students
            .Where(s => !string.IsNullOrEmpty(s.BagisciAdi))
            .ToListAsync();

        var memberScholarships = new Dictionary<string, decimal>();

        foreach (var member in members)
        {
            var totalAmount = students
                .Where(s => s.BagisciAdi != null && s.BagisciAdi.Equals(member.AdSoyad, StringComparison.OrdinalIgnoreCase))
                .Sum(s => s.ToplamAlinanBurs);

            if (totalAmount > 0)
            {
                memberScholarships[member.AdSoyad] = totalAmount;
            }
        }

        return memberScholarships
            .OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static string? NormalizeSicil(string? value)
    {
        var trimmed = value?.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return null;
        }

        return trimmed.ToUpperInvariant();
    }
}