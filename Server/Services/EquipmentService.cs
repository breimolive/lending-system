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

    public async Task<EquipmentQueriedDto> GetEquipments(EquipmentQueryDto? queryDto)
    {
        var query = _context.Equipment
            .Include(x=>x.CurrentLoan).ThenInclude(x=>x!.Borrower)
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.PreformedBy)
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.Equipment).ThenInclude(x => x.Category)
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (queryDto == null)
        {
            return new EquipmentQueriedDto();
        }

        if (!string.IsNullOrEmpty(queryDto.Name))
        {
            query = query.Where(x => x.Name.Contains(queryDto.Name));
        }

        if (!string.IsNullOrEmpty(queryDto.Borrower))
        {
            var normalized = queryDto.Borrower.Trim().ToLowerInvariant();
            query = query.Where(x => x.CurrentLoan != null && x.CurrentLoan.Borrower.FirstName.ToLower().Contains(normalized)
                                     || x.CurrentLoan != null && x.CurrentLoan.Borrower.LastName.ToLower().Contains(normalized))
                .Where(x=>x.CurrentLoan != null && x.CurrentLoan.Status == LoanStatus.OnLoan);
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

        var pageNumber = queryDto.PageNumber ?? 1;
        var pageSize = queryDto.PageSize ?? 10;

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var itemEntities = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new EquipmentQueriedDto
        {
            Equipments = itemEntities.Select(i => i.ToDto()).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<EquipmentDto> GetEquipment(Guid id)
    {
        var equipment = await _context.Equipment
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.PreformedBy)
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.Equipment).ThenInclude(x => x.Category)
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == id);

        return equipment == null ? throw new NotFoundException("Equipment not found") : equipment.ToDto();
    }

    public async Task<EquipmentDto> CreateEquipment(EquipmentCreateDto createDto)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == createDto.CategoryName);
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
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.PreformedBy)
            .Include(x => x.CurrentLoan).ThenInclude(x => x!.Equipment).ThenInclude(x => x.Category)
            .Include(x => x.Category)
            .Where(x => !x.IsDeleted)
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

        if (equipment.IsDeleted)
        {
            throw new Exception("Equipment already deleted");
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
    
    public async Task<List<CategoryDto>> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return categories.Select(c => c.ToDto()).ToList();
    }
}