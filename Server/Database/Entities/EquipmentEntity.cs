using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.Database.Entities;

public class EquipmentEntity
{
    public const int MaxNameLength = 100;
    public const int MinNameLength = 1;
    public const int MaxDescriptionLength = 500;
    public const int SerialNumberLength = 14;


    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public string? Description { get; protected set; }
    public string? SerialNumber { get; protected set; }
    public EquipmentStatus Status { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public Guid? CurrentLoanId { get; protected set; }
    public LoanEntity? CurrentLoan { get; protected set; }

    public Guid CategoryId { get; protected set; }
    public CategoryEntity Category { get; protected set; }

#pragma warning disable CS8618

    protected EquipmentEntity()
    {
    }
#pragma warning restore CS8618

    public EquipmentEntity(string name, string? description, string? serialNumber,
        CategoryEntity category)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        SerialNumber = serialNumber;
        Status = EquipmentStatus.Available;
        Category = category;
        CategoryId = category.Id;
    }

    public void UpdateName(string name)
    {
        switch (name.Length)
        {
            case > MaxNameLength:
                throw new ArgumentException(
                    $"Name is too long, maximum name length is {MaxNameLength} characters long.", nameof(name));
            case < MinNameLength:
                throw new ArgumentException(
                    $"Name is too short, minimum name length is {MinNameLength} character long.", nameof(name));
        }

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        if (description != null && description.Length > MaxDescriptionLength)
        {
            throw new ArgumentException(
                $"Description is too long, maximum description length is {MaxDescriptionLength} characters long.",
                nameof(description));
        }

        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSerialNumber(string? serialNumber)
    {
        if (serialNumber != null && serialNumber.Length != SerialNumberLength)
        {
            throw new ArgumentException($"Serial number must be exactly {SerialNumberLength} characters long.",
                nameof(serialNumber));
        }

        SerialNumber = serialNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCategory(CategoryEntity category)
    {
        Category = category;
        CategoryId = category.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCurrentLoan(LoanEntity loan)
    {
        CurrentLoan = loan;
        CurrentLoanId = loan.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(EquipmentStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /* AI used (Microsoft Copilot)
   Reason: I was struggling with a overflow stock issue when mapping equipment to DTO, because the Equipment entity has a reference to the current loan,
   which in turn has a reference to the equipment. This caused an infinite loop when mapping to DTO. By adding an optional parameter to the ToDto method,
   I can control whether to include the equipment details in the loan DTO, thus breaking the loop when necessary.
    */
    public EquipmentDto ToDto(bool includeCurrentLoan = true)
    {
        if (Category is null)
            throw new InvalidOperationException(
                $"Equipment {Id} has null Category. Ensure `Category` is included when querying the database before mapping to DTO (use Include(e => e.Category)).");

        return new EquipmentDto
        {
            Id = Id,
            Name = Name,
            Description = Description,
            SerialNumber = SerialNumber,
            Status = Status,
            IsDeleted = IsDeleted,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            CurrentLoan = includeCurrentLoan ? CurrentLoan?.ToDto(includeEquipment: false) : null,
            Category = Category.ToDto()
        };
    }
}