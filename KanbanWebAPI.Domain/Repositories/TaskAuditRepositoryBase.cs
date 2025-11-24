using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface ITaskAuditRepository : IRepositoryBase<TaskAudit>
{
    Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskItemId);
}

internal class TaskAuditRepositoryBase(AppDbContext context) : RepositoryBase<TaskAudit>(context), ITaskAuditRepository
{
    public async Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskItemId)
    {
        return await _context.TaskAudit
            .Where(a => a.TaskItemId == taskItemId)
            .OrderByDescending(a => a.CreateAt)
            .ToListAsync();
    }
}