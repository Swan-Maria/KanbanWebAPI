using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ColumnsController : ControllerBase
{
    private readonly IColumnService _columnService;

    public ColumnsController(IColumnService columnService)
    {
        _columnService = columnService;
    }

    [HttpGet("board/{boardId:guid}")]
    public async Task<ActionResult<IEnumerable<ColumnDto>>> GetByBoard(Guid boardId)
    {
        var columns = await _columnService.GetByBoardAsync(boardId);
        return Ok(columns);
    }

    [HttpPost]
    public async Task<ActionResult<ColumnDto>> Create([FromBody] CreateColumnDto dto)
    {
        var column = await _columnService.CreateAsync(dto);
        return Ok(column);
    }

    [HttpPut("{columnId:guid}")]
    public async Task<ActionResult<ColumnDto>> Update(Guid columnId, [FromBody] UpdateColumnDto dto)
    {
        var updated = await _columnService.UpdateAsync(columnId, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{columnId:guid}")]
    public async Task<IActionResult> Delete(Guid columnId)
    {
        await _columnService.DeleteAsync(columnId);
        return NoContent();
    }
}
