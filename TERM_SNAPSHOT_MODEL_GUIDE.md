# Term-Based Snapshot Model - Complete Guide

## 📋 Overview

This document describes the **term-based snapshot model** implemented for the İzollu Vakfı scholarship management system. This model allows you to view and manage data across different academic periods while maintaining complete historical accuracy.

## 🎯 Core Concept

### What is a Snapshot Model?

When you open a new term (e.g., 2026-2027), the system creates **snapshots** of:
- All active students (excluding graduated ones)
- All active member roles
- Current scholarship amounts
- All other term-dependent data

Each term maintains its **own independent dataset**, allowing you to:
- View historical data by switching between terms
- See who was a student or board member during any specific period
- Track scholarship amounts and recipients per term
- Generate accurate reports for any past term

### Key Principle

**NEVER query Student or Member tables directly for reports.**  
**ALWAYS query through StudentTerm and MemberTermRole tables filtered by TermId.**

---

## 🗂️ Data Model

### 1. Term (Academic/Financial Period)

```csharp
public class Term
{
    public int Id { get; set; }
    public DateTime Start { get; set; }              // Term start date
    public DateTime End { get; set; }                // Term end date
    public string DisplayName { get; set; }          // "2025-2026"
    public bool IsActive { get; set; }               // Only ONE term can be active
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<StudentTerm> StudentTerms { get; set; }
    public ICollection<MemberTermRole> MemberTermRoles { get; set; }
}
```

### 2. Student (Global Master Record)

```csharp
public class Student
{
    public int Id { get; set; }
    public string AdSoyad { get; set; }              // Full name
    public string? TCNo { get; set; }                // National ID
    public DateTime? DogumTarihi { get; set; }       // Birth date
    // ... other GLOBAL fields (address, contact, etc.)
    
    // Navigation to term-specific data
    public ICollection<StudentTerm> Terms { get; set; }
}
```

**Important:** Student table contains only GLOBAL data that doesn't change between terms.

### 3. StudentTerm (Per-Term Snapshot)

```csharp
public class StudentTerm
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int TermId { get; set; }
    
    // TERM-DEPENDENT FIELDS:
    public bool IsActive { get; set; }               // Active scholarship in this term
    public bool IsGraduated { get; set; }            // Graduated in/before this term
    public decimal MonthlyAmount { get; set; }       // Monthly scholarship amount
    public decimal TotalScholarshipReceived { get; set; }
    public DateTime? ScholarshipStart { get; set; }
    public DateTime? ScholarshipEnd { get; set; }
    public double? Gpa { get; set; }
    public int? ClassLevel { get; set; }             // 1, 2, 3, 4 (year)
    public string? DonorName { get; set; }
    public string? Department { get; set; }
    public string? University { get; set; }
    public string? TermNotes { get; set; }
    public string? TranscriptNotes { get; set; }
}
```

### 4. Member (Global Master Record)

```csharp
public class Member
{
    public int Id { get; set; }
    public string AdSoyad { get; set; }
    public string? TCNo { get; set; }
    // ... other GLOBAL fields
    
    // Navigation to term-specific roles
    public ICollection<MemberTermRole> TermRoles { get; set; }
}
```

### 5. MemberTermRole (Per-Term Role Snapshot)

```csharp
public class MemberTermRole
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int TermId { get; set; }
    
    // TERM-DEPENDENT FIELDS:
    public string Role { get; set; }                 // "Yönetim Kurulu", "Mütevelli", etc.
    public bool IsActive { get; set; }
    public bool IsBoardOfTrustees { get; set; }      // Mütevelli
    public bool IsExecutiveBoard { get; set; }       // Yönetim Kurulu
    public bool IsAuditCommittee { get; set; }       // Denetim Kurulu
    public bool IsProvidingScholarship { get; set; }
    public string? Status { get; set; }              // "Aktif", "Pasif", etc.
    public DateTime? RoleStartDate { get; set; }
    public DateTime? RoleEndDate { get; set; }
    public string? Notes { get; set; }
}
```

