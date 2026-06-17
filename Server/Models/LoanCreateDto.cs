namespace Server.Models;

public class LoanCreateDto
{
    public required Guid BorrowerId { get; set; }
    public required Guid PreformedById { get; set; }
    public required Guid EquipmentId { get; set; }
    public required LoanStatus Status { get; set; }
    public required DateTime LoanDate { get; set; }
    public required DateTime DueDate { get; set; }
}