using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelOps.Application.DTOs;
using SentinelOps.Application.WorkOrders.Commands;
using SentinelOps.Application.WorkOrders.Queries;
using SentinelOps.Domain.Enums;

namespace SentinelOps.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<List<WorkOrderDto>>> GetWorkOrders([FromQuery] int? machineId = null, [FromQuery] string? status = null, [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetWorkOrdersQuery(machineId, status, search));
        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "machines:read")]
    public async Task<ActionResult<WorkOrderDto>> GetWorkOrder(int id)
    {
        var result = await _mediator.Send(new GetWorkOrderByIdQuery(id));
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Policy = "machines:write")]
    public async Task<ActionResult<WorkOrderDto>> CreateWorkOrder([FromBody] CreateWorkOrderRequest request)
    {
        var result = await _mediator.Send(new CreateWorkOrderCommand(
            request.Title,
            request.Description,
            request.MachineId,
            request.RequestedByUserId,
            request.AssignedToUserId,
            request.Status,
            request.Priority,
            request.Type,
            request.Criticality,
            request.EstimatedHours,
            request.EstimatedCost,
            request.DueDate,
            request.FailureCode,
            request.RootCause,
            request.ResolutionNotes,
            request.ChecklistJson));

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return CreatedAtAction(nameof(GetWorkOrder), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "machines:write")]
    public async Task<ActionResult<WorkOrderDto>> UpdateWorkOrder(int id, [FromBody] UpdateWorkOrderRequest request)
    {
        var result = await _mediator.Send(new UpdateWorkOrderCommand(
            id,
            request.Title,
            request.Description,
            request.MachineId,
            request.RequestedByUserId,
            request.AssignedToUserId,
            request.Status,
            request.Priority,
            request.Type,
            request.Criticality,
            request.EstimatedHours,
            request.EstimatedCost,
            request.DueDate,
            request.FailureCode,
            request.RootCause,
            request.ResolutionNotes,
            request.ChecklistJson));

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "machines:delete")]
    public async Task<IActionResult> DeleteWorkOrder(int id)
    {
        var result = await _mediator.Send(new DeleteWorkOrderCommand(id));
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }
}

public class CreateWorkOrderRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MachineId { get; set; }
    public int? RequestedByUserId { get; set; }
    public int? AssignedToUserId { get; set; }
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Requested;
    public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Normal;
    public WorkOrderType Type { get; set; } = WorkOrderType.Corrective;
    public Criticality Criticality { get; set; } = Criticality.Medium;
    public decimal EstimatedHours { get; set; }
    public decimal EstimatedCost { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);
    public string? FailureCode { get; set; }
    public string? RootCause { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? ChecklistJson { get; set; }
}

public class UpdateWorkOrderRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? MachineId { get; set; }
    public int? RequestedByUserId { get; set; }
    public int? AssignedToUserId { get; set; }
    public WorkOrderStatus? Status { get; set; }
    public WorkOrderPriority? Priority { get; set; }
    public WorkOrderType? Type { get; set; }
    public Criticality? Criticality { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? EstimatedCost { get; set; }
    public DateTime? DueDate { get; set; }
    public string? FailureCode { get; set; }
    public string? RootCause { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? ChecklistJson { get; set; }
}
