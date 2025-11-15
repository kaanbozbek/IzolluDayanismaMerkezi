using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IzolluVakfi.Services;

/// <summary>
/// Service for managing academic/financial terms and creating term-based snapshots.
/// </summary>
public class TermService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TermService> _logger;

    public TermService(ApplicationDbContext context, ILogger<TermService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets the currently active term ID.
    /// </summary>
    /// <returns>The ID of the active term, or null if no active term exists.</returns>
    public async Task<int?> GetActiveTermIdAsync()
    {
        var activeTerm = await _context.Terms
            .Where(t => t.IsActive)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

        return activeTerm == 0 ? null : activeTerm;
    }

    /// <summary>
    /// Gets the currently active term.
    /// </summary>
    /// <returns>The active term entity, or null if no active term exists.</returns>
    public async Task<Term?> GetActiveTermAsync()
    {
        return await _context.Terms
            .Where(t => t.IsActive)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets all terms ordered by start date descending.
    /// </summary>
    /// <returns>List of all terms.</returns>
    public async Task<List<Term>> GetAllTermsAsync()
    {
        return await _context.Terms
            .OrderByDescending(t => t.Start)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Gets a specific term by ID.
    /// </summary>
    public async Task<Term?> GetTermByIdAsync(int termId)
    {
        return await _context.Terms
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == termId);
    }

    /// <summary>
    /// Opens a new academic/financial term and creates snapshots for all students and members.
    /// This method:
    /// 1. Validates the date range
    /// 2. Deactivates the previous active term
    /// 3. Creates a new term
    /// 4. Clones StudentTerm entries from the previous term
    /// 5. Clones MemberTermRole entries from the previous term
    /// </summary>
    /// <param name="start">Start date of the new term</param>
    /// <param name="end">End date of the new term</param>
    /// <param name="description">Optional description for the term</param>
    /// <returns>The newly created term</returns>
    /// <exception cref="InvalidOperationException">Thrown if validation fails</exception>
    public async Task<Term> OpenNewTermAsync(DateTime start, DateTime end, string? description = null)
    {
        // Validation
        if (end <= start)
        {
            throw new InvalidOperationException("Term end date must be after start date.");
        }

        // CRITICAL: Check for duplicate start date (unique constraint)
        var existingTermWithSameStart = await _context.Terms
            .AnyAsync(t => t.Start.Date == start.Date);
        
        if (existingTermWithSameStart)
        {
            throw new InvalidOperationException("Bu başlangıç tarihine sahip bir dönem zaten mevcut.");
        }

        if (end.Year != start.Year + 1 && end.Year != start.Year)
        {
            _logger.LogWarning("Term spans unusual date range: {Start} to {End}", start, end);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Find and deactivate the previous active term
            var previousActiveTerm = await _context.Terms
                .Where(t => t.IsActive)
                .FirstOrDefaultAsync();

            if (previousActiveTerm != null)
            {
                _logger.LogInformation("Deactivating previous term: {TermName}", previousActiveTerm.DisplayName);
                previousActiveTerm.IsActive = false;
                previousActiveTerm.UpdatedAt = DateTime.UtcNow;
            }

            // Create the new term
            var newTerm = new Term
            {
                Start = start,
                End = end,
                DisplayName = $"{start.Year}-{end.Year}",
                IsActive = true,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Terms.Add(newTerm);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new term: {TermName} (ID: {TermId})", newTerm.DisplayName, newTerm.Id);

            // Clone StudentTerm entries from the previous term
            if (previousActiveTerm != null)
            {
                var oldStudentTerms = await _context.StudentTerms
                    .Where(st => st.TermId == previousActiveTerm.Id)
                    .AsNoTracking()
                    .ToListAsync();

                // CRITICAL: Filter out graduated students - they should NOT appear in the new term
                var activeStudentTerms = oldStudentTerms.Where(st => !st.IsGraduated).ToList();
                
                _logger.LogInformation("Cloning {ActiveCount} active student term entries to new term (excluding {GraduatedCount} graduated students)", 
                    activeStudentTerms.Count, oldStudentTerms.Count - activeStudentTerms.Count);

                foreach (var oldSt in activeStudentTerms)
                {
                    var newStudentTerm = new StudentTerm
                    {
                        StudentId = oldSt.StudentId,
                        TermId = newTerm.Id,
                        IsActive = oldSt.IsActive, // Preserve active status
                        IsGraduated = false, // Reset graduated flag for new term
                        MonthlyAmount = oldSt.MonthlyAmount, // Carry over scholarship amount
                        ScholarshipStart = newTerm.Start, // Reset to new term start
                        ScholarshipEnd = null, // Clear end date for new term
                        Gpa = null, // Reset GPA for new term (will be updated with new transcripts)
                        ClassLevel = oldSt.ClassLevel.HasValue ? oldSt.ClassLevel.Value + 1 : null, // Increment class level
                        DonorName = oldSt.DonorName, // Preserve donor
                        Department = oldSt.Department, // Preserve department
                        University = oldSt.University, // Preserve university
                        TotalScholarshipReceived = oldSt.TotalScholarshipReceived, // Carry over cumulative total
                        TermNotes = null, // Clear notes for new term
                        TranscriptNotes = null, // Clear transcript for new term
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.StudentTerms.Add(newStudentTerm);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("No previous term found. Creating first term without cloning data.");
            }

            // Clone MemberTermRole entries from the previous term
            if (previousActiveTerm != null)
            {
                // Only clone ACTIVE member roles to the new term
                var oldMemberRoles = await _context.MemberTermRoles
                    .Where(mtr => mtr.TermId == previousActiveTerm.Id && mtr.IsActive)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation("Cloning {Count} active member role entries to new term", oldMemberRoles.Count);

                foreach (var oldRole in oldMemberRoles)
                {
                    var newMemberRole = new MemberTermRole
                    {
                        MemberId = oldRole.MemberId,
                        TermId = newTerm.Id,
                        Role = oldRole.Role,
                        IsActive = true, // All cloned roles start as active in new term
                        IsBoardOfTrustees = oldRole.IsBoardOfTrustees,
                        IsExecutiveBoard = oldRole.IsExecutiveBoard,
                        IsAuditCommittee = oldRole.IsAuditCommittee,
                        IsProvidingScholarship = oldRole.IsProvidingScholarship,
                        Status = "Aktif", // Reset status for new term
                        RoleStartDate = newTerm.Start, // Reset to new term start
                        RoleEndDate = null, // Clear end date for new term
                        Notes = null, // Clear notes for new term
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.MemberTermRoles.Add(newMemberRole);
                }

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            _logger.LogInformation("Successfully opened new term: {TermName}", newTerm.DisplayName);
            return newTerm;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to open new term");
            throw;
        }
    }

    /// <summary>
    /// Creates the first term and initializes StudentTerm and MemberTermRole entries from existing data.
    /// This is a one-time migration method to transition from the old model to the term-based snapshot model.
    /// </summary>
    public async Task<Term> InitializeFirstTermAsync(DateTime start, DateTime end, string? description = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Check if terms already exist
            var existingTerms = await _context.Terms.AnyAsync();
            if (existingTerms)
            {
                throw new InvalidOperationException("Terms already exist. Use OpenNewTermAsync to create additional terms.");
            }

            // Create the first term
            var firstTerm = new Term
            {
                Start = start,
                End = end,
                DisplayName = $"{start.Year}-{end.Year}",
                IsActive = true,
                Description = description ?? "Initial term (migrated from existing data)",
                CreatedAt = DateTime.UtcNow
            };

            _context.Terms.Add(firstTerm);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created first term: {TermName} (ID: {TermId})", firstTerm.DisplayName, firstTerm.Id);

            // Initialize StudentTerm entries from existing Student data
            var students = await _context.Students.AsNoTracking().ToListAsync();
            _logger.LogInformation("Initializing {Count} student term entries", students.Count);

            foreach (var student in students)
            {
                var studentTerm = new StudentTerm
                {
                    StudentId = student.Id,
                    TermId = firstTerm.Id,
                    IsActive = student.AktifBursMu,
                    IsGraduated = student.MezunMu,
                    MonthlyAmount = student.AylikTutar,
                    ScholarshipStart = student.BursBaslangicTarihi,
                    ScholarshipEnd = student.BursBitisTarihi,
                    Gpa = null, // Will be populated from transcript
                    ClassLevel = student.Sinif,
                    DonorName = student.BagisciAdi,
                    Department = student.Bolum,
                    University = student.Universite,
                    TotalScholarshipReceived = student.ToplamAlinanBurs,
                    TermNotes = student.Notlar,
                    TranscriptNotes = student.TranskriptNotu,
                    CreatedAt = DateTime.UtcNow
                };

                _context.StudentTerms.Add(studentTerm);
            }

            await _context.SaveChangesAsync();

            // Initialize MemberTermRole entries from existing Member data
            var members = await _context.Members.AsNoTracking().ToListAsync();
            _logger.LogInformation("Initializing {Count} member role entries", members.Count);

            foreach (var member in members)
            {
                // Determine the member's role based on their flags
                string role = "Üye"; // Default role
                if (member.IsMutevelli)
                    role = "Mütevelli";
                else if (member.IsYonetimKurulu)
                    role = "Yönetim Kurulu";
                else if (member.IsDenetimKurulu)
                    role = "Denetim Kurulu";

                var memberRole = new MemberTermRole
                {
                    MemberId = member.Id,
                    TermId = firstTerm.Id,
                    Role = role,
                    IsActive = member.AktifMi,
                    IsBoardOfTrustees = member.IsMutevelli,
                    IsExecutiveBoard = member.IsYonetimKurulu,
                    IsAuditCommittee = member.IsDenetimKurulu,
                    IsProvidingScholarship = member.BursVeriyor,
                    Status = member.Durum,
                    RoleStartDate = member.UyelikBaslangicTarihi,
                    RoleEndDate = null,
                    Notes = member.Notlar,
                    CreatedAt = DateTime.UtcNow
                };

                _context.MemberTermRoles.Add(memberRole);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully initialized first term with existing data");
            return firstTerm;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to initialize first term");
            throw;
        }
    }

    /// <summary>
    /// Updates a term's details (not recommended after data has been added).
    /// </summary>
    public async Task<Term> UpdateTermAsync(int termId, DateTime? start = null, DateTime? end = null, string? displayName = null, string? description = null)
    {
        var term = await _context.Terms.FindAsync(termId);
        if (term == null)
        {
            throw new InvalidOperationException($"Term with ID {termId} not found.");
        }

        // Validate end date is after start date
        var newStart = start ?? term.Start;
        var newEnd = end ?? term.End;
        if (newEnd <= newStart)
        {
            throw new InvalidOperationException("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
        }

        // Check for duplicate StartDate (excluding current term)
        if (start.HasValue)
        {
            var existingTermWithSameStart = await _context.Terms
                .AnyAsync(t => t.Id != termId && t.Start.Date == start.Value.Date);
            if (existingTermWithSameStart)
            {
                throw new InvalidOperationException("Bu başlangıç tarihine sahip bir dönem zaten mevcut.");
            }
        }

        if (start.HasValue)
            term.Start = start.Value;
        
        if (end.HasValue)
            term.End = end.Value;
        
        if (displayName != null)
            term.DisplayName = displayName;
        
        if (description != null)
            term.Description = description;

        term.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return term;
    }

    /// <summary>
    /// Sets a specific term as the active term (deactivating all others).
    /// </summary>
    public async Task SetActiveTermAsync(int termId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var term = await _context.Terms.FindAsync(termId);
            if (term == null)
            {
                throw new InvalidOperationException($"Term with ID {termId} not found.");
            }

            // Deactivate all terms
            var allTerms = await _context.Terms.ToListAsync();
            foreach (var t in allTerms)
            {
                t.IsActive = false;
                t.UpdatedAt = DateTime.UtcNow;
            }

            // Activate the selected term
            term.IsActive = true;
            term.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Set term {TermName} (ID: {TermId}) as active", term.DisplayName, termId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to set active term");
            throw;
        }
    }

    /// <summary>
    /// Deletes a term and all its associated data (StudentTerms, MemberTermRoles).
    /// Cannot delete the active term.
    /// </summary>
    public async Task DeleteTermAsync(int termId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var term = await _context.Terms.FindAsync(termId);
            if (term == null)
            {
                throw new InvalidOperationException($"Term with ID {termId} not found.");
            }

            // Check if this term is active (from SystemSettings, NOT Term.IsActive)
            var systemSettings = await _context.SystemSettings.FirstOrDefaultAsync();
            if (systemSettings?.ActiveTermId == termId)
            {
                throw new InvalidOperationException("Cannot delete the active term. Please set another term as active first.");
            }

            // Delete associated StudentTerms
            var studentTerms = await _context.StudentTerms
                .Where(st => st.TermId == termId)
                .ToListAsync();
            _context.StudentTerms.RemoveRange(studentTerms);

            // Delete associated MemberTermRoles
            var memberRoles = await _context.MemberTermRoles
                .Where(mtr => mtr.TermId == termId)
                .ToListAsync();
            _context.MemberTermRoles.RemoveRange(memberRoles);

            // Delete the term itself
            _context.Terms.Remove(term);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Deleted term {TermName} (ID: {TermId}) and all associated data", term.DisplayName, termId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to delete term {TermId}", termId);
            throw;
        }
    }
}
