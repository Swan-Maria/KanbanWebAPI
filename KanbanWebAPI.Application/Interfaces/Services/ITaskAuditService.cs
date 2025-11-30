using KanbanWebAPI.Application.DTOs.TaskAudits;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITaskAuditService
{
    Task<IEnumerable<TaskAuditDto>> GetByTaskAsync(Guid taskId);
}