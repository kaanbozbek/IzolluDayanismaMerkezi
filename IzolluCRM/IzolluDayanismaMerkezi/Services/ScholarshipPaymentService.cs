using Microsoft.EntityFrameworkCore;
using IzolluVakfi.Data;
using IzolluVakfi.Data.Entities;

namespace IzolluVakfi.Services;

/// <summary>
/// Service for managing scholarship payments against member commitments.
/// </summary>
public class ScholarshipPaymentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ScholarshipPaymentService> _logger;

    public ScholarshipPaymentService(
        ApplicationDbContext context,
        ILogger<ScholarshipPaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new scholarship payment.
    /// </summary>
    public async Task<ScholarshipPayment> CreatePaymentAsync(ScholarshipPayment payment)
    {
        // Validate commitment exists
        var commitment = await _context.MemberScholarshipCommitments
            .Include(c => c.Member)
            .Include(c => c.Term)
            .FirstOrDefaultAsync(c => c.Id == payment.CommitmentId);

        if (commitment == null)
        {
            throw new ArgumentException($"Commitment with ID {payment.CommitmentId} not found.", nameof(payment.CommitmentId));
        }

        // Validate student exists
        var studentExists = await _context.Students.AnyAsync(s => s.Id == payment.StudentId);
        if (!studentExists)
        {
            throw new ArgumentException($"Student with ID {payment.StudentId} not found.", nameof(payment.StudentId));
        }

        // Validate term exists
        var termExists = await _context.Terms.AnyAsync(t => t.Id == payment.TermId);
        if (!termExists)
        {
            throw new ArgumentException($"Term with ID {payment.TermId} not found.", nameof(payment.TermId));
        }

        payment.CreatedAt = DateTime.UtcNow;
        payment.Status = string.IsNullOrEmpty(payment.Status) ? "Completed" : payment.Status;

        _context.ScholarshipPayments.Add(payment);
        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Created payment: Amount={Amount}, Student={StudentId}, Commitment={CommitmentId}, Term={TermId}",
            payment.Amount, payment.StudentId, payment.CommitmentId, payment.TermId);

        return payment;
    }

    /// <summary>
    /// Gets all payments for a specific commitment.
    /// </summary>
    public async Task<List<ScholarshipPayment>> GetPaymentsByCommitmentAsync(int commitmentId)
    {
        return await _context.ScholarshipPayments
            .Include(p => p.Student)
            .Include(p => p.Term)
            .Where(p => p.CommitmentId == commitmentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all payments for a specific student.
    /// </summary>
    public async Task<List<ScholarshipPayment>> GetPaymentsByStudentAsync(int studentId)
    {
        return await _context.ScholarshipPayments
            .Include(p => p.Commitment)
                .ThenInclude(c => c.Member)
            .Include(p => p.Term)
            .Where(p => p.StudentId == studentId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all payments for a specific term.
    /// </summary>
    public async Task<List<ScholarshipPayment>> GetPaymentsByTermAsync(int termId)
    {
        return await _context.ScholarshipPayments
            .Include(p => p.Student)
            .Include(p => p.Commitment)
                .ThenInclude(c => c.Member)
            .Where(p => p.TermId == termId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all payments within a date range.
    /// </summary>
    public async Task<List<ScholarshipPayment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.ScholarshipPayments
            .Include(p => p.Student)
            .Include(p => p.Commitment)
                .ThenInclude(c => c.Member)
            .Include(p => p.Term)
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets total amount paid for a commitment.
    /// </summary>
    public async Task<decimal> GetTotalPaidAmountAsync(int commitmentId)
    {
        return await _context.ScholarshipPayments
            .Where(p => p.CommitmentId == commitmentId && p.Status == "Completed")
            .SumAsync(p => p.Amount);
    }

    /// <summary>
    /// Gets total amount paid to a student across all commitments.
    /// </summary>
    public async Task<decimal> GetTotalPaidToStudentAsync(int studentId, int? termId = null)
    {
        var query = _context.ScholarshipPayments
            .Where(p => p.StudentId == studentId && p.Status == "Completed");

        if (termId.HasValue)
        {
            query = query.Where(p => p.TermId == termId.Value);
        }

        return await query.SumAsync(p => p.Amount);
    }

    /// <summary>
    /// Gets total amount paid in a term.
    /// </summary>
    public async Task<decimal> GetTotalPaidInTermAsync(int termId)
    {
        return await _context.ScholarshipPayments
            .Where(p => p.TermId == termId && p.Status == "Completed")
            .SumAsync(p => p.Amount);
    }

    /// <summary>
    /// Updates an existing payment.
    /// </summary>
    public async Task UpdatePaymentAsync(ScholarshipPayment payment)
    {
        var existing = await _context.ScholarshipPayments.FindAsync(payment.Id);
        if (existing == null)
        {
            throw new ArgumentException($"Payment with ID {payment.Id} not found.", nameof(payment.Id));
        }

        existing.Amount = payment.Amount;
        existing.PaymentDate = payment.PaymentDate;
        existing.PaymentType = payment.PaymentType;
        existing.PaymentMethod = payment.PaymentMethod;
        existing.ReferenceNumber = payment.ReferenceNumber;
        existing.Notes = payment.Notes;
        existing.Status = payment.Status;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated payment ID: {PaymentId}", payment.Id);
    }

    /// <summary>
    /// Deletes a payment (soft delete by setting status to "Cancelled").
    /// </summary>
    public async Task CancelPaymentAsync(int paymentId, string? reason = null)
    {
        var payment = await _context.ScholarshipPayments.FindAsync(paymentId);
        if (payment == null)
        {
            throw new ArgumentException($"Payment with ID {paymentId} not found.", nameof(paymentId));
        }

        payment.Status = "Cancelled";
        payment.Notes = string.IsNullOrEmpty(reason) 
            ? payment.Notes 
            : $"{payment.Notes}\n[Cancelled: {reason}]";
        payment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Cancelled payment ID: {PaymentId}, Reason: {Reason}", paymentId, reason);
    }

    /// <summary>
    /// Hard deletes a payment (use with caution).
    /// </summary>
    public async Task DeletePaymentAsync(int paymentId)
    {
        var payment = await _context.ScholarshipPayments.FindAsync(paymentId);
        if (payment == null)
        {
            throw new ArgumentException($"Payment with ID {paymentId} not found.", nameof(paymentId));
        }

        _context.ScholarshipPayments.Remove(payment);
        await _context.SaveChangesAsync();

        _logger.LogWarning("Permanently deleted payment ID: {PaymentId}", paymentId);
    }
}
