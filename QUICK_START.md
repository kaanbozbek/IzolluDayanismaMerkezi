# 🚀 Quick Start: Term-Based Snapshot Model

## ✅ Implementation Complete!

All components have been created and the project builds successfully with **0 errors**.

---

## 📦 What's Been Delivered

### 1. Core Services
- ✅ **TermService.cs** (Updated)
  - `OpenNewTermAsync()` - Excludes graduated students ✨
  - `InitializeFirstTermAsync()` - One-time migration helper
  - `SetActiveTermAsync()` - Change active term
  - `GetActiveTermAsync()` / `GetAllTermsAsync()` - Query terms

- ✅ **TermReportService.cs** (NEW) 
  - Complete set of term-aware queries
  - Dashboard summary methods
  - Student/member statistics by term
  - University/class level breakdowns
  - Term comparison reports

### 2. Database
- ✅ **Entities** (Already existed, properly configured)
  - `Term` - Academic period
  - `StudentTerm` - Per-term student snapshot
  - `MemberTermRole` - Per-term member role

- ✅ **DbContext Configuration**
  - All indexes defined and optimized for SQLite
  - Foreign key relationships
  - WAL mode enabled for performance

- ✅ **Migration Script**
  - `ResetAndInitializeTerms_Manual.cs`
  - SQL migration to reset and initialize 2025-2026

### 3. Documentation
- ✅ **TERM_SNAPSHOT_MODEL_GUIDE.md** - Complete reference (18+ pages)
- ✅ **IMPLEMENTATION_SUMMARY.md** - Implementation overview
- ✅ **QUICK_START.md** (this file)

### 4. Tools
- ✅ **Initialize-TermSnapshot.ps1** - PowerShell migration script
- ✅ **TermReportsExample.razor** - Complete Blazor page example

### 5. Program.cs
- ✅ `TermReportService` registered in DI container
- ✅ `TermChangeNotifier` already registered (singleton)

---

## 🎬 Next Steps

### Step 1: Run the Migration

**Choose one method:**

#### Method A: PowerShell Script (Recommended)

```powershell
.\Initialize-TermSnapshot.ps1
```

This will:
- ✅ Backup your database automatically
- ✅ Delete old term data (not Students/Members)
- ✅ Create 2025-2026 term
- ✅ Populate StudentTerm and MemberTermRole from existing data

#### Method B: Manual SQL Execution

1. Open SQLite browser or tool
2. Run SQL from `Migrations/ResetAndInitializeTerms_Manual.cs`
3. Verify counts match expected

---

### Step 2: Verify Setup

Run the application:

```bash
dotnet run --project IzolluCRM/IzolluDayanismaMerkezi/IzolluVakfi.csproj
```

Navigate to: http://localhost:5000

**Check Settings → Dönemler:**
- Should see "2025-2026" term marked as active (★)

**Check database:**
```sql
SELECT COUNT(*) FROM Terms;           -- Should be 1
SELECT COUNT(*) FROM StudentTerms;    -- Should equal student count
SELECT COUNT(*) FROM MemberTermRoles; -- Should equal member count
```

---

### Step 3: Test New Term Creation

1. Go to **Settings → Dönemler**
2. Click **"Yeni Dönem Aç"**
3. Enter dates:
   - Start: 2026-01-01
   - End: 2027-01-01
4. Submit

**Expected behavior:**
- ✅ 2025-2026 becomes inactive
- ✅ 2026-2027 becomes active
- ✅ Active students are cloned
- ❌ Graduated students are NOT cloned
- ✅ Active member roles are cloned

---

### Step 4: Test Term Switching

1. Go to any page with term selector (e.g., Öğrenciler)
2. Switch between 2025-2026 and 2026-2027
3. **Expected:**
   - Data updates automatically
   - 2025-2026 shows historical data (includes graduates)
   - 2026-2027 shows only non-graduated students

---

### Step 5: Update Existing Pages

For each page that displays students or members, you need to:

#### A. Add Injections

```csharp
@inject TermService TermService
@inject TermReportService TermReportService  // Optional
@inject TermChangeNotifier TermChangeNotifier
@implements IDisposable
```

#### B. Add Term Selector

Use the existing `TermSelector` component or create a MudSelect:

```razor
<TermSelector SelectedTermId="_selectedTermId" 
              SelectedTermIdChanged="OnTermChanged" />
```

#### C. Update Queries

Replace:
```csharp
var students = await _context.Students
    .Where(s => s.AktifBursMu)
    .ToListAsync();
```

With:
```csharp
var students = await _context.StudentTerms
    .Where(st => st.TermId == _selectedTermId 
              && st.IsActive 
              && !st.IsGraduated)
    .Include(st => st.Student)
    .ToListAsync();
```

#### D. Subscribe to Events

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

---

## 📊 Example: Using TermReportService

### In Your Blazor Page

