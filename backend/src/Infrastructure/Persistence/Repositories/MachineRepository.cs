using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Repositories;

namespace SentinelOps.Infrastructure.Persistence.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly ApplicationDbContext _context;

    public MachineRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Machine?> GetByIdAsync(int id)
    {
        return await _context.Machines
            .Include(m => m.MachineType)
            .Include(m => m.ParentDependencies)
                .ThenInclude(d => d.DependsOnMachine)
            .Include(m => m.ChildDependencies)
                .ThenInclude(d => d.Machine)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Machine?> GetByCodeAsync(string code)
    {
        return await _context.Machines
            .Include(m => m.MachineType)
            .FirstOrDefaultAsync(m => m.Code == code);
    }

    public async Task<IEnumerable<Machine>> GetAllAsync(string? searchTerm = null, string? typeCode = null, int? minCriticality = null)
    {
        var query = _context.Machines
            .Include(m => m.MachineType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(m => m.Name.Contains(searchTerm) || 
                                     m.Code.Contains(searchTerm) || 
                                     m.SerialNumber.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(typeCode))
        {
            query = query.Where(m => m.MachineType.Code == typeCode);
        }

        if (minCriticality.HasValue)
        {
            query = query.Where(m => (int)m.Criticality >= minCriticality.Value);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(Machine machine)
    {
        await _context.Machines.AddAsync(machine);
    }

    public void Update(Machine machine)
    {
        _context.Machines.Update(machine);
    }

    public void Delete(Machine machine)
    {
        _context.Machines.Remove(machine);
    }

    public async Task<bool> ExistsAsync(string code)
    {
        return await _context.Machines.AnyAsync(m => m.Code == code);
    }

    public async Task<IEnumerable<MachineDependency>> GetDependenciesAsync(int machineId)
    {
        return await _context.MachineDependencies
            .Include(d => d.Machine)
            .Include(d => d.DependsOnMachine)
            .Where(d => d.MachineId == machineId)
            .ToListAsync();
    }

    public async Task AddDependencyAsync(MachineDependency dependency)
    {
        await _context.MachineDependencies.AddAsync(dependency);
    }

    public void RemoveDependency(MachineDependency dependency)
    {
        _context.MachineDependencies.Remove(dependency);
    }
}
