namespace Server.Models;

public record UserLoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}