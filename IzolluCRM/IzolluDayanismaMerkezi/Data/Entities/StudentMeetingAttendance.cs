namespace IzolluVakfi.Data.Entities;

public class StudentMeetingAttendance
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int MeetingId { get; set; }
    public bool Katildi { get; set; } = true;
    public DateTime? GuncellemeTarihi { get; set; }

    public virtual Student Student { get; set; } = null!;
    public virtual Meeting Meeting { get; set; } = null!;
}
