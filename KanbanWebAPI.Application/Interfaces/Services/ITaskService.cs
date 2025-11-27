using KanbanWebAPI.Application.DTOs.Tasks;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetByColumnAsync(Guid columnId);
    Task<TaskDto?> GetByIdAsync(Guid taskId);
    Task<TaskDto> CreateAsync(CreateTaskDto dto);
    Task<TaskDto?> UpdateAsync(Guid taskId, UpdateTaskDto dto);
    Task DeleteAsync(Guid taskId);
}
