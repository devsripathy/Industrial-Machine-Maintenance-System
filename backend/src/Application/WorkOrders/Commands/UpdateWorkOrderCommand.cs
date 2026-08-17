using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Enums;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.WorkOrders.Commands;

public record UpdateWorkOrderCommand(
    int Id,
    string? Title,
    string? Description,
    int? MachineId,
    int? RequestedByUserId,
    int? AssignedToUserId,
    WorkOrderStatus? Status,
    WorkOrderPriority? Priority,
    WorkOrderType? Type,
    Criticality? Criticality,
    decimal? EstimatedHours,
    decimal? EstimatedCost,
    DateTime? DueDate,
    string? FailureCode = null,
    string? RootCause = null,
    string? ResolutionNotes = null,
    string? ChecklistJson = null) : IRequest<Result<WorkOrderDto>>;

public class UpdateWorkOrderCommandHandler : IRequestHandler<UpdateWorkOrderCommand, Result<WorkOrderDto>>
{
    private readonly IMachineRepository _machineRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkOrderCommandHandler(IMachineRepository machineRepository, IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkOrderDto>> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
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

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            workOrder.Title = request.Title.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            workOrder.Description = request.Description.Trim();
        }

        if (request.MachineId.HasValue)
        {
            var machine = await _machineRepository.GetByIdAsync(request.MachineId.Value);
            if (machine is null)
            {
                return Result<WorkOrderDto>.Failure("Machine not found.");
            }

            workOrder.MachineId = request.MachineId.Value;
        }

        if (request.RequestedByUserId.HasValue)
        {
            workOrder.RequestedByUserId = request.RequestedByUserId.Value;
        }

        if (request.AssignedToUserId.HasValue)
        {
            workOrder.AssignedToUserId = request.AssignedToUserId.Value;
        }

        if (request.Status.HasValue)
        {
            workOrder.Status = request.Status.Value;
        }

        if (request.Priority.HasValue)
        {
            workOrder.Priority = request.Priority.Value;
        }

        if (request.Type.HasValue)
        {
            workOrder.Type = request.Type.Value;
        }

        if (request.Criticality.HasValue)
        {
            workOrder.Criticality = request.Criticality.Value;
        }

        if (request.EstimatedHours.HasValue)
        {
            workOrder.EstimatedHours = request.EstimatedHours.Value;
        }

        if (request.EstimatedCost.HasValue)
        {
            workOrder.EstimatedCost = request.EstimatedCost.Value;
        }

        if (request.DueDate.HasValue)
        {
            workOrder.DueDate = request.DueDate.Value;
        }

        if (request.FailureCode != null)
        {
            workOrder.FailureCode = request.FailureCode;
        }

        if (request.RootCause != null)
        {
            workOrder.RootCause = request.RootCause;
        }

        if (request.ResolutionNotes != null)
        {
            workOrder.ResolutionNotes = request.ResolutionNotes;
        }

        if (request.ChecklistJson != null)
        {
            workOrder.ChecklistJson = request.ChecklistJson;
        }

        workOrder.UpdatedAt = DateTime.UtcNow;
        _workOrderRepository.Update(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
