using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Entities;
using Server.Exceptions;
using Server.Models;

namespace Server.Services;

public class LoanService
{
    private readonly DatabaseContext _context;

    public LoanService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<LoanDto>> GetAllLoansForAnEquipment(Guid equipmentId)
    {
        var loans = await _context.Loans
            .Where(x => x.EquipmentId == equipmentId)
            .ToListAsync();

        if (loans == null)
        {
            throw new NotFoundException("Loans not found");
        }

        return loans
            .Select(x => x.ToDto())
            .ToList();
    }

    public async Task<LoanDto> GetLoan(Guid loanId)
    {
        var loan = await _context.Loans
            .FirstOrDefaultAsync(x => x.Id == loanId);

        return loan == null ? throw new NotFoundException("Loan not found") : loan.ToDto();
    }

    public async Task<LoanDto> CreateLoan(LoanCreateDto request)
    {
        var equipment = await _context.Equipment
            .FirstOrDefaultAsync(x => x.Id == request.EquipmentId);

        var borrower = await _context.Borrowers
            .FirstOrDefaultAsync(x => x.Id == request.BorrowerId);

        var preformedBy = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.PreformedById);

        if (equipment == null)
        {
            throw new NotFoundException("Equipment not found");
        }

        if (borrower == null)
        {
            throw new NotFoundException("Borrower not found");
        }

        if (preformedBy == null)
        {
            throw new NotFoundException("User not found");
        }

        var loan = new LoanEntity(request.LoanDate, request.DueDate, request.Status, equipment, borrower, preformedBy);
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return loan.ToDto();
    }

    public async Task ReturnLoan(Guid userId, Guid loanId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var loan = await _context.Loans
            .Where(x => x.PreformedById == userId)
            .FirstOrDefaultAsync(x => x.Id == loanId);

        if (loan == null)
        {
            throw new NotFoundException("Loan not found");
        }

        if (loan.Status != LoanStatus.Returned)
        {
            loan.UpdateStatus(LoanStatus.Returned);
        }

        await _context.SaveChangesAsync();
    }
}