using KanbanWebAPI.Application.DTOs.Tasks;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITaskService
{
    Task<TaskDto> CreateAsync(Guid columnId, CreateTaskDto dto);
    Task<TaskDto?> GetByIdAsync(Guid id);
    Task<List<TaskDto>> GetForColumnAsync(Guid columnId);
    Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto);
    Task DeleteAsync(Guid id);
    Task MoveAsync(Guid id, MoveTaskDto dto);
}
