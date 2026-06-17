using System.Security.Cryptography;
using System.Text;

namespace Server.Database;

public class UserEntity
{
    public const int MinFullNameLength = 1;
    public const int MaxFullNameLength = 60;

    public const int MaxEmailLength = 100;

    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 16;

    public Guid Id { get; protected set; }
    public string Email { get; protected set; }
    public string FullName { get; protected set; }
    public string Hash { get; protected set; }
    public string Salt { get; protected set; }

#pragma warning disable CS8618
    protected UserEntity()
    {
    }
#pragma warning restore CS8618

    public UserEntity(string fullName, string email, string password, string pepper)
    {
        Id = Guid.NewGuid();
        ;
        FullName = fullName;
        Salt = GenerateSalt();
        Hash = Convert.ToBase64String(HashPassword(password, Salt, pepper));
        Email = email;
    }

    public bool ComparePassword(string password, string pepper)
    {
        byte[] hashBytes = Convert.FromBase64String(Hash);
        byte[] tempHash = HashPassword(password, Salt, pepper);
        return CryptographicOperations.FixedTimeEquals(hashBytes.AsSpan(), tempHash.AsSpan());
    }

    private static string GenerateSalt()
    {
        byte[] bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes.AsSpan());
        return Convert.ToBase64String(bytes);
    }

    private static byte[] HashPassword(string password, string salt, string pepper)
    {
        using SHA256 digest = SHA256.Create();

        byte[] pepperBytes = Encoding.UTF8.GetBytes(pepper);
        digest.TransformBlock(pepperBytes, 0, pepperBytes.Length, null, 0);

        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        digest.TransformBlock(saltBytes, 0, saltBytes.Length, null, 0);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        digest.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);

        return digest.Hash!;
    }
}