using SentinelOps.Domain.Entities;

namespace SentinelOps.Domain.Repositories;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(int id);
    Task<IEnumerable<WorkOrder>> GetAllAsync(int? machineId = null, string? status = null, string? search = null);
    Task AddAsync(WorkOrder workOrder);
    void Update(WorkOrder workOrder);
    void Delete(WorkOrder workOrder);
}
