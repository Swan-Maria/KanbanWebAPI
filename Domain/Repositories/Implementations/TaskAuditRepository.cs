using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class TaskAuditRepository : GenericRepository<TaskAudit>, ITaskAuditRepository
{
    public TaskAuditRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskId)
    {
        return await _context.TaskAudits
            .Where(a => a.TaskId == taskId)
            .OrderByDescending(a => a.ChangedAt)
            .ToListAsync();
    }
}