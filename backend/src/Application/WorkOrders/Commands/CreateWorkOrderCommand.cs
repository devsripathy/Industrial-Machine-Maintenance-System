using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Enums;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.WorkOrders.Commands;

public record CreateWorkOrderCommand(
    string Title,
    string Description,
    int MachineId,
    int? RequestedByUserId,
    int? AssignedToUserId,
    WorkOrderStatus Status,
    WorkOrderPriority Priority,
    WorkOrderType Type,
    Criticality Criticality,
    decimal EstimatedHours,
    decimal EstimatedCost,
    DateTime DueDate,
    string? FailureCode = null,
    string? RootCause = null,
    string? ResolutionNotes = null,
    string? ChecklistJson = null) : IRequest<Result<WorkOrderDto>>;

public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, Result<WorkOrderDto>>
{
    private readonly IMachineRepository _machineRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkOrderCommandHandler(IMachineRepository machineRepository, IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkOrderDto>> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Result<WorkOrderDto>.Failure("Work order title is required.");
        }

        if (request.MachineId <= 0)
        {
            return Result<WorkOrderDto>.Failure("A valid machine id is required.");
        }

        var machine = await _machineRepository.GetByIdAsync(request.MachineId);
        if (machine is null)
        {
            return Result<WorkOrderDto>.Failure("Machine not found.");
        }

        var workOrder = new Domain.Entities.WorkOrder
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            MachineId = request.MachineId,
            RequestedByUserId = request.RequestedByUserId,
            AssignedToUserId = request.AssignedToUserId,
            Status = request.Status,
            Priority = request.Priority,
            Type = request.Type,
            Criticality = request.Criticality,
            EstimatedHours = request.EstimatedHours,
            EstimatedCost = request.EstimatedCost,
            DueDate = request.DueDate,
            FailureCode = request.FailureCode,
            RootCause = request.RootCause,
            ResolutionNotes = request.ResolutionNotes,
            ChecklistJson = request.ChecklistJson,
            CreatedAt = DateTime.UtcNow
        };

        await _workOrderRepository.AddAsync(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var saved = await _workOrderRepository.GetByIdAsync(workOrder.Id);
        if (saved is null)
        {
            return Result<WorkOrderDto>.Failure("Work order could not be loaded after creation.");
        }

        return Result<WorkOrderDto>.Success(MapToDto(saved));
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
