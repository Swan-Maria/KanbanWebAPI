using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("column/{columnId:guid}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetByColumn(Guid columnId)
    {
        var tasks = await _taskService.GetByColumnAsync(columnId);
        return Ok(tasks);
    }

    [HttpGet("{taskId:guid}")]
    public async Task<ActionResult<TaskDto>> GetById(Guid taskId)
    {
        var task = await _taskService.GetByIdAsync(taskId);
        if (task is null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
    {
        var task = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { taskId = task.TaskId }, task);
    }

    [HttpPut("{taskId:guid}")]
    public async Task<ActionResult<TaskDto>> Update(Guid taskId, [FromBody] UpdateTaskDto dto)
    {
        var updated = await _taskService.UpdateAsync(taskId, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> Delete(Guid taskId)
    {
        await _taskService.DeleteAsync(taskId);
        return NoContent();
    }
}