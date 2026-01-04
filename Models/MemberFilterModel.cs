namespace IzolluVakfi.Models;

public class MemberFilterModel
{
    public string? SearchText { get; set; }
    public MemberStatusFilter Status { get; set; } = MemberStatusFilter.All;
    public BursVeriyorFilter BursVeriyor { get; set; } = BursVeriyorFilter.All;
    public List<string> SelectedProfessions { get; set; } = new();
    public List<string> SelectedCities { get; set; } = new();
    public List<string> SelectedMembershipTypes { get; set; } = new();
    public RoleFilter Role { get; set; } = RoleFilter.All;
    public string? SelectedPeriod { get; set; }
    public DateTime? MembershipStartFrom { get; set; }
    public DateTime? MembershipStartTo { get; set; }
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }

    public bool HasAnyFilter()
    {
        return !string.IsNullOrWhiteSpace(SearchText)
               || Status != MemberStatusFilter.All
               || BursVeriyor != BursVeriyorFilter.All
               || SelectedProfessions.Any()
               || SelectedCities.Any()
               || SelectedMembershipTypes.Any()
               || Role != RoleFilter.All
               || !string.IsNullOrWhiteSpace(SelectedPeriod)
               || MembershipStartFrom.HasValue
               || MembershipStartTo.HasValue
               || AgeFrom.HasValue
               || AgeTo.HasValue;
    }

    public void Clear()
    {
        SearchText = null;
        Status = MemberStatusFilter.All;
        BursVeriyor = BursVeriyorFilter.All;
        SelectedProfessions.Clear();
        SelectedCities.Clear();
        SelectedMembershipTypes.Clear();
        Role = RoleFilter.All;
        SelectedPeriod = null;
        MembershipStartFrom = null;
        MembershipStartTo = null;
        AgeFrom = null;
        AgeTo = null;
    }
}

public enum MemberStatusFilter
{
    All,
    Active,
    Inactive
}

public enum BursVeriyorFilter
{
    All,
    Yes,
    No
}

public enum RoleFilter
{
    All,
    Mutevelli,
    YonetimKurulu
}