---

## 🔄 Term Lifecycle

### Initial Setup (One-Time Migration)

```csharp
// Run this ONCE to transition from old model to snapshot model
var firstTerm = await termService.InitializeFirstTermAsync(
    start: new DateTime(2025, 1, 1),
    end: new DateTime(2026, 1, 1),
    description: "İlk dönem - Mevcut verilerden aktarıldı"
);
```

This will:
1. Create the 2025-2026 term
2. Copy all existing students → StudentTerm entries
3. Copy all existing members → MemberTermRole entries

### Opening a New Term

```csharp
var newTerm = await termService.OpenNewTermAsync(
    start: new DateTime(2026, 1, 1),
    end: new DateTime(2027, 1, 1),
    description: "2026-2027 akademik yılı"
);
```

**What happens:**

1. **Previous active term is deactivated**
   ```csharp
   oldTerm.IsActive = false;
   ```

2. **New term is created and set as active**
   ```csharp
   newTerm.IsActive = true;
   ```

3. **Active students are cloned (EXCLUDING graduated students)**
   ```csharp
   var activeStudents = oldStudentTerms.Where(st => !st.IsGraduated);
   
   foreach (var student in activeStudents)
   {
       var clone = new StudentTerm
       {
           StudentId = student.StudentId,
           TermId = newTerm.Id,
           IsActive = student.IsActive,
           IsGraduated = false,  // Reset for new term
           MonthlyAmount = student.MonthlyAmount,
           ClassLevel = student.ClassLevel + 1,  // Increment year
           // ... other fields
       };
   }
   ```

4. **Active member roles are cloned**
   ```csharp
   var activeMemberRoles = oldMemberRoles.Where(mtr => mtr.IsActive);
   
   foreach (var role in activeMemberRoles)
   {
       var clone = new MemberTermRole
       {
           MemberId = role.MemberId,
           TermId = newTerm.Id,
           Role = role.Role,
           IsActive = true,
           // ... other fields
       };
   }
   ```

### Critical Behavior: Graduated Students

**Rule:** Graduated students do NOT appear in new terms.

**Example:**
- **2025-2026 Term:**
  - Ali (Active, IsGraduated = false)
  - Ayşe (Active, IsGraduated = false)
  - Mehmet (Active, IsGraduated = true) ← Graduated in this term

- **When opening 2026-2027:**
  - ✅ Ali is cloned to new term
  - ✅ Ayşe is cloned to new term
  - ❌ Mehmet is NOT cloned (graduated)

- **Viewing 2025-2026 (historical):**
  - You still see all three students including Mehmet

- **Viewing 2026-2027 (current):**
  - You only see Ali and Ayşe

---

## 📊 Querying Data (The Right Way)

### ❌ WRONG: Querying Global Tables

```csharp
// NEVER DO THIS FOR REPORTS:
var students = await _context.Students
    .Where(s => s.AktifBursMu)
    .ToListAsync();
```

**Problem:** This ignores term context and gives current state, not historical data.

### ✅ CORRECT: Querying Through Snapshot Tables

```csharp
// ALWAYS DO THIS:
var students = await _context.StudentTerms
    .Where(st => st.TermId == selectedTermId 
              && st.IsActive 
              && !st.IsGraduated)
    .Include(st => st.Student)  // Join to get global data if needed
    .ToListAsync();
```

### Example Queries

#### 1. Active Scholarship Students for a Term

```csharp
public async Task<int> GetActiveScholarshipStudentCountAsync(int termId)
{
    return await _context.StudentTerms
        .Where(st => st.TermId == termId 
                  && st.IsActive 
                  && !st.IsGraduated)
        .CountAsync();
}
```

#### 2. Graduated Students for a Term

```csharp
public async Task<List<Student>> GetGraduatedStudentsAsync(int termId)
{
    return await _context.StudentTerms
        .Where(st => st.TermId == termId && st.IsGraduated)
        .Include(st => st.Student)
        .Select(st => st.Student)
        .ToListAsync();
}
```

