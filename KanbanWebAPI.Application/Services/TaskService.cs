using AutoMapper;
using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Domain.Repositories;
using KanbanWebAPI.Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TaskService(ITaskItemRepository taskRepository, IMapper mapper) : ITaskService
{
    private readonly ITaskItemRepository _taskRepository = taskRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TaskDto>> GetByColumnAsync(Guid columnId)
    {
        var tasks = await _taskRepository.GetByColumnIdAsync(columnId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto?> GetByIdAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        return task is null ? null : _mapper.Map<TaskDto>(task);
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
    {
        var entity = _mapper.Map<TaskItem>(dto);
        await _taskRepository.CreateAsync(entity);
        return _mapper.Map<TaskDto>(entity);
    }

    public async Task<TaskDto?> UpdateAsync(Guid taskId, UpdateTaskDto dto)
    {
        var entity = await _taskRepository.GetByIdAsync(taskId);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _taskRepository.UpdateAsync(entity);
        return _mapper.Map<TaskDto>(entity);
    }

    public async Task DeleteAsync(Guid taskId)
    {
        await _taskRepository.DeleteByIdAsync(taskId);
    }
}
