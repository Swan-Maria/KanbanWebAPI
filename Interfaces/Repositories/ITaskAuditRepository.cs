using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface ITaskAuditRepository
{
    Task AddAsync(TaskAudit audit);
    Task<List<TaskAudit>> GetForTaskAsync(Guid taskId);
    Task SaveChangesAsync();
}
