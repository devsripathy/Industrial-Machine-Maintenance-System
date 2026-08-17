using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelOps.Application.DTOs;
using SentinelOps.Application.Machines.Commands;
using SentinelOps.Application.Machines.Queries;

namespace SentinelOps.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MachinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MachinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<IEnumerable<MachineDto>>> GetMachines([FromQuery] string? search = null, [FromQuery] string? typeCode = null)
    {
        var result = await _mediator.Send(new GetMachinesQuery(search, typeCode));
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<MachineDto>> GetMachine(int id)
    {
        var result = await _mediator.Send(new GetMachineByIdQuery(id));
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Policy = "machines:write")]
    public async Task<ActionResult<MachineDto>> CreateMachine([FromBody] CreateMachineRequest request)
    {
        var result = await _mediator.Send(new CreateMachineCommand(
            request.Name,
            request.Code,
            request.SerialNumber,
            request.MachineTypeId,
            request.Model,
            request.Location,
            request.SpecificationsJson,
            request.QrCodeData,
            request.InstallDate,
            request.Criticality,
            request.Status));

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return CreatedAtAction(nameof(GetMachine), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "machines:write")]
    public async Task<ActionResult<MachineDto>> UpdateMachine(int id, [FromBody] UpdateMachineRequest request)
    {
        var result = await _mediator.Send(new UpdateMachineCommand(
            id,
            request.Name,
            request.Code,
            request.SerialNumber,
            request.MachineTypeId,
            request.Model,
            request.Location,
            request.SpecificationsJson,
            request.QrCodeData,
            request.InstallDate,
            request.Criticality,
            request.Status));

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "machines:delete")]
    public async Task<IActionResult> DeleteMachine(int id)
    {
        var result = await _mediator.Send(new DeleteMachineCommand(id));
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }
}

public class CreateMachineRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public int MachineTypeId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? SpecificationsJson { get; set; }
    public string? QrCodeData { get; set; }
    public DateTime? InstallDate { get; set; }
    public SentinelOps.Domain.Enums.Criticality Criticality { get; set; }
    public SentinelOps.Domain.Enums.MachineStatus Status { get; set; } = SentinelOps.Domain.Enums.MachineStatus.Running;
}

public class UpdateMachineRequest
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? SerialNumber { get; set; }
    public int? MachineTypeId { get; set; }
    public string? Model { get; set; }
    public string? Location { get; set; }
    public string? SpecificationsJson { get; set; }
    public string? QrCodeData { get; set; }
    public DateTime? InstallDate { get; set; }
    public SentinelOps.Domain.Enums.Criticality? Criticality { get; set; }
    public SentinelOps.Domain.Enums.MachineStatus? Status { get; set; }
}
