namespace SentinelOps.Domain.Entities;

public class MachineDependency
{
    public int MachineId { get; set; }
    public virtual Machine Machine { get; set; } = null!;
    
    public int DependsOnMachineId { get; set; }
    public virtual Machine DependsOnMachine { get; set; } = null!;
    
    public string? DependencyType { get; set; } // E.g., Electrical, Hydraulic, Process
}
