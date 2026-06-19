using Server.Models;

namespace Server.Database.Entities;

public class LoanEntity
{
    public Guid Id { get; protected set; }
    public DateTime LoanDate { get; protected set; }
    public DateTime DueDate { get; protected set; }
    public DateTime? ReturnDate { get; protected set; }
    public LoanStatus Status { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    public Guid EquipmentId { get; protected set; }
    public EquipmentEntity Equipment { get; protected set; }

    public Guid BorrowerId { get; protected set; }
    public BorrowerEntity Borrower { get; protected set; }

    public UserEntity PreformedBy { get; protected set; }
    public Guid PreformedById { get; protected set; }


#pragma warning disable CS8618
    protected LoanEntity()
    {
    }
#pragma warning restore CS8618

    public LoanEntity(DateTime loanDate, DateTime dueDate, EquipmentEntity equipment,
        BorrowerEntity borrower, UserEntity preformedBy)
    {
        Id = Guid.NewGuid();
        LoanDate = loanDate;
        DueDate = dueDate;
        Status = LoanStatus.OnLoan;
        Equipment = equipment;
        EquipmentId = equipment.Id;
        Borrower = borrower;
        BorrowerId = borrower.Id;
        PreformedBy = preformedBy;
        PreformedById = preformedBy.Id;
    }

    public void UpdateStatus(LoanStatus status)
    {
        Status = status;
        if (status == LoanStatus.Returned)
        {
            ReturnDate = DateTime.UtcNow;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /* AI used (Microsoft Copilot)
    Reason: I was struggling with a overflow stock issue when mapping equipment to DTO, because the Equipment entity has a reference to the current loan,
    which in turn has a reference to the equipment. This caused an infinite loop when mapping to DTO. By adding an optional parameter to the ToDto method, 
    I can control whether to include the equipment details in the loan DTO, thus breaking the loop when necessary.
     */
    public LoanDto ToDto(bool includeEquipment = false)
    {
        {
            if (Equipment is null)
                throw new InvalidOperationException(
                    $"Loan {Id} has null Equipment. Ensure `Equipment` is included when querying the database before mapping to DTO (use Include(l => l.Equipment)).");

            if (PreformedBy is null)
                throw new InvalidOperationException(
                    $"Loan {Id} has null PreformedBy. Ensure `PreformedBy` is included when querying the database before mapping to DTO (use Include(l => l.PreformedBy)).");

            if (Borrower is null)
                throw new InvalidOperationException(
                    $"Loan {Id} has null Borrower. Ensure `Borrower` is included when querying the database before mapping to DTO (use Include(l => l.Borrower)).");
            
            return new LoanDto
            {
                Id = Id,
                PreformedById = PreformedById,
                PreformedBy = PreformedBy.ToDto(),
                EquipmentId = EquipmentId,
                Equipment = (includeEquipment ? Equipment.ToDto(includeCurrentLoan: false) : null)!,
                Borrower = Borrower.ToDto(),
                LoanDate = LoanDate,
                DueDate = DueDate,
                ReturnDate = ReturnDate,
                Status = Status
            };
        }
    }
}