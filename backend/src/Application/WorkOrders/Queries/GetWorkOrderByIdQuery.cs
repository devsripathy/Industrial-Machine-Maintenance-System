using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.WorkOrders.Queries;

public record GetWorkOrderByIdQuery(int Id) : IRequest<Result<WorkOrderDto>>;

public class GetWorkOrderByIdQueryHandler : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrderDto>>
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrderByIdQueryHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<Result<WorkOrderDto>> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return Result<WorkOrderDto>.Failure("Work order id must be greater than zero.");
        }

        var workOrder = await _workOrderRepository.GetByIdAsync(request.Id);
        if (workOrder is null)
        {
            return Result<WorkOrderDto>.Failure("Work order not found.");
        }

        return Result<WorkOrderDto>.Success(MapToDto(workOrder));
    }

    private static WorkOrderDto MapToDto(Domain.Entities.WorkOrder workOrder)
    {
        return new WorkOrderDto
        {
            Id = workOrder.Id,
            Title = workOrder.Title,
            Description = workOrder.Description,
            FailureCode = workOrder.FailureCode,
            RootCause = workOrder.RootCause,
            ResolutionNotes = workOrder.ResolutionNotes,
            ChecklistJson = workOrder.ChecklistJson,
            MachineId = workOrder.MachineId,
            MachineName = workOrder.Machine?.Name ?? string.Empty,
            MachineCode = workOrder.Machine?.Code ?? string.Empty,
            RequestedByUserId = workOrder.RequestedByUserId,
            RequestedByUsername = workOrder.RequestedByUser?.Username,
            AssignedToUserId = workOrder.AssignedToUserId,
            AssignedToUsername = workOrder.AssignedToUser?.Username,
            Status = workOrder.Status.ToString(),
            StatusValue = (int)workOrder.Status,
            Priority = workOrder.Priority.ToString(),
            PriorityValue = (int)workOrder.Priority,
            Type = workOrder.Type.ToString(),
            TypeValue = (int)workOrder.Type,
            Criticality = workOrder.Criticality.ToString(),
            CriticalityValue = (int)workOrder.Criticality,
            EstimatedHours = workOrder.EstimatedHours,
            ActualHours = workOrder.ActualHours,
            EstimatedCost = workOrder.EstimatedCost,
            ActualCost = workOrder.ActualCost,
            DueDate = workOrder.DueDate,
            StartedAt = workOrder.StartedAt,
            CompletedAt = workOrder.CompletedAt,
            CreatedAt = workOrder.CreatedAt,
            UpdatedAt = workOrder.UpdatedAt
        };
    }
}
