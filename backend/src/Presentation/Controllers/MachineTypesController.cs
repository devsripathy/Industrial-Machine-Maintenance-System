using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SentinelOps.Domain.Entities;
using SentinelOps.Infrastructure.Persistence;

namespace SentinelOps.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MachineTypesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MachineTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<IEnumerable<MachineType>>> GetMachineTypes()
    {
        return Ok(await _context.MachineTypes.OrderBy(mt => mt.Name).ToListAsync());
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<MachineType>> GetMachineType(int id)
    {
        var machineType = await _context.MachineTypes.FindAsync(id);
        return machineType is null ? NotFound() : Ok(machineType);
    }

    [HttpPost]
    [Authorize(Policy = "machines:write")]
    public async Task<ActionResult<MachineType>> CreateMachineType([FromBody] MachineType machineType)
    {
        if (string.IsNullOrWhiteSpace(machineType.Name) || string.IsNullOrWhiteSpace(machineType.Code))
        {
            return BadRequest(new { message = "Name and code are required." });
        }

        if (await _context.MachineTypes.AnyAsync(mt => mt.Code == machineType.Code))
        {
            return Conflict(new { message = "A machine type with this code already exists." });
        }

        _context.MachineTypes.Add(machineType);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMachineType), new { id = machineType.Id }, machineType);
    }
}
