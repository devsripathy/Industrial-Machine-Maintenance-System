using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Enums;
using SentinelOps.Shared.Utilities;

namespace SentinelOps.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. Seed Permissions
        var permissions = new List<Permission>
        {
            new() { Name = "machines:read", Description = "View machines profiles, hierarchy and configurations" },
            new() { Name = "machines:write", Description = "Create, edit and configure machines" },
            new() { Name = "machines:delete", Description = "Remove machines from inventory" },
            new() { Name = "users:read", Description = "View user profiles and activity" },
            new() { Name = "users:write", Description = "Manage user profiles and roles" },
            new() { Name = "audit_logs:read", Description = "View system audit logs" }
        };

        foreach (var permission in permissions)
        {
            if (!await context.Permissions.AnyAsync(p => p.Name == permission.Name))
            {
                await context.Permissions.AddAsync(permission);
            }
        }
        await context.SaveChangesAsync();

        // 2. Seed Roles and map Permissions
        var allPermissions = await context.Permissions.ToListAsync();
        
        var roles = new List<(string Name, string Description, List<string> PermissionNames)>
        {
            ("Administrator", "Full system administrator", allPermissions.Select(p => p.Name).ToList()),
            ("Maintenance Manager", "Manages scheduling, machines, and technicians", new List<string> { "machines:read", "machines:write" }),
            ("Supervisor", "Oversees shifts and production context", new List<string> { "machines:read" }),
            ("Technician", "Executes work orders and maintenance tasks", new List<string> { "machines:read" }),
            ("Machine Operator", "Monitors active machine execution", new List<string> { "machines:read" }),
            ("Safety Officer", "Audits safety compliance", new List<string> { "machines:read" }),
            ("Inventory Manager", "Manages spare parts and inventory levels", new List<string> { "machines:read" })
        };

        foreach (var roleData in roles)
        {
            var role = await context.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == roleData.Name);
            if (role == null)
            {
                role = new Role
                {
                    Name = roleData.Name,
                    Description = roleData.Description
                };
                
                foreach (var permName in roleData.PermissionNames)
                {
                    var perm = allPermissions.First(p => p.Name == permName);
                    role.Permissions.Add(perm);
                }
                
                await context.Roles.AddAsync(role);
            }
        }
        await context.SaveChangesAsync();

        // 3. Seed Default Admin User
        var adminRole = await context.Roles.FirstAsync(r => r.Name == "Administrator");
        if (!await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            var passwordHash = PasswordHasher.HashPassword("AdminPassword123!");
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@sentinelops.com",
                FullName = "System Administrator",
                PasswordHash = passwordHash,
                PasswordSalt = string.Empty, // Salt handled automatically by BCrypt
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // 4. Seed Machine Types
        var machineTypes = new List<MachineType>
        {
            new() { Code = "CNC", Name = "CNC Milling / Lathe Machine", Description = "Computer Numerical Control machines for precision manufacturing" },
            new() { Code = "ROB", Name = "Industrial Robotic Arm", Description = "Articulated arms for welding, assembly, and pick-and-place" },
            new() { Code = "BRL", Name = "Steam Boiler / Pressure Vessel", Description = "Critical utilities providing thermal energy and steam" }
        };

        foreach (var mt in machineTypes)
        {
            if (!await context.MachineTypes.AnyAsync(x => x.Code == mt.Code))
            {
                await context.MachineTypes.AddAsync(mt);
            }
        }
        await context.SaveChangesAsync();

        // 5. Seed Sample Machines
        var dbCncType = await context.MachineTypes.FirstAsync(x => x.Code == "CNC");
        var dbRobType = await context.MachineTypes.FirstAsync(x => x.Code == "ROB");
        var dbBrlType = await context.MachineTypes.FirstAsync(x => x.Code == "BRL");

        if (!await context.Machines.AnyAsync())
        {
            var machine1 = new Machine
            {
                Name = "Precision CNC Milling Machine 01",
                Code = "CNC-01",
                SerialNumber = "SN-CNC-998811",
                MachineTypeId = dbCncType.Id,
                Criticality = Criticality.High,
                Status = MachineStatus.Running,
                Model = "Siemens SINUMERIK 840D",
                Location = "Factory Floor A - Section 1",
                SpecificationsJson = "{\"MaxRpm\": 12000, \"AxisCount\": 5, \"PowerRatingKw\": 15.0, \"WorkspaceMm\": \"1000x500x500\"}",
                QrCodeData = "sentinelops://machines/CNC-01",
                InstallDate = DateTime.UtcNow.AddYears(-2),
                CreatedAt = DateTime.UtcNow
            };

            var machine2 = new Machine
            {
                Name = "Assembly Robotic Arm 01",
                Code = "ROB-01",
                SerialNumber = "SN-ROB-112233",
                MachineTypeId = dbRobType.Id,
                Criticality = Criticality.High,
                Status = MachineStatus.Running,
                Model = "KUKA KR 60-3",
                Location = "Factory Floor A - Assembly Line 1",
                SpecificationsJson = "{\"PayloadKg\": 60, \"ReachMm\": 2033, \"RepeatabilityMm\": 0.05}",
                QrCodeData = "sentinelops://machines/ROB-01",
                InstallDate = DateTime.UtcNow.AddYears(-1),
                CreatedAt = DateTime.UtcNow
            };

            var machine3 = new Machine
            {
                Name = "Steam Boiler Utility 01",
                Code = "BRL-01",
                SerialNumber = "SN-BRL-554400",
                MachineTypeId = dbBrlType.Id,
                Criticality = Criticality.Critical,
                Status = MachineStatus.Running,
                Model = "Cleaver-Brooks CB-100",
                Location = "Utility Block - Powerhouse",
                SpecificationsJson = "{\"MaxPressurePsi\": 150, \"SteamOutputLbsHr\": 3450, \"FuelType\": \"Natural Gas\"}",
                QrCodeData = "sentinelops://machines/BRL-01",
                InstallDate = DateTime.UtcNow.AddYears(-5),
                CreatedAt = DateTime.UtcNow
            };

            await context.Machines.AddRangeAsync(machine1, machine2, machine3);
            await context.SaveChangesAsync();

            // Set up topological dependency: ROB-01 depends on CNC-01 (parts must be milled before robotic arm picks them)
            var dep1 = new MachineDependency
            {
                MachineId = machine2.Id, // ROB-01
                DependsOnMachineId = machine1.Id, // CNC-01
                DependencyType = "Process Line Sequence"
            };
            await context.MachineDependencies.AddAsync(dep1);
            await context.SaveChangesAsync();
        }
    }
}
