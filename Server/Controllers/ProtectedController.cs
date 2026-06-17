using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/protected")]
public class ProtectedController : ControllerBase
{
    [HttpGet("data")]
    public string GetData()
    {
        return "This is protected data";
    }
}