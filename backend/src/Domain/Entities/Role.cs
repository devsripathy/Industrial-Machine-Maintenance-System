using System.Collections.Generic;

namespace SentinelOps.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; // E.g. "Administrator", "Technician"
    public string? Description { get; set; }
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
