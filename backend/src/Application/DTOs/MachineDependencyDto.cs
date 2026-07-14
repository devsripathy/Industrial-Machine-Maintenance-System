namespace SentinelOps.Application.DTOs;

public class MachineDependencyDto
{
    public int DependsOnMachineId { get; set; }
    public string DependsOnMachineName { get; set; } = null!;
    public string DependsOnMachineCode { get; set; } = null!;
    public string? DependencyType { get; set; }
}
