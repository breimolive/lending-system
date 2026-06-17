using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers;

public record UserRegisterRequest(string FullName, string Email, string Password);

public record UserLoginRequest(string Email, string Password);


public record UserDto(Guid Id, string FullName, string Email);

[ApiController]
[AllowAnonymous]
[Route("/api/v1/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly DatabaseContext _db;
    private readonly string _pepper;


    public UserController(ILogger<UserController> logger, DatabaseContext db, IConfiguration configuration)
    {
        _logger = logger;
        _db = db;
        _pepper = configuration["DatabasePepper"] ?? throw new ArgumentNullException("DatabasePepper");
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId));
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserDto(user.Id, user.FullName, user.Email));
    }


    [HttpPost("register")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        var existing = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existing != null)
        {
            return Conflict("User already exists");
        }

        var user = new UserEntity(request.FullName, request.Email, request.Password, _pepper);
        _db.Add(user);
        await _db.SaveChangesAsync();

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return Ok(new UserDto(user.Id, user.FullName, user.Email));
    }
    
    [HttpPost("login")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (!user.ComparePassword(request.Password, _pepper))
        {
            return Unauthorized("Invalid password");
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return Ok(new UserDto(user.Id, user.FullName, user.Email));
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}