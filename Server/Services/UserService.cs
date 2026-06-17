using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Exceptions;
using Server.Models;

namespace Server.Services;

public class UserService
{
    private readonly DatabaseContext _context;
    private readonly string _pepper;

    public UserService(DatabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _pepper = configuration["DatabasePepper"] ?? throw new ArgumentNullException("DatabasePepper");
    }

    public async Task<UserDto> Login(UserLoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return !user.ComparePassword(request.Password, _pepper)
            ? throw new UnauthorizedException("Invalid password")
            : user.ToDto();
    }
    
    public async Task<UserDto> GetUser(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));
        return user == null ? throw new NotFoundException("User not found") : user.ToDto();
    }
}