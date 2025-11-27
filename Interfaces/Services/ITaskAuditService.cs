namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITaskAuditService
{
    Task RecordAsync(Guid taskId, Guid userId, string action, string? details = null);
}
