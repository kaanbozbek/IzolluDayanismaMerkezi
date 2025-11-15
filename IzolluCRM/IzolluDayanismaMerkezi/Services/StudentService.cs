using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class StudentService
{
    private readonly ApplicationDbContext _context;
    private readonly ActivityLogService _logService;
    private readonly MeetingService _meetingService;
    private readonly TermService _termService;

    public StudentService(ApplicationDbContext context, ActivityLogService logService, MeetingService meetingService, TermService termService)
    {
        _context = context;
        _logService = logService;
        _meetingService = meetingService;
        _termService = termService;
    }

    // Legacy methods - kept for backward compatibility
    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.Transcripts)
            .OrderByDescending(s => s.OlusturmaTarihi)
            .ToListAsync();
    }

    public async Task<List<Student>> GetCurrentStudentsAsync()
    {
        return await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => !s.MezunMu)
            .OrderByDescending(s => s.OlusturmaTarihi)
            .ToListAsync();
    }

    public async Task<List<Student>> GetActiveScholarshipStudentsAsync()
    {
        return await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => s.AktifBursMu)
            .OrderBy(s => s.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Student>> GetGraduatedStudentsAsync()
    {
        return await _context.Students
            .Where(s => s.MezunMu)
            .OrderByDescending(s => s.MezuniyetTarihi)
            .ToListAsync();
    }

    // Term-based query methods
    public async Task<List<Student>> GetStudentsByTermAsync(int termId)
    {
        var studentIds = await _context.StudentTerms
            .Where(st => st.TermId == termId)
            .Select(st => st.StudentId)
            .ToListAsync();

        return await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => studentIds.Contains(s.Id))
            .OrderBy(s => s.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Student>> GetActiveStudentsByTermAsync(int termId)
    {
        var studentIds = await _context.StudentTerms
            .Where(st => st.TermId == termId && !st.IsGraduated)
            .Select(st => st.StudentId)
            .ToListAsync();

        return await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => studentIds.Contains(s.Id))
            .OrderBy(s => s.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Student>> GetScholarshipStudentsByTermAsync(int termId)
    {
        var studentIds = await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsActive && st.MonthlyAmount > 0)
            .Select(st => st.StudentId)
            .ToListAsync();

        return await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => studentIds.Contains(s.Id))
            .OrderBy(s => s.AdSoyad)
            .ToListAsync();
    }

    public async Task<List<Student>> GetGraduatedStudentsByTermAsync(int termId)
    {
        var studentIds = await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsGraduated)
            .Select(st => st.StudentId)
            .ToListAsync();

        return await _context.Students
            .Where(s => studentIds.Contains(s.Id))
            .OrderByDescending(s => s.MezuniyetTarihi)
            .ToListAsync();
    }

    public async Task<StudentTerm?> GetStudentTermAsync(int studentId, int termId)
    {
        return await _context.StudentTerms
            .Include(st => st.Student)
                .ThenInclude(s => s.Transcripts)
            .Include(st => st.Term)
            .FirstOrDefaultAsync(st => st.StudentId == studentId && st.TermId == termId);
    }

    public async Task<List<StudentTerm>> GetStudentTermsByTermAsync(int termId)
    {
        return await _context.StudentTerms
            .Include(st => st.Student)
                .ThenInclude(s => s.Transcripts)
            .Where(st => st.TermId == termId)
            .OrderBy(st => st.Student.AdSoyad)
            .ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Transcripts)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        student.SicilNumarasi = NormalizeSicil(student.SicilNumarasi);

        if (!string.IsNullOrEmpty(student.SicilNumarasi))
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.SicilNumarasi == student.SicilNumarasi);

            if (existingStudent != null)
            {
                throw new InvalidOperationException($"Bu sicil numarası ({student.SicilNumarasi}) zaten kayıtlı!");
            }
        }

        student.OlusturmaTarihi = DateTime.Now;
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Create StudentTerm record for active term
        var activeTerm = await _termService.GetActiveTermAsync();
        if (activeTerm != null)
        {
            var studentTerm = new StudentTerm
            {
                StudentId = student.Id,
                TermId = activeTerm.Id,
                IsActive = true,  // Always true for new students - represents presence in term
                IsGraduated = false,  // New students are not graduated
                MonthlyAmount = student.AylikTutar,
                ScholarshipStart = student.BursBaslangicTarihi,
                ScholarshipEnd = student.BursBitisTarihi,
                DonorName = student.BagisciAdi,
                University = student.Universite,
                Department = student.Bolum,
                ClassLevel = student.Sinif,
                TermNotes = student.Notlar,
                CreatedAt = DateTime.UtcNow
            };
            _context.StudentTerms.Add(studentTerm);
            await _context.SaveChangesAsync();
        }

        await _meetingService.SeedAttendanceForNewStudentAsync(student.Id);

        await _logService.LogAsync("OgrenciEkle", $"Yeni öğrenci eklendi: {student.AdSoyad}");

        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        student.SicilNumarasi = NormalizeSicil(student.SicilNumarasi);

        if (!string.IsNullOrEmpty(student.SicilNumarasi))
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.SicilNumarasi == student.SicilNumarasi && s.Id != student.Id);

            if (existingStudent != null)
            {
                throw new InvalidOperationException($"Bu sicil numarası ({student.SicilNumarasi}) zaten kayıtlı!");
            }
        }

        student.GuncellemeTarihi = DateTime.Now;
        _context.Students.Update(student);
        await _context.SaveChangesAsync();

        await _logService.LogAsync("OgrenciGuncelle", $"Öğrenci güncellendi: {student.AdSoyad}");

        return student;
    }

    public async Task DeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            await _logService.LogAsync("OgrenciSil", $"Öğrenci silindi: {student.AdSoyad}");
        }
    }

    public async Task GraduateStudentAsync(int id, DateTime graduationDate)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            student.MezunMu = true;
            student.MezuniyetTarihi = graduationDate;
            student.AktifBursMu = false;
            student.MezunOlduguDonem = student.Donem; // Aktif dönem mezuniyet dönemi olarak kaydedilir
            student.GuncellemeTarihi = DateTime.Now;

            // Update all StudentTerm records to mark as graduated
            var studentTerms = await _context.StudentTerms
                .Where(st => st.StudentId == id)
                .ToListAsync();
            
            foreach (var studentTerm in studentTerms)
            {
                studentTerm.IsGraduated = true;
            }

            await _context.SaveChangesAsync();

            await _logService.LogAsync("OgrenciMezun", $"Öğrenci mezun edildi: {student.AdSoyad}");
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Students.CountAsync();
    }

    public async Task<int> GetActiveScholarshipCountAsync()
    {
        return await _context.Students.CountAsync(s => s.AktifBursMu);
    }

    public async Task<int> GetGraduatedCountAsync()
    {
        return await _context.Students.CountAsync(s => s.MezunMu);
    }

    public async Task<decimal> GetTotalActiveScholarshipAmountAsync()
    {
        // SQLite doesn't support Sum on decimal, so we load into memory first
        var activeStudents = await _context.Students
            .Where(s => s.AktifBursMu)
            .Select(s => s.AylikTutar)
            .ToListAsync();
        
        return activeStudents.Sum();
    }

    // Term-based count methods
    public async Task<int> GetStudentCountByTermAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsActive)
            .CountAsync();
    }

    public async Task<int> GetScholarshipCountByTermAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsActive && st.MonthlyAmount > 0)
            .CountAsync();
    }

    public async Task<int> GetGraduatedCountByTermAsync(int termId)
    {
        return await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsGraduated)
            .CountAsync();
    }

    public async Task<decimal> GetTotalScholarshipAmountByTermAsync(int termId)
    {
        var amounts = await _context.StudentTerms
            .Where(st => st.TermId == termId && st.IsActive)
            .Select(st => st.MonthlyAmount)
            .ToListAsync();
        
        return amounts.Sum();
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