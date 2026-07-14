using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Repositories;

namespace SentinelOps.Infrastructure.Persistence.Repositories;

public class MachineTypeRepository : IMachineTypeRepository
{
    private readonly ApplicationDbContext _context;

    public MachineTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MachineType?> GetByIdAsync(int id)
    {
        return await _context.MachineTypes.FindAsync(id);
    }

    public async Task<MachineType?> GetByCodeAsync(string code)
    {
        return await _context.MachineTypes.FirstOrDefaultAsync(mt => mt.Code == code);
    }

    public async Task<IEnumerable<MachineType>> GetAllAsync()
    {
        return await _context.MachineTypes.ToListAsync();
    }

    public async Task AddAsync(MachineType machineType)
    {
        await _context.MachineTypes.AddAsync(machineType);
    }

    public void Delete(MachineType machineType)
    {
        _context.MachineTypes.Remove(machineType);
    }
}
