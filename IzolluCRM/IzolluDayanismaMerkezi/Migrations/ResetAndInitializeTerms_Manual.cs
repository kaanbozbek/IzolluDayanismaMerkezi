using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IzolluVakfi.Migrations
{
    /// <summary>
    /// MANUAL MIGRATION: Reset all term-related data and initialize clean 2025-2026 term.
    /// 
    /// This migration:
    /// 1. Deletes all existing Term, StudentTerm, and MemberTermRole data
    /// 2. Does NOT delete Student or Member master records
    /// 3. Creates a clean 2025-2026 term
    /// 4. Populates StudentTerm snapshots from existing Student data
    /// 5. Populates MemberTermRole snapshots from existing Member data
    /// 
    /// IMPORTANT: Run this migration only once during the transition to the term-based snapshot model.
    /// After this migration, use TermService.OpenNewTermAsync() to create new terms.
    /// </summary>
    public partial class ResetAndInitializeTerms_Manual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ========================================
            // STEP 1: CLEAN UP OLD TERM DATA
            // ========================================
            
            migrationBuilder.Sql(@"
                -- Delete all MemberTermRole entries
                DELETE FROM MemberTermRoles;
                
                -- Delete all StudentTerm entries
                DELETE FROM StudentTerms;
                
                -- Delete all Term entries
                DELETE FROM Terms;
                
                -- Delete legacy Period entries if they exist
                DELETE FROM Periods WHERE 1=1;
            ");

            // ========================================
            // STEP 2: CREATE CLEAN 2025-2026 TERM
            // ========================================
            
            migrationBuilder.Sql(@"
                INSERT INTO Terms (Start, End, DisplayName, IsActive, Description, CreatedAt)
                VALUES (
                    '2025-01-01 00:00:00',
                    '2026-01-01 00:00:00',
                    '2025-2026',
                    1,
                    'İlk dönem - Mevcut verilerden aktarıldı',
                    datetime('now')
                );
            ");

            // ========================================
            // STEP 3: POPULATE StudentTerm SNAPSHOTS
            // ========================================
            
            migrationBuilder.Sql(@"
                -- Create StudentTerm entries for all existing students
                INSERT INTO StudentTerms (
                    StudentId,
                    TermId,
                    IsActive,
                    IsGraduated,
                    MonthlyAmount,
                    ScholarshipStart,
                    ScholarshipEnd,
                    Gpa,
                    ClassLevel,
                    DonorName,
                    Department,
                    University,
                    TotalScholarshipReceived,
                    TermNotes,
                    TranscriptNotes,
                    CreatedAt
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
            ");

            // ========================================
            // STEP 4: POPULATE MemberTermRole SNAPSHOTS
            // ========================================
            
            migrationBuilder.Sql(@"
                -- Create MemberTermRole entries for all existing members
                INSERT INTO MemberTermRoles (
                    MemberId,
                    TermId,
                    Role,
                    IsActive,
                    IsBoardOfTrustees,
                    IsExecutiveBoard,
                    IsAuditCommittee,
                    IsProvidingScholarship,
                    Status,
                    RoleStartDate,
                    RoleEndDate,
                    Notes,
                    CreatedAt
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
            ");

            // ========================================
            // STEP 5: VERIFICATION (Log counts)
            // ========================================
            
            migrationBuilder.Sql(@"
                -- This will be logged in the migration output
                SELECT 
                    'Migration Complete' AS Status,
                    (SELECT COUNT(*) FROM Terms) AS TermCount,
                    (SELECT COUNT(*) FROM StudentTerms) AS StudentTermCount,
                    (SELECT COUNT(*) FROM MemberTermRoles) AS MemberTermRoleCount,
                    (SELECT COUNT(*) FROM Students) AS TotalStudents,
                    (SELECT COUNT(*) FROM Members) AS TotalMembers;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // WARNING: Down migration will delete all term data
            // This should only be used if you need to completely roll back
            
            migrationBuilder.Sql(@"
                DELETE FROM MemberTermRoles;
                DELETE FROM StudentTerms;
                DELETE FROM Terms;
            ");
        }
    }
}
