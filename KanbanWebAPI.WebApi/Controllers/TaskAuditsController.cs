using KanbanWebAPI.Application.DTOs.TaskAudits;
using KanbanWebAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TaskAuditsController : ControllerBase
{
    private readonly ITaskAuditService _auditService;

    public TaskAuditsController(ITaskAuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<ActionResult<IEnumerable<TaskAuditDto>>> GetByTask(Guid taskId)
    {
        var audits = await _auditService.GetByTaskAsync(taskId);
        return Ok(audits);
    }
}
