using System.Collections.Generic;

namespace SentinelOps.Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; // E.g. "machines:read", "machines:write"
    public string? Description { get; set; }
    
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
