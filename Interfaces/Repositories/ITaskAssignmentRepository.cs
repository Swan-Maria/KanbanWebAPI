using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface ITaskAssignmentRepository
{
    Task<List<User>> GetAssigneesAsync(Guid taskId);
    Task AssignUsersAsync(Guid taskId, IEnumerable<Guid> userIds);
    Task RemoveUserAsync(Guid taskId, Guid userId);
    Task<bool> IsUserAssignedAsync(Guid taskId, Guid userId);
    Task<List<TaskItem>> GetUserTasksAsync(Guid userId);
}
