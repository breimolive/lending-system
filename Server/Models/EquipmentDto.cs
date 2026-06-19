namespace Server.Models;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public EquipmentStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public LoanDto? CurrentLoan { get; set; }
    public required CategoryDto Category { get; set; }
}