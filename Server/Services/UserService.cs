using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Entities;
using Server.Exceptions;
using Server.Models;

namespace Server.Services;

public class UserService
{
    private readonly DatabaseContext _context;

    public UserService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Login(UserLoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return !user.ComparePassword(request.Password, user.PasswordHash)
            ? throw new UnauthorizedException("Invalid password")
            : user.ToDto();
    }
    
    public async Task<BorrowerDto> CreateBorrower(BorrowerCreateDto request)
    {
        var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existingBorrower != null)
        {
            return existingBorrower.ToDto();
        }
        
        var borrower = new BorrowerEntity(request.Email, request.FirstName, request.LastName, request.PhoneNumber);
        _context.Borrowers.Add(borrower);
        await _context.SaveChangesAsync();

        return borrower.ToDto();
    }

    public async Task<BorrowerDto> GetBorrower(string email)
    {
        var existingBorrower = await _context.Borrowers.FirstOrDefaultAsync(x => x.Email == email);
        return existingBorrower == null ? throw new NotFoundException("Borrower not found") : existingBorrower.ToDto();
    }
    
    public async Task<UserDto> GetUser(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));
        return user == null ? throw new NotFoundException("User not found") : user.ToDto();
    }
}