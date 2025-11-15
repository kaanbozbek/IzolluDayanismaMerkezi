namespace IzolluVakfi.Models;

public class StudentFilterModel
{
    public string? SearchText { get; set; }

    public ScholarshipStatusFilter ScholarshipStatus { get; set; } = ScholarshipStatusFilter.All;
    public GenderFilter Gender { get; set; } = GenderFilter.All;

    public List<string> SelectedUniversities { get; set; } = new();
    public List<string> SelectedDepartments { get; set; } = new();
    public List<int> SelectedClassLevels { get; set; } = new();
    public List<string> SelectedProfessions { get; set; } = new();
    public List<string> SelectedCities { get; set; } = new();

    public DateTime? ScholarshipStartFrom { get; set; }
    public DateTime? ScholarshipStartTo { get; set; }

    public bool HasAnyFilter()
    {
        return !string.IsNullOrWhiteSpace(SearchText) ||
               ScholarshipStatus != ScholarshipStatusFilter.All ||
               Gender != GenderFilter.All ||
               SelectedUniversities.Any() ||
               SelectedDepartments.Any() ||
               SelectedClassLevels.Any() ||
               SelectedProfessions.Any() ||
               SelectedCities.Any() ||
               ScholarshipStartFrom.HasValue ||
               ScholarshipStartTo.HasValue;
    }

    public void Clear()
    {
        SearchText = null;
        ScholarshipStatus = ScholarshipStatusFilter.All;
        Gender = GenderFilter.All;
        SelectedUniversities.Clear();
        SelectedDepartments.Clear();
        SelectedClassLevels.Clear();
        SelectedProfessions.Clear();
        SelectedCities.Clear();
        ScholarshipStartFrom = null;
        ScholarshipStartTo = null;
    }
}

public enum ScholarshipStatusFilter
{
    All,
    HasScholarship,
    NoScholarship,
    ScholarshipCut
}

public enum GenderFilter
{
    All,
    Male,
    Female
}