```csharp
@inject TermReportService ReportService

@code {
    private int? _selectedTermId;
    private DashboardSummaryDto? _summary;
    private List<StudentTermDetailDto> _activeStudents = new();
    
    protected override async Task OnInitializedAsync()
    {
        var activeTerm = await TermService.GetActiveTermAsync();
        _selectedTermId = activeTerm?.Id;
        await LoadReports();
    }
    
    private async Task LoadReports()
    {
        if (!_selectedTermId.HasValue) return;
        
        // Get dashboard summary
        _summary = await ReportService.GetDashboardSummaryAsync(_selectedTermId.Value);
        
        // Get active students
        _activeStudents = await ReportService.GetActiveScholarshipStudentsAsync(_selectedTermId.Value);
        
        // Get statistics
        var universityStats = await ReportService.GetScholarshipByUniversityAsync(_selectedTermId.Value);
        var executiveBoard = await ReportService.GetExecutiveBoardMembersAsync(_selectedTermId.Value);
        
        StateHasChanged();
    }
}
```

---

## 🎯 Key Rules to Remember

### ✅ DO:
- **ALWAYS filter by TermId** in queries
- Use `StudentTerm` and `MemberTermRole` for reports
- Subscribe to `TermChangeNotifier.OnTermChanged`
- Implement `IDisposable` to unsubscribe events
- Use `InvokeAsync(StateHasChanged)` in event handlers

### ❌ DON'T:
- Query `Students` or `Members` directly for reports
- Forget to filter by `TermId`
- Mix term logic with global student/member data
- Modify closed term data (only edit active term)

---

## 🐛 Troubleshooting

### Migration Fails
```bash
# Check database is not locked
# Close all connections to database
# Run migration again
.\Initialize-TermSnapshot.ps1
```

### Reports Show Wrong Data
```csharp
// Verify query includes TermId filter
.Where(st => st.TermId == selectedTermId)

// Check _selectedTermId is set
if (!_selectedTermId.HasValue) return;
```

### Graduated Students Appear in New Term
```csharp
// Check TermService.OpenNewTermAsync excludes graduates
.Where(st => !st.IsGraduated)  // ← This line must exist
```

### Page Doesn't Refresh on Term Change
```csharp
// 1. Subscribe to event
TermChangeNotifier.OnTermChanged += OnTermChangedHandler;

// 2. Use InvokeAsync
await InvokeAsync(StateHasChanged);

// 3. Unsubscribe on dispose
public void Dispose() {
    TermChangeNotifier.OnTermChanged -= OnTermChangedHandler;
}
```

---

## 📚 Documentation Reference

- **Complete Guide**: `TERM_SNAPSHOT_MODEL_GUIDE.md` (18 pages)
- **Implementation Summary**: `IMPLEMENTATION_SUMMARY.md`
- **Example Page**: `Pages/TermReportsExample.razor`
- **Service Code**: `Services/TermService.cs`, `Services/TermReportService.cs`
- **Migration**: `Migrations/ResetAndInitializeTerms_Manual.cs`

---

## ✅ Checklist

Before going live:

- [ ] Run migration script
- [ ] Verify 2025-2026 term exists and is active
- [ ] Verify StudentTerms populated
- [ ] Verify MemberTermRoles populated
- [ ] Test creating new term (2026-2027)
- [ ] Verify graduated students excluded
- [ ] Update all pages to use term-aware queries
- [ ] Add term selector to all relevant pages
- [ ] Subscribe to TermChangeNotifier on all pages
- [ ] Test term switching behavior
- [ ] Verify reports are accurate per term
- [ ] Test dashboard shows correct counts
- [ ] Performance test with real data
- [ ] Backup production database before deployment

---

## 🎉 Success Criteria

Your implementation is complete when:

1. ✅ Migration runs successfully
2. ✅ Can create new terms via Settings
3. ✅ Graduated students don't appear in new terms
4. ✅ Can switch between terms and see historical data
5. ✅ All reports filter by TermId
6. ✅ Dashboard shows term-specific metrics
7. ✅ Pages auto-refresh when term changes
8. ✅ No memory leaks from event subscriptions

---

## 💡 Pro Tips

1. **Use TermReportService for all reports** - Don't write raw LINQ queries
2. **Always test with graduated students** - This is the critical behavior
3. **Check indexes are working** - Use SQLite EXPLAIN QUERY PLAN
4. **Monitor performance** - Watch query execution times
5. **Keep terms closed after created** - Don't modify historical data

---

## 🚀 You're Ready!

All code is complete and builds successfully. Just run the migration and start testing!

**Questions?** Check the detailed guide: `TERM_SNAPSHOT_MODEL_GUIDE.md`

**Need examples?** See: `Pages/TermReportsExample.razor`

**Want query templates?** Check: `Services/TermReportService.cs`

---

**Happy coding! 🎓**
