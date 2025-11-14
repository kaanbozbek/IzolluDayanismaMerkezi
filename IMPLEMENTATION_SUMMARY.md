# Term-Based Snapshot Model Implementation

## 📋 Quick Start

This implementation provides a complete **term-based snapshot system** for managing students, members, and scholarships across different academic periods.

### What's Been Implemented

✅ **Entity Model**
- `Term` - Academic/financial period
- `StudentTerm` - Per-term student snapshot
- `MemberTermRole` - Per-term member role snapshot

✅ **Database Configuration**
- Entity Framework Core configuration
- Performance indexes for SQLite
- Foreign key relationships

✅ **Services**
- `TermService` - Term management and snapshot creation
- `TermReportService` - Term-aware reporting with example queries
- `TermChangeNotifier` - Cross-component event notifications

✅ **Migration**
- SQL migration to reset and initialize clean 2025-2026 term
- PowerShell script for easy execution

✅ **Documentation**
- Complete guide (`TERM_SNAPSHOT_MODEL_GUIDE.md`)
- Example Blazor component (`TermReportsExample.razor`)
- This README

---

## 🚀 Getting Started

### Step 1: Run the Migration

**Option A: Using PowerShell Script (Recommended)**

```powershell
.\Initialize-TermSnapshot.ps1
```

This will:
- Backup your database
- Delete all existing term data
- Create clean 2025-2026 term
- Populate StudentTerm and MemberTermRole from existing data

**Option B: Manual Migration**

```bash
cd IzolluCRM/IzolluDayanismaMerkezi
dotnet ef migrations add ResetAndInitializeTerms
dotnet ef database update
```

### Step 2: Verify the Setup

Run the application:

```bash
dotnet run --project IzolluCRM/IzolluDayanismaMerkezi/IzolluVakfi.csproj
```

Navigate to: **Settings → Dönemler**

You should see:
- ✅ One term: "2025-2026" (Active)
- ✅ All existing students in StudentTerms table
- ✅ All existing members in MemberTermRoles table

### Step 3: Update Your Pages

All pages that display student or member data should:

1. **Add term selector**
2. **Filter queries by TermId**
3. **Subscribe to TermChangeNotifier**
4. **Implement IDisposable for cleanup**

See `TermReportsExample.razor` for a complete example.

---

## 🎯 Key Concepts

### Snapshot Model

When you open a new term (e.g., 2026-2027):
- ✅ Active students are cloned
- ❌ Graduated students are NOT cloned
- ✅ Active member roles are cloned
- ✅ Each term has independent data

### Critical Behavior

**Graduated students don't advance to new terms:**

```
2025-2026:
  - Ali (Active) → Copied to 2026-2027
  - Ayşe (Active) → Copied to 2026-2027
  - Mehmet (Graduated) → NOT copied to 2026-2027

When viewing 2025-2026: You see all three students
When viewing 2026-2027: You see only Ali and Ayşe
```

### Query Pattern

❌ **NEVER do this:**
```csharp
var students = await _context.Students
    .Where(s => s.AktifBursMu)
    .ToListAsync();
```

✅ **ALWAYS do this:**
```csharp
var students = await _context.StudentTerms
    .Where(st => st.TermId == selectedTermId 
              && st.IsActive 
              && !st.IsGraduated)
    .Include(st => st.Student)
    .ToListAsync();
```

---

## 📁 Files Created/Modified

### New Files

1. **`Services/TermService.cs`**
   - Updated with graduated student filtering
   - Clone logic for new terms

2. **`Services/TermReportService.cs`** ⭐ NEW
   - Complete set of term-aware query examples
   - Dashboard summary methods
   - Student/member statistics by term

3. **`Migrations/ResetAndInitializeTerms_Manual.cs`** ⭐ NEW
   - Manual migration for term initialization
   - SQL to create 2025-2026 term
   - Population of StudentTerm and MemberTermRole

4. **`Pages/TermReportsExample.razor`** ⭐ NEW
   - Complete example of term-aware Blazor page
   - Shows dashboard, tables, statistics
   - Demonstrates proper query patterns

5. **`TERM_SNAPSHOT_MODEL_GUIDE.md`** ⭐ NEW
   - Comprehensive documentation
   - Data model explanation
   - Query examples
   - Best practices

6. **`Initialize-TermSnapshot.ps1`** ⭐ NEW
   - PowerShell script to run migration
   - Includes backup functionality
   - Verification steps

7. **`IMPLEMENTATION_SUMMARY.md`** (this file)

### Modified Files

1. **`Program.cs`**
   - Added `TermReportService` registration

2. **`Data/ApplicationDbContext.cs`**
   - Already had proper configuration (no changes needed)

3. **`Data/Entities/Term.cs`**
   - Already existed (no changes needed)

4. **`Data/Entities/StudentTerm.cs`**
   - Already existed (no changes needed)

5. **`Data/Entities/MemberTermRole.cs`**
   - Already existed (no changes needed)

---

## 🔄 Workflow: Opening a New Term

### Via UI (Settings Page)

1. Go to **Settings → Dönemler**
2. Click **"Yeni Dönem Aç"**
3. Enter start/end dates
4. System automatically:
   - Deactivates current term
   - Creates new term
   - Clones active students (excludes graduated)
   - Clones active member roles
   - Notifies all pages to refresh

### Via Code

