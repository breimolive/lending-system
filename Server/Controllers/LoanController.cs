using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("/api/v1/loans")]
public class LoanController : ControllerBase
{
    private readonly LoanService _loanService;

    public LoanController(LoanService loanService)
    {
        _loanService = loanService;
    }

    [Authorize]
    [HttpGet("equipment/{equipmentId}/loans")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<LoanDto>), StatusCodes.Status200OK)]
    public async Task<List<LoanDto>> GetLoans([FromRoute] string equipmentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var id = Guid.Parse(equipmentId);
        return await _loanService.GetAllLoansForAnEquipment(id);
    }

    [Authorize]
    [HttpGet("{loanId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
    public async Task<LoanDto> GetLoan([FromRoute] string loanId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var id = Guid.Parse(loanId);
        return await _loanService.GetLoan(id);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
    public async Task<LoanDto> CreateLoan([FromBody] LoanCreateDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        return await _loanService.CreateLoan(request);
    }

    [Authorize]
    [HttpPost("return")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task ReturnLoan([FromBody] string loanId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var userGuid = Guid.Parse(userId);
        var loanGuid = Guid.Parse(loanId);

        await _loanService.ReturnLoan(userGuid, loanGuid);
    }
}