#### 3. Total Scholarship Budget for a Term

```csharp
public async Task<decimal> GetMonthlyBudgetAsync(int termId)
{
    return await _context.StudentTerms
        .Where(st => st.TermId == termId 
                  && st.IsActive 
                  && !st.IsGraduated)
        .SumAsync(st => (decimal?)st.MonthlyAmount) ?? 0m;
}
```

#### 4. Executive Board Members for a Term

```csharp
public async Task<List<Member>> GetExecutiveBoardAsync(int termId)
{
    return await _context.MemberTermRoles
        .Where(mtr => mtr.TermId == termId 
                   && mtr.IsActive 
                   && mtr.IsExecutiveBoard)
        .Include(mtr => mtr.Member)
        .Select(mtr => mtr.Member)
        .ToListAsync();
}
```

#### 5. Scholarship Distribution by University

```csharp
public async Task<List<UniversityStatDto>> GetScholarshipByUniversityAsync(int termId)
{
    return await _context.StudentTerms
        .Where(st => st.TermId == termId 
                  && st.IsActive 
                  && !st.IsGraduated
                  && st.University != null)
        .GroupBy(st => st.University)
        .Select(g => new UniversityStatDto
        {
            University = g.Key,
            StudentCount = g.Count(),
            TotalMonthlyAmount = g.Sum(st => st.MonthlyAmount)
        })
        .OrderByDescending(u => u.StudentCount)
        .ToListAsync();
}
```

---

## 🎨 UI Implementation

### Term Selector Component

Every page should have a term selector:

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

### Auto-Refresh on Term Change

```csharp
@inject TermChangeNotifier TermChangeNotifier
@implements IDisposable

protected override async Task OnInitializedAsync()
{
    // Subscribe to term change notifications
    TermChangeNotifier.OnTermChanged += OnTermChangedHandler;
    
    // Load data for active term
    var activeTerm = await TermService.GetActiveTermAsync();
    _selectedTermId = activeTerm?.Id;
    await LoadData();
}

private async void OnTermChangedHandler()
{
    // Reload when active term changes
    var activeTerm = await TermService.GetActiveTermAsync();
    _selectedTermId = activeTerm?.Id;
    await LoadData();
    await InvokeAsync(StateHasChanged);
}

private async Task LoadData()
{
    if (!_selectedTermId.HasValue) return;
    
    // ALWAYS filter by term ID
    _students = await _context.StudentTerms
        .Where(st => st.TermId == _selectedTermId.Value)
        .Include(st => st.Student)
        .ToListAsync();
}

public void Dispose()
{
    TermChangeNotifier.OnTermChanged -= OnTermChangedHandler;
}
```

---

## 🔒 Business Rules

### 1. Only ONE Active Term

```csharp
// When setting a term as active, deactivate all others
await termService.SetActiveTermAsync(termId);
```

### 2. Graduated Students Don't Advance

```csharp
// When opening a new term, exclude graduated students
var activeStudents = oldStudentTerms.Where(st => !st.IsGraduated);
```

### 3. Historical Data is Immutable

- Once a term is closed, its StudentTerm and MemberTermRole data should NOT be modified
- Changes should only be made to the active term

### 4. All Reports Must Be Term-Aware

- Dashboard widgets
- Student lists
- Member lists
- Financial reports
- Export/PDF generation

---

## 📝 Migration Steps

### Step 1: Run the Reset Migration

```bash
# This will:
# - Delete all existing Term, StudentTerm, MemberTermRole data
# - Create fresh 2025-2026 term
# - Populate StudentTerm from existing Students
# - Populate MemberTermRole from existing Members

dotnet ef migrations add ResetAndInitializeTerms
dotnet ef database update
```

Alternatively, run the manual SQL migration file: `ResetAndInitializeTerms_Manual.cs`

### Step 2: Verify Data

```csharp
var termCount = await _context.Terms.CountAsync();
var studentTermCount = await _context.StudentTerms.CountAsync();
var memberRoleCount = await _context.MemberTermRoles.CountAsync();

Console.WriteLine($"Terms: {termCount}");
Console.WriteLine($"Student Terms: {studentTermCount}");
Console.WriteLine($"Member Roles: {memberRoleCount}");
```

