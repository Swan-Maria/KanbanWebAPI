using KanbanWebAPI.Application.DTOs.Tasks;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITaskAssignmentService
{
    Task<List<TaskAssigneeDto>> GetAssigneesAsync(Guid taskId);
    Task AssignUsersAsync(Guid taskId, AssignUsersDto dto);
    Task RemoveUserAsync(Guid taskId, Guid userId);
}
