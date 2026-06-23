using Server.Models;

namespace Server.Database.Entities;

public class BorrowerEntity
{
    public const int MaxEmailLength = 200;
    public const int MaxFirstNameLength = 100;
    public const int MaxLastNameLength = 100;
    public const int MaxPhoneNumberLength = 8;
    public Guid Id { get; protected set; }
    public string Email { get; protected set; }
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public string PhoneNumber { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    
#pragma warning disable CS8618
    protected BorrowerEntity(){}
#pragma warning restore CS8618
    
    public BorrowerEntity(string email, string firstName, string lastName, string phoneNumber)
    {
        if (email.Length > MaxEmailLength)
        {
            throw new ArgumentException($"Email is too long, maximum email length is {MaxEmailLength} characters long.", nameof(email));
        }
        if (firstName.Length > MaxFirstNameLength)
        {
            throw new ArgumentException($"First name is too long, maximum first name length is {MaxFirstNameLength} characters long.", nameof(firstName));
        }
        if (lastName.Length > MaxLastNameLength)
        {
            throw new ArgumentException($"Last name is too long, maximum last name length is {MaxLastNameLength} characters long.", nameof(lastName));
        }
        if (phoneNumber.Length > MaxPhoneNumberLength)
        {
            throw new ArgumentException($"Phone number is too long, maximum phone number length is {MaxPhoneNumberLength} characters long.", nameof(phoneNumber));
        }
        
        Id = Guid.NewGuid();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }
    
    public BorrowerDto ToDto()
    {
        return new BorrowerDto
        {
            Id = Id,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = PhoneNumber,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}