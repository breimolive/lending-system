using System.Security.Cryptography;
using System.Text;
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
    public string PasswordSalt { get; protected set; }
    public DateTime CreatedAt { get; protected set; }

#pragma warning disable CS8618
    protected UserEntity()
    {
    }
#pragma warning restore CS8618

    public UserEntity(string fullName, string email, string password, string pepper)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        PasswordSalt = GenerateSalt();
        PasswordHash = Convert.ToBase64String(HashPassword(password, PasswordSalt, pepper));
        Email = email;
        CreatedAt = DateTime.Now;
    }

    public bool ComparePassword(string password, string pepper)
    {
        var hashBytes = Convert.FromBase64String(PasswordHash);
        var tempHash = HashPassword(password, PasswordSalt, pepper);
        return CryptographicOperations.FixedTimeEquals(hashBytes.AsSpan(), tempHash.AsSpan());
    }

    private static string GenerateSalt()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes.AsSpan());
        return Convert.ToBase64String(bytes);
    }

    private static byte[] HashPassword(string password, string salt, string pepper)
    {
        using var digest = SHA256.Create();

       var pepperBytes = Encoding.UTF8.GetBytes(pepper);
        digest.TransformBlock(pepperBytes, 0, pepperBytes.Length, null, 0);

        var saltBytes = Encoding.UTF8.GetBytes(salt);
        digest.TransformBlock(saltBytes, 0, saltBytes.Length, null, 0);

        var passwordBytes = Encoding.UTF8.GetBytes(password);
        digest.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);

        return digest.Hash!;
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