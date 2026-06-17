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

    public LoanEntity(DateTime loanDate, DateTime dueDate, LoanStatus status, EquipmentEntity equipment,
        BorrowerEntity borrower, UserEntity preformedBy)
    {
        Id = Guid.NewGuid();
        LoanDate = loanDate;
        DueDate = dueDate;
        Status = status;
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
    
    public LoanDto ToDto()
    {
        return new LoanDto
        {
            Id = Id,
            PreformedById = PreformedById,
            PreformedBy = PreformedBy.ToDto(),
            EquipmentId = EquipmentId,
            Equipment = Equipment.ToDto(),
            LoanDate = LoanDate,
            DueDate = DueDate,
            ReturnDate = ReturnDate,
            Status = Status
        };
    }
}