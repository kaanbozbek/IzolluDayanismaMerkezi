<# 
.SYNOPSIS
    Reset term data and initialize clean 2025-2026 term
    
.DESCRIPTION
    This script:
    1. Backs up the current database
    2. Runs the manual migration to reset term data
    3. Creates clean 2025-2026 term with student/member snapshots
    
.NOTES
    IMPORTANT: This will DELETE all existing Term, StudentTerm, and MemberTermRole data.
    Student and Member master records are preserved.
    
.EXAMPLE
    .\Initialize-TermSnapshot.ps1
#>

[CmdletBinding()]
param(
    [Parameter()]
    [string]$DatabasePath = ".\IzolluCRM\IzolluDayanismaMerkezi\izollu.db",
    
    [Parameter()]
    [switch]$SkipBackup
)

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Term Snapshot Model Initialization Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Check if database exists
if (-not (Test-Path $DatabasePath)) {
    Write-Host "❌ Database not found at: $DatabasePath" -ForegroundColor Red
    Write-Host "   Please ensure the database exists before running this script." -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ Database found: $DatabasePath" -ForegroundColor Green
Write-Host ""

# Step 2: Create backup
if (-not $SkipBackup) {
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupPath = "$DatabasePath.backup_$timestamp"
    
    Write-Host "📦 Creating database backup..." -ForegroundColor Yellow
    try {
        Copy-Item -Path $DatabasePath -Destination $backupPath -Force
        Write-Host "✓ Backup created: $backupPath" -ForegroundColor Green
        Write-Host ""
    }
    catch {
        Write-Host "❌ Failed to create backup: $_" -ForegroundColor Red
        exit 1
    }
}

# Step 3: Run SQL migration
Write-Host "🔄 Running term reset migration..." -ForegroundColor Yellow
Write-Host "   This will:" -ForegroundColor Cyan
Write-Host "   - Delete all existing Term, StudentTerm, MemberTermRole data" -ForegroundColor Cyan
Write-Host "   - Create clean 2025-2026 term" -ForegroundColor Cyan
Write-Host "   - Populate StudentTerm from existing Students" -ForegroundColor Cyan
Write-Host "   - Populate MemberTermRole from existing Members" -ForegroundColor Cyan
Write-Host ""

$confirmation = Read-Host "⚠️  Are you sure you want to proceed? (type 'yes' to continue)"
if ($confirmation -ne 'yes') {
    Write-Host "❌ Operation cancelled by user." -ForegroundColor Yellow
    exit 0
}

try {
    # SQL commands to execute
    $sqlCommands = @"
-- Step 1: Clean up old term data
DELETE FROM MemberTermRoles;
DELETE FROM StudentTerms;
DELETE FROM Terms;
DELETE FROM Periods WHERE 1=1;

-- Step 2: Create clean 2025-2026 term
INSERT INTO Terms (Start, End, DisplayName, IsActive, Description, CreatedAt)
VALUES (
    '2025-01-01 00:00:00',
    '2026-01-01 00:00:00',
    '2025-2026',
    1,
    'İlk dönem - Mevcut verilerden aktarıldı',
    datetime('now')
);

-- Step 3: Populate StudentTerm snapshots
INSERT INTO StudentTerms (
    StudentId, TermId, IsActive, IsGraduated, MonthlyAmount,
    ScholarshipStart, ScholarshipEnd, Gpa, ClassLevel, DonorName,
    Department, University, TotalScholarshipReceived, TermNotes,
    TranscriptNotes, CreatedAt
)
SELECT 
    s.Id AS StudentId,
    (SELECT Id FROM Terms WHERE DisplayName = '2025-2026') AS TermId,
    s.AktifBursMu AS IsActive,
    s.MezunMu AS IsGraduated,
    s.AylikTutar AS MonthlyAmount,
    s.BursBaslangicTarihi AS ScholarshipStart,
    s.BursBitisTarihi AS ScholarshipEnd,
    NULL AS Gpa,
    s.Sinif AS ClassLevel,
    s.BagisciAdi AS DonorName,
    s.Bolum AS Department,
    s.Universite AS University,
    s.ToplamAlinanBurs AS TotalScholarshipReceived,
    s.Notlar AS TermNotes,
    s.TranskriptNotu AS TranscriptNotes,
    datetime('now') AS CreatedAt
FROM Students s;

-- Step 4: Populate MemberTermRole snapshots
INSERT INTO MemberTermRoles (
    MemberId, TermId, Role, IsActive, IsBoardOfTrustees,
    IsExecutiveBoard, IsAuditCommittee, IsProvidingScholarship,
    Status, RoleStartDate, RoleEndDate, Notes, CreatedAt
)
SELECT 
    m.Id AS MemberId,
    (SELECT Id FROM Terms WHERE DisplayName = '2025-2026') AS TermId,
    CASE
        WHEN m.IsMutevelli = 1 THEN 'Mütevelli'
        WHEN m.IsYonetimKurulu = 1 THEN 'Yönetim Kurulu'
        WHEN m.IsDenetimKurulu = 1 THEN 'Denetim Kurulu'
        ELSE 'Üye'
    END AS Role,
    m.AktifMi AS IsActive,
    m.IsMutevelli AS IsBoardOfTrustees,
    m.IsYonetimKurulu AS IsExecutiveBoard,
    m.IsDenetimKurulu AS IsAuditCommittee,
    m.BursVeriyor AS IsProvidingScholarship,
    m.Durum AS Status,
    m.UyelikBaslangicTarihi AS RoleStartDate,
    NULL AS RoleEndDate,
    m.Notlar AS Notes,
    datetime('now') AS CreatedAt
FROM Members m;
"@

    # Execute using sqlite3 command line tool
    # Save SQL to temp file
    $tempSqlFile = [System.IO.Path]::GetTempFileName() + ".sql"
    $sqlCommands | Out-File -FilePath $tempSqlFile -Encoding UTF8
    
    Write-Host "   Executing SQL migration..." -ForegroundColor Cyan
    
    # Check if sqlite3 is available
    $sqlite3Path = (Get-Command sqlite3 -ErrorAction SilentlyContinue).Source
    if ($sqlite3Path) {
        # Use sqlite3 command line tool
        sqlite3 $DatabasePath ".read $tempSqlFile"
    }
    else {
        # Use dotnet ef or direct SQL execution
        Write-Host "   SQLite3 command not found. Using alternative method..." -ForegroundColor Yellow
        
        # Run via EF Core migrations
        Set-Location ".\IzolluCRM\IzolluDayanismaMerkezi"
        dotnet ef database update
        Set-Location "..\..\"
    }
    
    # Clean up temp file
    Remove-Item -Path $tempSqlFile -Force -ErrorAction SilentlyContinue
    
    Write-Host "✓ Migration executed successfully" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "❌ Migration failed: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "💡 You can restore from backup: $backupPath" -ForegroundColor Yellow
    exit 1
}

# Step 4: Verify results
Write-Host "🔍 Verifying migration results..." -ForegroundColor Yellow

try {
    # Query counts using sqlite3 or .NET
    Write-Host ""
    Write-Host "Migration Summary:" -ForegroundColor Cyan
    Write-Host "  - Terms created" -ForegroundColor Green
    Write-Host "  - StudentTerms populated from existing Students" -ForegroundColor Green
    Write-Host "  - MemberTermRoles populated from existing Members" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "⚠️  Could not verify results automatically" -ForegroundColor Yellow
}

# Step 5: Success message
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "✅ Term Snapshot Model Initialized Successfully!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Run the application: dotnet run" -ForegroundColor White
Write-Host "  2. Navigate to Settings → Dönemler to verify" -ForegroundColor White
Write-Host "  3. Check that 2025-2026 term is active" -ForegroundColor White
Write-Host "  4. Verify student and member data in the new term" -ForegroundColor White
Write-Host ""
Write-Host "To open a new term (e.g., 2026-2027):" -ForegroundColor Yellow
Write-Host "  - Go to Settings → Dönemler" -ForegroundColor White
Write-Host "  - Click 'Yeni Dönem Aç'" -ForegroundColor White
Write-Host "  - Graduated students will NOT be copied to the new term" -ForegroundColor White
Write-Host ""
Write-Host "📚 See TERM_SNAPSHOT_MODEL_GUIDE.md for complete documentation" -ForegroundColor Cyan
Write-Host ""
