namespace Server.Models;

public class EquipmentQueryDto
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? SerialNumber { get; set; }
    public string? Status { get; set; }
    public string? Borrower { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public EquipmentSortBy? SortBy { get; set; }
    public bool Ascending { get; set; } = true;
}