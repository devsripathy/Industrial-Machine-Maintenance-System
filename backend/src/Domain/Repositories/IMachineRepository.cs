using System.Collections.Generic;
using System.Threading.Tasks;
using SentinelOps.Domain.Entities;

namespace SentinelOps.Domain.Repositories;

public interface IMachineRepository
{
    Task<Machine?> GetByIdAsync(int id);
    Task<Machine?> GetByCodeAsync(string code);
    Task<IEnumerable<Machine>> GetAllAsync(string? searchTerm = null, string? typeCode = null, int? minCriticality = null);
    Task AddAsync(Machine machine);
    void Update(Machine machine);
    void Delete(Machine machine);
    Task<bool> ExistsAsync(string code);
    Task<IEnumerable<MachineDependency>> GetDependenciesAsync(int machineId);
    Task AddDependencyAsync(MachineDependency dependency);
    void RemoveDependency(MachineDependency dependency);
}
