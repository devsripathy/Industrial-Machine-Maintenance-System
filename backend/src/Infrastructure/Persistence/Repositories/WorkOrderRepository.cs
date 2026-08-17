using Microsoft.EntityFrameworkCore;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Repositories;

namespace SentinelOps.Infrastructure.Persistence.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly ApplicationDbContext _context;

    public WorkOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder?> GetByIdAsync(int id)
    {
        return await _context.WorkOrders
            .Include(w => w.Machine)
            .Include(w => w.RequestedByUser)
            .Include(w => w.AssignedToUser)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<WorkOrder>> GetAllAsync(int? machineId = null, string? status = null, string? search = null)
    {
        var query = _context.WorkOrders
            .Include(w => w.Machine)
            .Include(w => w.RequestedByUser)
            .Include(w => w.AssignedToUser)
            .AsQueryable();

        if (machineId.HasValue)
        {
            query = query.Where(w => w.MachineId == machineId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var statusText = status.Trim();
            query = query.Where(w => w.Status.ToString() == statusText || w.Status.ToString().Contains(statusText));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(w =>
                w.Title.Contains(term) ||
                w.Description.Contains(term) ||
                (w.FailureCode != null && w.FailureCode.Contains(term)) ||
                w.Machine.Name.Contains(term) ||
                w.Machine.Code.Contains(term));
        }

        return await query.OrderByDescending(w => w.CreatedAt).ToListAsync();
    }

    public async Task AddAsync(WorkOrder workOrder)
    {
        await _context.WorkOrders.AddAsync(workOrder);
    }

    public void Update(WorkOrder workOrder)
    {
        _context.WorkOrders.Update(workOrder);
    }

    public void Delete(WorkOrder workOrder)
    {
        _context.WorkOrders.Remove(workOrder);
    }
}
