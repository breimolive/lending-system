namespace Server.Models;

public class EquipmentQueriedDto
{
    public List<EquipmentDto> Equipments { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}