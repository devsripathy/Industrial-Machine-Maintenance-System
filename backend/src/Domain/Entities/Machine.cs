using System;
using System.Collections.Generic;
using SentinelOps.Domain.Enums;

namespace SentinelOps.Domain.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!; // E.g., CNC-01
    public string SerialNumber { get; set; } = null!;
    public int MachineTypeId { get; set; }
    public virtual MachineType MachineType { get; set; } = null!;
    
    public Criticality Criticality { get; set; }
    public MachineStatus Status { get; set; }
    
    public string Model { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? SpecificationsJson { get; set; } // Custom key-value pairs stored as JSON
    public string? QrCodeData { get; set; }
    
    public DateTime InstallDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Relationships for machine dependency graph
    public virtual ICollection<MachineDependency> ParentDependencies { get; set; } = new List<MachineDependency>();
    public virtual ICollection<MachineDependency> ChildDependencies { get; set; } = new List<MachineDependency>();
}
