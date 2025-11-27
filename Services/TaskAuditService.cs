using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TaskAuditService : ITaskAuditService
{
    private readonly ITaskAuditRepository _auditRepository;

    public TaskAuditService(ITaskAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task RecordAsync(Guid taskId, Guid userId, string action, string? details = null)
    {
        var entity = new TaskAudit
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            UserId = userId,
            Action = action,
            Details = details,
            CreatedAt = DateTime.UtcNow
        };

        await _auditRepository.AddAsync(entity);
        await _auditRepository.SaveChangesAsync();
    }
}
