using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

public class MeetingService
{
    private readonly ApplicationDbContext _context;
    private readonly ActivityLogService _logService;

    public MeetingService(ApplicationDbContext context, ActivityLogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public async Task<List<Meeting>> GetAllAsync()
    {
        return await _context.Meetings
            .Include(m => m.Attendances)
                .ThenInclude(a => a.Student)
            .OrderByDescending(m => m.Tarih)
            .ToListAsync();
    }

    public async Task<Meeting?> GetByIdAsync(int id)
    {
        return await _context.Meetings
            .Include(m => m.Attendances)
                .ThenInclude(a => a.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Meeting> CreateAsync(Meeting meeting)
    {
        meeting.OlusturmaTarihi = DateTime.Now;
        if (meeting.BitisTarihi <= meeting.Tarih)
        {
            meeting.BitisTarihi = meeting.Tarih.AddHours(1);
        }
        _context.Meetings.Add(meeting);
        await _context.SaveChangesAsync();

        await EnsureAttendanceRecordsAsync(meeting.Id);
        await _logService.LogAsync("ToplantiEkle", $"Yeni toplantı oluşturuldu: {meeting.Baslik}");
        return meeting;
    }

    public async Task DeleteAsync(int meetingId)
    {
        var meeting = await _context.Meetings.FindAsync(meetingId);
        if (meeting == null)
            return;

        _context.Meetings.Remove(meeting);
        await _context.SaveChangesAsync();

        await _logService.LogAsync("ToplantiSil", $"Toplantı silindi: {meeting.Baslik}");
    }

    public async Task<List<StudentMeetingAttendance>> GetAttendancesForStudentAsync(int studentId)
    {
        return await _context.StudentMeetingAttendances
            .Include(a => a.Meeting)
            .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Meeting.Tarih)
            .ToListAsync();
    }

    public async Task UpdateAttendanceAsync(int attendanceId, bool katildi)
    {
        var attendance = await _context.StudentMeetingAttendances.FindAsync(attendanceId);
        if (attendance == null)
            return;

        attendance.Katildi = katildi;
        attendance.GuncellemeTarihi = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task SeedAttendanceForNewStudentAsync(int studentId)
    {
        var meetingIds = await _context.Meetings.Select(m => m.Id).ToListAsync();
        if (!meetingIds.Any())
            return;

        var newRecords = meetingIds.Select(id => new StudentMeetingAttendance
        {
            StudentId = studentId,
            MeetingId = id,
            Katildi = true
        });

        _context.StudentMeetingAttendances.AddRange(newRecords);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureAttendanceRecordsAsync(int meetingId)
    {
        var existingStudentIds = await _context.StudentMeetingAttendances
            .Where(a => a.MeetingId == meetingId)
            .Select(a => a.StudentId)
            .ToListAsync();

        var students = await _context.Students
            .Where(s => !existingStudentIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        if (!students.Any())
            return;

        var attendances = students.Select(studentId => new StudentMeetingAttendance
        {
            MeetingId = meetingId,
            StudentId = studentId,
            Katildi = true
        });

        _context.StudentMeetingAttendances.AddRange(attendances);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ApplyLatestMeetingAbsencePenaltyAsync()
    {
        var result = await ApplyLatestMeetingAbsencePenaltyWithDetailsAsync();
        return result.Count;
    }

    public async Task<List<Student>> ApplyLatestMeetingAbsencePenaltyWithDetailsAsync()
    {
        var latestMeeting = await _context.Meetings
            .OrderByDescending(m => m.Tarih)
            .FirstOrDefaultAsync();

        if (latestMeeting == null)
            return new List<Student>();

        var absentStudentIds = await _context.StudentMeetingAttendances
            .Where(a => a.MeetingId == latestMeeting.Id && !a.Katildi)
            .Select(a => a.StudentId)
            .ToListAsync();

        if (!absentStudentIds.Any())
            return new List<Student>();

        var affectedStudents = await _context.Students
            .Where(s => absentStudentIds.Contains(s.Id) && s.AktifBursMu)
            .ToListAsync();

        foreach (var student in affectedStudents)
        {
            student.AktifBursMu = false;
            student.ScholarshipCutReason = $"Toplantı kontrolü ({latestMeeting.Baslik} - {latestMeeting.Tarih:dd.MM.yyyy})";
            student.ScholarshipCutDate = DateTime.Now;

            var reason = "Toplantıya katılmama";
            if (string.IsNullOrWhiteSpace(student.Notlar))
            {
                student.Notlar = reason;
            }
            else if (!student.Notlar.Contains(reason))
            {
                student.Notlar = $"{student.Notlar}; {reason}";
            }

            await _logService.LogAsync(
                "BursKes",
                $"{student.AdSoyad} - Bursu kesildi: {reason}"
            );
        }

        await _context.SaveChangesAsync();
        return affectedStudents;
    }
}
