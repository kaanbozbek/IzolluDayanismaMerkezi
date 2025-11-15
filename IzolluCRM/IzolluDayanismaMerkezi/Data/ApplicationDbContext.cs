using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<TranscriptRecord> TranscriptRecords { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Settings> Settings { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<StudentMeetingAttendance> StudentMeetingAttendances { get; set; }
    
    // Term-based snapshot entities
    public DbSet<Term> Terms { get; set; }
    public DbSet<StudentTerm> StudentTerms { get; set; }
    public DbSet<MemberTermRole> MemberTermRoles { get; set; }
    public DbSet<MemberScholarshipCommitment> MemberScholarshipCommitments { get; set; }
    public DbSet<TermScholarshipConfig> TermScholarshipConfigs { get; set; }
    
    // System-wide settings and payment tracking
    public DbSet<SystemSettings> SystemSettings { get; set; }
    public DbSet<ScholarshipPayment> ScholarshipPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Student entity
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AylikTutar).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ToplamAlinanBurs).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.SicilNumarasi).IsUnique();
            entity.HasMany(e => e.Transcripts)
                  .WithOne(e => e.Student)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Donor entity
        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BirimBursTutari).HasColumnType("decimal(18,2)");
        });

        // Configure Member entity
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SicilNumarasi).IsUnique();
        });

        // Configure TranscriptRecord entity
        modelBuilder.Entity<TranscriptRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Baslik).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ToplantiTuru).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Konum).HasMaxLength(500);
            entity.Property(e => e.BitisTarihi).IsRequired();
        });

        modelBuilder.Entity<StudentMeetingAttendance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.StudentId, e.MeetingId }).IsUnique();

            entity.HasOne(e => e.Student)
                  .WithMany(s => s.MeetingAttendances)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Meeting)
                  .WithMany(m => m.Attendances)
                  .HasForeignKey(e => e.MeetingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ActivityLog entity
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        // Configure Settings entity
        modelBuilder.Entity<Settings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Key).IsUnique();
        });

        // ===============================================================
        // TERM-BASED SNAPSHOT MODEL CONFIGURATION
        // ===============================================================

        // Configure Term entity
        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DisplayName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            
            // Index for quickly finding the active term
            entity.HasIndex(e => e.IsActive);
            
            // UNIQUE INDEX: Prevent duplicate start dates
            entity.HasIndex(e => e.Start)
                  .IsUnique()
                  .HasDatabaseName("IX_Terms_Start_Unique");
            
            // Index for date range queries
            entity.HasIndex(e => new { e.Start, e.End });

            entity.HasMany(e => e.StudentTerms)
                  .WithOne(e => e.Term)
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of terms with data

            entity.HasMany(e => e.MemberTermRoles)
                  .WithOne(e => e.Term)
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure StudentTerm entity
        modelBuilder.Entity<StudentTerm>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Decimal precision for monetary values
            entity.Property(e => e.MonthlyAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalScholarshipReceived).HasColumnType("decimal(18,2)");
            
            // Configure foreign key relationships
            entity.HasOne(e => e.Student)
                  .WithMany(s => s.Terms)
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Cascade); // Delete term data when student is deleted

            entity.HasOne(e => e.Term)
                  .WithMany(t => t.StudentTerms)
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);

            // CRITICAL INDEXES FOR PERFORMANCE
            // Index for filtering by term (most common query)
            entity.HasIndex(e => e.TermId)
                  .HasDatabaseName("IX_StudentTerms_TermId");

            // Index for finding a student's terms
            entity.HasIndex(e => e.StudentId)
                  .HasDatabaseName("IX_StudentTerms_StudentId");

            // Composite index for term + student queries (prevents duplicates)
            entity.HasIndex(e => new { e.TermId, e.StudentId })
                  .IsUnique()
                  .HasDatabaseName("IX_StudentTerms_TermId_StudentId");

            // Index for active scholarship queries
            entity.HasIndex(e => new { e.TermId, e.IsActive })
                  .HasDatabaseName("IX_StudentTerms_TermId_IsActive");

            // Index for graduated student queries
            entity.HasIndex(e => new { e.TermId, e.IsGraduated })
                  .HasDatabaseName("IX_StudentTerms_TermId_IsGraduated");
        });

        // Configure MemberTermRole entity
        modelBuilder.Entity<MemberTermRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Role).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            // Configure foreign key relationships
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.TermRoles)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade); // Delete role data when member is deleted

            entity.HasOne(e => e.Term)
                  .WithMany(t => t.MemberTermRoles)
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);

            // CRITICAL INDEXES FOR PERFORMANCE
            // Index for filtering by term
            entity.HasIndex(e => e.TermId)
                  .HasDatabaseName("IX_MemberTermRoles_TermId");

            // Index for finding a member's roles
            entity.HasIndex(e => e.MemberId)
                  .HasDatabaseName("IX_MemberTermRoles_MemberId");

            // Composite index for term + member queries
            entity.HasIndex(e => new { e.TermId, e.MemberId })
                  .HasDatabaseName("IX_MemberTermRoles_TermId_MemberId");

            // Index for active role queries
            entity.HasIndex(e => new { e.TermId, e.IsActive })
                  .HasDatabaseName("IX_MemberTermRoles_TermId_IsActive");

            // Index for board member queries
            entity.HasIndex(e => new { e.TermId, e.IsExecutiveBoard })
                  .HasDatabaseName("IX_MemberTermRoles_TermId_IsExecutiveBoard");

            // Index for scholarship provider queries
            entity.HasIndex(e => new { e.TermId, e.IsProvidingScholarship })
                  .HasDatabaseName("IX_MemberTermRoles_TermId_IsProvidingScholarship");
        });

        // Configure MemberScholarshipCommitment entity
        modelBuilder.Entity<MemberScholarshipCommitment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.YearlyAmountPerScholarship).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.PledgedCount).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);

            // Configure foreign key relationships
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.ScholarshipCommitments)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Term)
                  .WithMany()
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);

            // CRITICAL UNIQUE INDEX: One commitment per member per term
            entity.HasIndex(e => new { e.MemberId, e.TermId })
                  .IsUnique()
                  .HasDatabaseName("IX_MemberScholarshipCommitments_MemberId_TermId_Unique");

            // Index for term-based queries
            entity.HasIndex(e => e.TermId)
                  .HasDatabaseName("IX_MemberScholarshipCommitments_TermId");
        });

        // Configure TermScholarshipConfig entity
        modelBuilder.Entity<TermScholarshipConfig>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.YearlyAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.MonthlyAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);

            // Configure foreign key relationship with CASCADE delete
            entity.HasOne(e => e.Term)
                  .WithMany()
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Cascade);

            // CRITICAL UNIQUE INDEX: One configuration per term
            entity.HasIndex(e => e.TermId)
                  .IsUnique()
                  .HasDatabaseName("IX_TermScholarshipConfigs_TermId_Unique");
        });

        // Configure SystemSettings entity
        modelBuilder.Entity<SystemSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.AppVersion).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            // Configure foreign key to ActiveTerm
            entity.HasOne(e => e.ActiveTerm)
                  .WithMany()
                  .HasForeignKey(e => e.ActiveTermId)
                  .OnDelete(DeleteBehavior.SetNull); // Don't cascade delete when term is removed
        });

        // Configure ScholarshipPayment entity
        modelBuilder.Entity<ScholarshipPayment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(1000);

            // Configure foreign key relationships
            entity.HasOne(e => e.Commitment)
                  .WithMany()
                  .HasForeignKey(e => e.CommitmentId)
                  .OnDelete(DeleteBehavior.Restrict); // Preserve payment history

            entity.HasOne(e => e.Student)
                  .WithMany()
                  .HasForeignKey(e => e.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Term)
                  .WithMany()
                  .HasForeignKey(e => e.TermId)
                  .OnDelete(DeleteBehavior.Restrict);

            // CRITICAL INDEXES FOR PERFORMANCE
            // Index for commitment-based queries
            entity.HasIndex(e => e.CommitmentId)
                  .HasDatabaseName("IX_ScholarshipPayments_CommitmentId");

            // Index for student payment history
            entity.HasIndex(e => e.StudentId)
                  .HasDatabaseName("IX_ScholarshipPayments_StudentId");

            // Index for term-based payment queries
            entity.HasIndex(e => e.TermId)
                  .HasDatabaseName("IX_ScholarshipPayments_TermId");

            // Index for payment date queries
            entity.HasIndex(e => e.PaymentDate)
                  .HasDatabaseName("IX_ScholarshipPayments_PaymentDate");

            // Composite index for term + date range queries
            entity.HasIndex(e => new { e.TermId, e.PaymentDate })
                  .HasDatabaseName("IX_ScholarshipPayments_TermId_PaymentDate");
        });
    }
}
