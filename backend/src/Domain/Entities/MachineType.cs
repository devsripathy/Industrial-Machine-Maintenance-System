using System.Collections.Generic;

namespace SentinelOps.Domain.Entities;

public class MachineType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!; // E.g., CNC, ROBOT, BOILER
    public string? Description { get; set; }
    
    // Navigation property
    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
}
