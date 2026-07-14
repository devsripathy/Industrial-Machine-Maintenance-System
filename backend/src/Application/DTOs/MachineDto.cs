using System;
using System.Collections.Generic;

namespace SentinelOps.Application.DTOs;

public class MachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    
    public int MachineTypeId { get; set; }
    public string MachineTypeCode { get; set; } = null!;
    public string MachineTypeName { get; set; } = null!;
    
    public string Criticality { get; set; } = null!;
    public int CriticalityValue { get; set; }
    
    public string Status { get; set; } = null!;
    public int StatusValue { get; set; }
    
    public string Model { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? SpecificationsJson { get; set; }
    public string? QrCodeData { get; set; }
    
    public DateTime InstallDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<MachineDependencyDto> Dependencies { get; set; } = new();
}
