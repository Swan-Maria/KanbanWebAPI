using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface ITaskAuditRepository : IGenericRepository<TaskAudit>
{
    Task<IEnumerable<TaskAudit>> GetByTaskIdAsync(Guid taskId);
}