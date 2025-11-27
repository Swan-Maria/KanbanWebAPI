using AutoMapper;
using KanbanWebAPI.Application.DTOs.TaskAudits;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Domain.Repositories;

namespace KanbanWebAPI.Application.Services;

public class TaskAuditService(ITaskAuditRepository auditRepository, IMapper mapper) : ITaskAuditService
{
    private readonly ITaskAuditRepository _auditRepository = auditRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TaskAuditDto>> GetByTaskAsync(Guid taskId)
    {
        var audits = await _auditRepository.GetByTaskIdAsync(taskId);
        return _mapper.Map<IEnumerable<TaskAuditDto>>(audits);
    }
}