```csharp
var newTerm = await termService.OpenNewTermAsync(
    start: new DateTime(2026, 9, 1),
    end: new DateTime(2027, 8, 31),
    description: "2026-2027 akademik yılı"
);
```

---

## 📊 Example Queries

All provided in `TermReportService.cs`:

### Student Queries
- `GetActiveScholarshipStudentCountAsync(termId)`
- `GetGraduatedStudentCountAsync(termId)`
- `GetTotalScholarshipAmountAsync(termId)`
- `GetMonthlyScholarshipBudgetAsync(termId)`
- `GetActiveScholarshipStudentsAsync(termId)`
- `GetGraduatedStudentsAsync(termId)`
- `GetScholarshipByUniversityAsync(termId)`
- `GetScholarshipByClassLevelAsync(termId)`

### Member Queries
- `GetExecutiveBoardMembersAsync(termId)`
- `GetBoardOfTrusteesAsync(termId)`
- `GetMemberCountByRoleAsync(termId)`
- `GetScholarshipProviderCountAsync(termId)`

### Dashboard
- `GetDashboardSummaryAsync(termId)` - All metrics in one call
- `GetTermComparisonAsync()` - Compare all terms

---

## 🎨 UI Updates Needed

For each page that shows students or members:

### 1. Add Injections

```csharp
@inject TermService TermService
@inject TermReportService TermReportService  // Optional
@inject TermChangeNotifier TermChangeNotifier
@implements IDisposable
```

### 2. Add Term Selector

```razor
<MudSelect T="int" 
           Label="Dönem Seçin" 
           Value="_selectedTermId"
           ValueChanged="OnTermSelectedAsync">
    @foreach (var term in _terms)
    {
        <MudSelectItem Value="@term.Id">
            @term.DisplayName @(term.IsActive ? " ★" : "")
        </MudSelectItem>
    }
</MudSelect>
```

### 3. Subscribe to Events

```csharp
protected override async Task OnInitializedAsync()
{
    TermChangeNotifier.OnTermChanged += OnTermChangedHandler;
    
    var activeTerm = await TermService.GetActiveTermAsync();
    _selectedTermId = activeTerm?.Id;
    await LoadData();
}

private async void OnTermChangedHandler()
{
    var activeTerm = await TermService.GetActiveTermAsync();
    _selectedTermId = activeTerm?.Id;
    await LoadData();
    await InvokeAsync(StateHasChanged);
}

public void Dispose()
{
    TermChangeNotifier.OnTermChanged -= OnTermChangedHandler;
}
```

### 4. Update Queries

Replace:
```csharp
var students = await _context.Students.ToListAsync();
```

With:
```csharp
var students = await _context.StudentTerms
    .Where(st => st.TermId == _selectedTermId)
    .Include(st => st.Student)
    .ToListAsync();
```

---

## ✅ Verification Checklist

After implementation:

### Database
- [ ] Terms table has 2025-2026 entry with IsActive = true
- [ ] StudentTerms table populated (count should equal Students count)
- [ ] MemberTermRoles table populated (count should equal Members count)
- [ ] All foreign keys are valid

### Application
- [ ] Application runs without errors
- [ ] Settings page shows term list
- [ ] Can create new term via "Yeni Dönem Aç"
- [ ] Graduated students excluded when opening new term

### Pages
- [ ] All pages have term selector
- [ ] Changing term reloads data
- [ ] Reports show term-specific data
- [ ] Dashboard shows correct counts per term

### Behavior
- [ ] Graduated students don't appear in new terms
- [ ] Can switch back to old terms and see historical data
- [ ] Member roles are term-specific
- [ ] Reports are accurate per term

---

## 🆘 Troubleshooting

### Migration Failed

1. Check database file exists
2. Verify no active connections
3. Restore from backup if needed
4. Run SQL manually using SQLite browser

### Reports Show Wrong Data

- Ensure all queries filter by `TermId`
- Check `_selectedTermId` is set correctly
- Verify term selector is working

### Graduated Students Still Appear

- Check `OpenNewTermAsync` filters out `IsGraduated`
- Verify `IsGraduated` field is set correctly in source data
- Check logs for cloning operation

### Performance Issues

- Verify indexes are created (check with SQLite browser)
- Enable WAL mode (done in Program.cs)
- Check query execution plans

---

## 📚 Documentation

- **Complete Guide**: `TERM_SNAPSHOT_MODEL_GUIDE.md`
- **Service Code**: `Services/TermService.cs`, `Services/TermReportService.cs`
- **Example Page**: `Pages/TermReportsExample.razor`
- **Migration**: `Migrations/ResetAndInitializeTerms_Manual.cs`

---

## 🎓 Key Takeaways

1. **Each term is a snapshot in time**
2. **NEVER query Student/Member directly for reports**
3. **ALWAYS filter by TermId**
4. **Graduated students don't advance**
5. **Historical data is preserved**

---

## 📞 Support

If you encounter issues:

1. Check the detailed guide: `TERM_SNAPSHOT_MODEL_GUIDE.md`
2. Review example page: `TermReportsExample.razor`
3. Check logs in console output
4. Verify database schema with SQLite browser

---

**Implementation completed successfully! 🎉**

All core components are in place. You now need to:
1. Run the migration
2. Update existing pages to use term-aware queries
3. Test the behavior thoroughly
