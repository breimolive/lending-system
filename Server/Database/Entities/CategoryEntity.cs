using Server.Models;

namespace Server.Database.Entities;

public class CategoryEntity
{
    public const int MaxNameLength = 50;
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

#pragma warning disable CS8618
    protected CategoryEntity()
    {
    }
#pragma warning restore CS8618

    public CategoryEntity(string name)
    {
        if (name.Length > MaxNameLength)
        {
            throw new ArgumentException($"Name is too long, maximum name length is {MaxNameLength} characters long.",
                nameof(name));
        }

        Id = Guid.NewGuid();
        Name = name;
    }
    
    public CategoryDto ToDto()
    {
        return new CategoryDto
        {
            Id = Id,
            Name = Name
        };
    }
}