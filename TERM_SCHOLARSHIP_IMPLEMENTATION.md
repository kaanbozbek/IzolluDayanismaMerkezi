# Term-Based Scholarship Configuration Implementation

## ✅ Completed Features

### 1. Database Schema
- **Created**: `TermScholarshipConfig` entity
  - Properties: `TermId`, `YearlyAmount`, `MonthlyAmount`, `LastUpdatedAt`, `Notes`, `CreatedAt`
  - Unique constraint on `TermId` (one config per term)
  - Foreign key relationship with `Term` entity
- **Migration**: `AddTermScholarshipConfig` applied successfully
- **Table Created**: `TermScholarshipConfigs` in SQLite database

### 2. Service Layer
- **Created**: `TermScholarshipConfigService`
  - `GetOrCreateForTermAsync(int termId)` - Returns or creates config with defaults (36000 yearly, 3000 monthly)
  - `GetAllAsync()` - Returns all configs ordered by term start date
  - `UpdateAsync(int termId, decimal yearlyAmount)` - Updates yearly, recalculates monthly
  - `UpdateByMonthlyAmountAsync(int termId, decimal monthlyAmount)` - Updates monthly, recalculates yearly
  - `DeleteAsync(int id)` - Removes configuration
- **Registered**: Service added to dependency injection in `Program.cs`

### 3. Settings Page UI
- **Replaced**: Static "Burs Tutarı" field with term-based configuration
- **Features**:
  - Term selector dropdown (MudSelect) - active terms shown first
  - Editable yearly amount field
  - Editable monthly amount field
  - Auto-calculation: changing yearly updates monthly (÷12), changing monthly updates yearly (×12)
  - Last updated timestamp display
  - "Geçmiş Dönem Burs Tutarları" table showing all term configurations
  - Table columns: Dönem, Yıllık Tutar, Aylık Tutar, Son Güncelleme
  - Active term indicator chip

## 📋 Next Steps

### High Priority (Term-Based Transformation)
1. **Reports Page** - Transform to use term-based scholarship amounts
   - Add term selector at top
   - Remove extra filter drawer
   - Load `TermScholarshipConfig` for selected term
   - Replace hard-coded amounts with `config.YearlyAmount` and `config.MonthlyAmount`
   - Calculate totals: `TotalMonthly = config.MonthlyAmount * ActiveStudentCount`

2. **Donor Detail** - Show pledge history across all terms
   - Load all `MemberScholarshipCommitments` for donor
   - Group by term
   - Display: Dönem, Taahhüt Edilen, Ödenen, Kalan
   - Optionally show monetary amounts using term scholarship config

### Medium Priority (Bugfixes)
3. **Student Graduation Flow**
   - Ensure `MezunMu = true` is set on graduation
   - Filter active students: `WHERE MezunMu = false`
   - Filter graduates: `WHERE MezunMu = true`
   - Verify student moves from active to graduate list

4. **Member Name Clickable**
   - Make member name in Members table clickable
   - Open detail dialog on click

### Low Priority (UX Improvements)
5. **Donor Stats Connection**
   - Link Reports page "Bağışçılar" stats to actual Donor entity
   - Use `DonorService.GetTotalCountAsync()` or count Members with donor flag

6. **Reports Filter Cleanup**
   - Keep only term selector as filter
   - Remove separate filter drawer/panel

## 🎯 Current State

**Completed**: 3/8 major features (37.5%)
- ✅ TermScholarshipConfig entity and service
- ✅ Database migration
- ✅ Settings page term-based UI

**In Progress**: None

**Pending**: 5 features (Reports, Donor detail, Graduation fix, Member click, Donor stats)

## 🧪 Testing Checklist

### Settings Page
- [ ] Open Settings → "Burs Tutarı" tab
- [ ] Verify term selector shows active term first
- [ ] Select a term, verify amounts load
- [ ] Change yearly amount, verify monthly updates automatically
- [ ] Change monthly amount, verify yearly updates automatically
- [ ] Save, verify success message
- [ ] Check "Geçmiş Dönem Burs Tutarları" table populates
- [ ] Verify active term shows chip indicator

### Database
- [ ] Check `TermScholarshipConfigs` table exists
- [ ] Verify unique index on `TermId`
- [ ] Insert duplicate TermId, verify constraint error
- [ ] Verify foreign key to `Terms` table works

### Service Layer
- [ ] Call `GetOrCreateForTermAsync()` with new term, verify default values
- [ ] Call `UpdateAsync()`, verify monthly is YearlyAmount/12
- [ ] Call `UpdateByMonthlyAmountAsync()`, verify yearly is MonthlyAmount*12
- [ ] Call `GetAllAsync()`, verify results ordered by term start date DESC

## 📝 Notes

### Design Decisions
- **Defaults**: New term configs default to 36000 TL yearly / 3000 TL monthly
- **Auto-calculation**: System recalculates monthly/yearly to keep values synchronized
- **One config per term**: Unique constraint ensures data integrity
- **Historical preservation**: All past term configs remain read-only in history table

### Technical Details
- **Technology**: Blazor Server, MudBlazor, EF Core, SQLite
- **Pattern**: Term-based snapshot architecture
- **Validation**: Decimal(18,2) for currency values
- **Navigation**: Eager loading with `.Include(c => c.Term)` for performance

### Breaking Changes
- Old `DefaultBursTutari` setting still stored in Settings table but no longer used in UI
- Reports page will need update to read from `TermScholarshipConfig` instead of Settings

## 🔗 Related Files

### Created
- `Data/Entities/TermScholarshipConfig.cs`
- `Services/TermScholarshipConfigService.cs`
- `Migrations/20251115150424_AddTermScholarshipConfig.cs`

### Modified
- `Data/ApplicationDbContext.cs` - Added DbSet and entity configuration
- `Program.cs` - Registered TermScholarshipConfigService
- `Pages/Settings.razor` - Replaced static field with term-based UI
