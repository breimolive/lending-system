using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("/api/v1/equipment")]
public class EquipmentController: ControllerBase
{
    private readonly EquipmentService _equipmentService;

    public EquipmentController(EquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<EquipmentDto>), StatusCodes.Status200OK)]
    public async Task<List<EquipmentDto>> GetEquipment([FromQuery] EquipmentQueryDto query)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }
        
        return await _equipmentService.GetEquipments(query);
    }

    [Authorize]
    [HttpGet("{equipmentId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    public async Task<EquipmentDto> GetEquipmentById([FromRoute] string equipmentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }
        
        var guidEquipmentId = Guid.Parse(equipmentId);
        return await _equipmentService.GetEquipment(guidEquipmentId);
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    public async Task<EquipmentDto> CreateEquipment([FromBody] EquipmentCreateDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        return await _equipmentService.CreateEquipment(request);
    }
    
    [Authorize]
    [HttpPut("update/{equipmentId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    public async Task<EquipmentDto> UpdateEquipment([FromRoute] string equipmentId, [FromBody] EquipmentUpdateDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        ArgumentNullException.ThrowIfNull(equipmentId);
        var guidEquipmentId = Guid.Parse(equipmentId);
        
        return await _equipmentService.UpdateEquipment(guidEquipmentId, request);
    }

    [Authorize]
    [HttpDelete("delete/{equipmentId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTask([FromRoute] string equipmentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }
        
        await _equipmentService.DeleteEquipment(Guid.Parse(equipmentId));
        return NoContent();
    }
    
    [Authorize]
    [HttpPost("category")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    public async Task<CategoryDto> CreateCategory([FromBody] string name)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }
        return await _equipmentService.CreateCategory(name);
    }
}