namespace Server.Models;

public class EquipmentCreateDto
{
    public required string Name { get; set; }
    public required EquipmentStatus Status { get; set; }
    public required CategoryDto Category { get; set; }
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
}