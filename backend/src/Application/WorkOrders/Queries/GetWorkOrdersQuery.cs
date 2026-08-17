using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.WorkOrders.Queries;

public record GetWorkOrdersQuery(int? MachineId = null, string? Status = null, string? Search = null) : IRequest<Result<List<WorkOrderDto>>>;

public class GetWorkOrdersQueryHandler : IRequestHandler<GetWorkOrdersQuery, Result<List<WorkOrderDto>>>
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrdersQueryHandler(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<Result<List<WorkOrderDto>>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
    {
        var workOrders = await _workOrderRepository.GetAllAsync(request.MachineId, request.Status, request.Search);
        return Result<List<WorkOrderDto>>.Success(workOrders.Select(MapToDto).ToList());
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
