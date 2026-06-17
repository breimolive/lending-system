namespace Server.Models;

public record CreateCategoryDto
{
    public string Name { get; set; } = null!;
}