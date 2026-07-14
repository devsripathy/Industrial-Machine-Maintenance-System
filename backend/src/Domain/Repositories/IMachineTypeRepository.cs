using System.Collections.Generic;
using System.Threading.Tasks;
using SentinelOps.Domain.Entities;

namespace SentinelOps.Domain.Repositories;

public interface IMachineTypeRepository
{
    Task<MachineType?> GetByIdAsync(int id);
    Task<MachineType?> GetByCodeAsync(string code);
    Task<IEnumerable<MachineType>> GetAllAsync();
    Task AddAsync(MachineType machineType);
    void Delete(MachineType machineType);
}
