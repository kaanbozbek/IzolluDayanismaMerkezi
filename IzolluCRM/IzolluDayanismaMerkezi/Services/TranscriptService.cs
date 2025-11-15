using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

public class TranscriptService
{
    private readonly ApplicationDbContext _context;
    private readonly ActivityLogService _logService;

    public TranscriptService(ApplicationDbContext context, ActivityLogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public async Task<List<TranscriptRecord>> GetByStudentIdAsync(int studentId)
    {
        return await _context.TranscriptRecords
            .Where(t => t.StudentId == studentId)
            .OrderByDescending(t => t.GirisTarihi)
            .ToListAsync();
    }

    public async Task<TranscriptRecord> CreateAsync(TranscriptRecord transcript)
    {
        _context.TranscriptRecords.Add(transcript);
        await _context.SaveChangesAsync();

        // Update student's TranskriptNotu with the latest transcript GNO
        var student = await _context.Students.FindAsync(transcript.StudentId);
        if (student != null)
        {
            student.TranskriptNotu = transcript.GNO.ToString("0.00");
            student.GuncellemeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        await _logService.LogAsync("TranskriptEkle", 
            $"Transkript eklendi - Öğrenci ID: {transcript.StudentId}, GNO: {transcript.GNO}");

        return transcript;
    }

    public async Task DeleteAsync(int id)
    {
        var transcript = await _context.TranscriptRecords.FindAsync(id);
        if (transcript != null)
        {
            var studentId = transcript.StudentId;
            
            // PDF dosyasını sil
            if (!string.IsNullOrEmpty(transcript.PdfDosyaYolu))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", transcript.PdfDosyaYolu.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            _context.TranscriptRecords.Remove(transcript);
            await _context.SaveChangesAsync();

            // Update student's TranskriptNotu with the new latest transcript GNO
            var latestTranscript = await _context.TranscriptRecords
                .Where(t => t.StudentId == studentId)
                .OrderByDescending(t => t.GirisTarihi)
                .FirstOrDefaultAsync();
            
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                student.TranskriptNotu = latestTranscript != null ? latestTranscript.GNO.ToString("0.00") : string.Empty;
                student.GuncellemeTarihi = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            await _logService.LogAsync("TranskriptSil", 
                $"Transkript silindi - ID: {id}, Öğrenci ID: {studentId}");
        }
    }

    public async Task<decimal?> GetLatestGNOAsync(int studentId)
    {
        var latest = await _context.TranscriptRecords
            .Where(t => t.StudentId == studentId)
            .OrderByDescending(t => t.GirisTarihi)
            .FirstOrDefaultAsync();

        return latest?.GNO;
    }

    public async Task<int> CheckAndUpdateScholarshipsAsync()
    {
        var result = await CheckAndUpdateScholarshipsWithDetailsAsync();
        return result.Count;
    }

    public async Task<List<Student>> CheckAndUpdateScholarshipsWithDetailsAsync()
    {
        var activeStudents = await _context.Students
            .Include(s => s.Transcripts)
            .Where(s => s.AktifBursMu)
            .ToListAsync();

        var affectedStudents = new List<Student>();

        foreach (var student in activeStudents)
        {
            var latestTranscript = student.Transcripts
                .OrderByDescending(t => t.GirisTarihi)
                .FirstOrDefault();

            if (latestTranscript != null && latestTranscript.GNO < 2.0m)
            {
                student.AktifBursMu = false;
                student.ScholarshipCutDate = DateTime.Now;
                student.ScholarshipCutReason = "Transkript Kontrolü";
                student.TranskriptNotu = latestTranscript.GNO.ToString("0.00");
                student.GuncellemeTarihi = DateTime.Now;

                affectedStudents.Add(student);

                await _logService.LogAsync("BursKesildi", 
                    $"{student.AdSoyad} - Transkript Kontrolü - GNO: {latestTranscript.GNO}");
            }
        }

        if (affectedStudents.Count > 0)
        {
            await _context.SaveChangesAsync();
        }

        await _logService.LogAsync("TranskriptKontrol", 
            $"Transkript kontrolü tamamlandı. {affectedStudents.Count} öğrencinin bursu kesildi.");

        return affectedStudents;
    }
}