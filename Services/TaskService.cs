using AutoMapper;
using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<TaskDto> CreateAsync(Guid columnId, CreateTaskDto dto)
    {
        var entity = _mapper.Map<TaskItem>(dto);
        entity.Id = Guid.NewGuid();
        entity.ColumnId = columnId;
        entity.CreatedAt = DateTime.UtcNow;

        await _taskRepository.AddAsync(entity);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(entity);
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id)
    {
        var entity = await _taskRepository.GetByIdAsync(id);
        return entity is null ? null : _mapper.Map<TaskDto>(entity);
    }

    public async Task<List<TaskDto>> GetForColumnAsync(Guid columnId)
    {
        var items = await _taskRepository.GetForColumnAsync(columnId);
        return _mapper.Map<List<TaskDto>>(items);
    }

    public async Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto)
    {
        var entity = await _taskRepository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _taskRepository.UpdateAsync(entity);
        await _taskRepository.SaveChangesAsync();

        return _mapper.Map<TaskDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _taskRepository.GetByIdAsync(id);
        if (entity is null) return;

        await _taskRepository.DeleteAsync(entity);
        await _taskRepository.SaveChangesAsync();
    }

    public async Task MoveAsync(Guid id, MoveTaskDto dto)
    {
        var entity = await _taskRepository.GetByIdAsync(id);
        if (entity is null) return;

        entity.ColumnId = dto.ToColumnId;
        await _taskRepository.UpdateAsync(entity);
        await _taskRepository.SaveChangesAsync();
    }
}