### Step 3: Update All Services

Replace direct queries to Students/Members with queries through StudentTerms/MemberTermRoles.

### Step 4: Update All Blazor Pages

Add TermSelector components and ensure all data loading is filtered by TermId.

### Step 5: Test Behavior

1. **Test 1: Create New Term**
   - Open 2026-2027 term
   - Verify graduated students from 2025-2026 don't appear

2. **Test 2: Switch Between Terms**
   - View 2025-2026 → should show all students including graduates
   - View 2026-2027 → should show only active students

3. **Test 3: Report Accuracy**
   - Compare reports for different terms
   - Verify counts and amounts are different per term

---

## 🚀 Performance Optimization

### Database Indexes

The DbContext already includes critical indexes:

```csharp
// StudentTerm indexes
entity.HasIndex(e => e.TermId);
entity.HasIndex(e => e.StudentId);
entity.HasIndex(e => new { e.TermId, e.StudentId }).IsUnique();
entity.HasIndex(e => new { e.TermId, e.IsActive });
entity.HasIndex(e => new { e.TermId, e.IsGraduated });

// MemberTermRole indexes
entity.HasIndex(e => e.TermId);
entity.HasIndex(e => e.MemberId);
entity.HasIndex(e => new { e.TermId, e.MemberId });
entity.HasIndex(e => new { e.TermId, e.IsActive });
entity.HasIndex(e => new { e.TermId, e.IsExecutiveBoard });
```

### SQLite Optimizations

```csharp
// WAL mode for better concurrency
PRAGMA journal_mode = WAL;
PRAGMA synchronous = NORMAL;
PRAGMA temp_store = MEMORY;
PRAGMA mmap_size = 30000000000;
```

---

## ✅ Checklist

Before going to production:

- [ ] Migration completed successfully
- [ ] All services use term-aware queries
- [ ] All pages have TermSelector component
- [ ] All pages subscribe to TermChangeNotifier
- [ ] All pages implement IDisposable
- [ ] Dashboard shows term-specific data
- [ ] Reports filter by TermId
- [ ] Excel exports are term-aware
- [ ] PDF exports are term-aware
- [ ] Graduated student behavior verified
- [ ] Term switching tested
- [ ] Performance tested with real data
- [ ] Indexes verified

---

## 🆘 Common Issues

### Issue 1: Reports Show Wrong Data

**Symptom:** Reports don't match selected term.

**Solution:** Check that queries include `.Where(x => x.TermId == selectedTermId)`

### Issue 2: Graduated Students Appear in New Term

**Symptom:** Students marked as graduated still show up in new terms.

**Solution:** Verify `OpenNewTermAsync` filters out `IsGraduated == true` students.

### Issue 3: Pages Don't Refresh

**Symptom:** Changing term doesn't update page data.

**Solution:** 
1. Ensure page subscribes to `TermChangeNotifier.OnTermChanged`
2. Call `StateHasChanged()` in the handler
3. Use `InvokeAsync()` for thread-safe updates

### Issue 4: Multiple Active Terms

**Symptom:** More than one term has `IsActive = true`.

**Solution:** Always use `SetActiveTermAsync()` which deactivates all other terms first.

---

## 📚 Additional Resources

- See `TermService.cs` for term management logic
- See `TermReportService.cs` for example term-aware queries
- See `TermReportsExample.razor` for complete UI example
- See `ApplicationDbContext.cs` for entity configurations and indexes

---

## 🎓 Summary

**Remember:**
1. Each term is a snapshot in time
2. NEVER query Student/Member directly for reports
3. ALWAYS filter by TermId
4. Graduated students don't advance to new terms
5. Historical data is preserved and viewable by switching terms

This design ensures:
- ✅ Complete historical accuracy
- ✅ Independent term management
- ✅ Flexible reporting across periods
- ✅ Data integrity and consistency
