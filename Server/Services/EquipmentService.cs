using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Entities;
using Server.Exceptions;
using Server.Models;

namespace Server.Services;

public class EquipmentService
{
    private readonly DatabaseContext _context;

    public EquipmentService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<EquipmentDto>> GetEquipments(EquipmentQueryDto? queryDto)
    {
        var query = _context.Equipment
            .Include(x => x.CurrentLoan)
            .Include(x => x.Category)
            .AsQueryable();

        if (queryDto == null)
        {
            return [];
        }

        if (!string.IsNullOrEmpty(queryDto.Name))
        {
            query = query.Where(x => x.Name.Contains(queryDto.Name));
        }

        if (!string.IsNullOrEmpty(queryDto.Category))
        {
            var normalized = queryDto.Category.Trim().ToLowerInvariant();
            query = query.Where(x => x.Category.Name.ToLower() == normalized);
        }

        if (!string.IsNullOrEmpty(queryDto.SerialNumber))
        {
            query = query.Where(x => x.SerialNumber != null && x.SerialNumber.Contains(queryDto.SerialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto.Status))
        {
            if (!Enum.TryParse<EquipmentStatus>(queryDto.Status, true, out var status))
            {
                throw new BadRequestException("Invalid status value");
            }

            query = query.Where(x => x.Status == status);
        }

        if (queryDto.SortBy.HasValue)
        {
            query = queryDto.SortBy switch
            {
                EquipmentSortBy.None => query,
                EquipmentSortBy.Name => queryDto.Ascending
                    ? query.OrderBy(x => x.Name)
                    : query.OrderByDescending(x => x.Name),

                EquipmentSortBy.Category => queryDto.Ascending
                    ? query.OrderBy(x => x.Category)
                    : query.OrderByDescending(x => x.Category),

                EquipmentSortBy.Status => queryDto.Ascending
                    ? query.OrderBy(x => x.Status)
                    : query.OrderByDescending(x => x.Status),

                _ => query
            };
        }

        return await query
            .Select(x => x.ToDto())
            .ToListAsync();
    }

    public async Task<EquipmentDto> GetEquipment(Guid id)
    {
        var equipment = await _context.Equipment
            .Include(x => x.CurrentLoan)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        return equipment == null ? throw new NotFoundException("Equipment not found") : equipment.ToDto();
    }

    public async Task<EquipmentDto> CreateEquipment(EquipmentCreateDto createDto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == createDto.Category.Id);
        if (category == null)
        {
            throw new NotFoundException("Category not found");
        }

        var equipment = new EquipmentEntity(createDto.Name, createDto.Description, createDto.SerialNumber,
            createDto.Status, category);
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        return equipment.ToDto();
    }

    public async Task<EquipmentDto> UpdateEquipment(Guid id, EquipmentUpdateDto updateDto)
    {
        var equipment = await _context.Equipment
            .Include(x => x.CurrentLoan)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (equipment == null)
        {
            throw new NotFoundException("Equipment not found");
        }

        if (!string.IsNullOrEmpty(updateDto.Name))
        {
            equipment.UpdateName(updateDto.Name);
        }

        if (!string.IsNullOrEmpty(updateDto.Description))
        {
            equipment.UpdateDescription(updateDto.Description);
        }

        if (!string.IsNullOrEmpty(updateDto.SerialNumber))
        {
            equipment.UpdateSerialNumber(updateDto.SerialNumber);
        }

        if (!string.IsNullOrEmpty(updateDto.Status))
        {
            if (!Enum.TryParse<EquipmentStatus>(updateDto.Status, true, out var status))
            {
                throw new BadRequestException("Invalid status value");
            }

            equipment.UpdateStatus(status);
        }

        if (!string.IsNullOrEmpty(updateDto.CategoryName))
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == updateDto.CategoryName);
            if (category == null)
            {
                throw new NotFoundException("Category not found");
            }

            equipment.UpdateCategory(category);
        }

        await _context.SaveChangesAsync();

        return equipment.ToDto();
    }

    public async Task DeleteEquipment(Guid id)
    {
        var equipment = await _context.Equipment.FirstOrDefaultAsync(x => x.Id == id);
        if (equipment == null)
        {
            throw new NotFoundException("Equipment not found");
        }

        equipment.MarkAsDeleted();
        await _context.SaveChangesAsync();
    }

    public async Task<CategoryDto> CreateCategory(string name)
    {
        var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        if (existing != null)
        {
            throw new BadRequestException("Category with the same name already exists");
        }

        var category = new CategoryEntity(name);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.ToDto();
    }
}