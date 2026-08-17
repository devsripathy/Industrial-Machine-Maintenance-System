using SentinelOps.Domain.Enums;

namespace SentinelOps.Domain.Entities;

public class WorkOrder
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? FailureCode { get; set; }
    public string? RootCause { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? ChecklistJson { get; set; }

    public int MachineId { get; set; }
    public virtual Machine Machine { get; set; } = null!;

    public int? RequestedByUserId { get; set; }
    public virtual User? RequestedByUser { get; set; }

    public int? AssignedToUserId { get; set; }
    public virtual User? AssignedToUser { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Requested;
    public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Normal;
    public WorkOrderType Type { get; set; } = WorkOrderType.Corrective;
    public Criticality Criticality { get; set; } = Criticality.Medium;

    public decimal EstimatedHours { get; set; }
    public decimal ActualHours { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }

    public DateTime DueDate { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
