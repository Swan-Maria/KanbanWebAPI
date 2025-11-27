using AutoMapper;
using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;

namespace KanbanWebAPI.Application.Services;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    public TaskAssignmentService(ITaskAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskAssigneeDto>> GetAssigneesAsync(Guid taskId)
    {
        var users = await _assignmentRepository.GetAssigneesAsync(taskId);
        return users
            .Select(u => new TaskAssigneeDto
            {
                UserId = u.Id,
                User = _mapper.Map<DTOs.Users.UserDto>(u)
            })
            .ToList();
    }

    public async Task AssignUsersAsync(Guid taskId, AssignUsersDto dto)
    {
        await _assignmentRepository.AssignUsersAsync(taskId, dto.UserIds);
    }

    public async Task RemoveUserAsync(Guid taskId, Guid userId)
    {
        await _assignmentRepository.RemoveUserAsync(taskId, userId);
    }
}
