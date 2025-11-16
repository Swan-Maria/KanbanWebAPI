using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface ITaskAuditRepository : IRepositoryBase<TaskAudit>
{
    Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskId);
}

internal class TaskAuditRepositoryBase(AppDbContext context) : RepositoryBase<TaskAudit>(context), ITaskAuditRepository
{
    public async Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskId)
    {
        return await _context.TaskAudits
            .Where(a => a.TaskId == taskId)
            .OrderByDescending(a => a.ChangedAt)
            .ToListAsync();
    }
}