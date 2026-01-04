namespace IzolluVakfi.Data.Entities;

public class MemberMeetingAttendance
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int MeetingId { get; set; }
    public bool Katildi { get; set; } = true;
    public string? Mazeret { get; set; }
    public int Status { get; set; } = 0;
    public DateTime? GuncellemeTarihi { get; set; }

    public virtual Member Member { get; set; } = null!;
    public virtual Meeting Meeting { get; set; } = null!;
}
