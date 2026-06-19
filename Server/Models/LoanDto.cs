namespace Server.Models;

public class LoanDto
{
    public required Guid Id { get; set; }
    public required UserDto PreformedBy { get; set; }
    public required Guid PreformedById { get; set; }
    public required EquipmentDto Equipment { get; set; }
    public required BorrowerDto Borrower { get; set; }
    public required Guid EquipmentId { get; set; }
    public required DateTime DueDate { get; set; }
    public required DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public required LoanStatus Status { get; set; }
}