namespace SentinelOps.Application.DTOs;

public class WorkOrderDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? FailureCode { get; set; }
    public string? RootCause { get; set; }
    public string? ResolutionNotes { get; set; }
    public string? ChecklistJson { get; set; }

    public int MachineId { get; set; }
    public string MachineName { get; set; } = string.Empty;
    public string MachineCode { get; set; } = string.Empty;

    public int? RequestedByUserId { get; set; }
    public string? RequestedByUsername { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUsername { get; set; }

    public string Status { get; set; } = string.Empty;
    public int StatusValue { get; set; }
    public string Priority { get; set; } = string.Empty;
    public int PriorityValue { get; set; }
    public string Type { get; set; } = string.Empty;
    public int TypeValue { get; set; }
    public string Criticality { get; set; } = string.Empty;
    public int CriticalityValue { get; set; }

    public decimal EstimatedHours { get; set; }
    public decimal ActualHours { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }

    public DateTime DueDate { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
