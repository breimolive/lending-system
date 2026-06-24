using Server.Models;

namespace Server.Database.Entities;

public class UserEntity
{
    public const int MinFullNameLength = 1;
    public const int MaxFullNameLength = 100;

    public const int MaxEmailLength = 200;

    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 16;
    
    public const int MaxPasswordSaltLength = 40;
    public const int MaxPasswordHashLength = 70;

    public Guid Id { get; protected set; }
    public string Email { get; protected set; }
    public string FullName { get; protected set; }
    public string PasswordHash { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

#pragma warning disable CS8618
    protected UserEntity()
    {
    }
#pragma warning restore CS8618

    public UserEntity(string fullName, string email, string password)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        PasswordHash = HashPassword(password);
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public bool ComparePassword(string password, string storedPassword)
    {
       return BCrypt.Net.BCrypt.Verify(password, storedPassword);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 13);
    }
    
    public UserDto ToDto()
    {
        return new UserDto
        {
            Id = Id,
            FullName = FullName,
            Email = Email
        };
    }
}