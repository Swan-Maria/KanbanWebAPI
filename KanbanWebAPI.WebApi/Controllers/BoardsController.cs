using KanbanWebAPI.Application.DTOs.Boards;
using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;
    private readonly IColumnService _columnService;
    private readonly ITaskService _taskService;

    public BoardsController(
        IBoardService boardService,
        IColumnService columnService,
        ITaskService taskService)
    {
        _boardService = boardService;
        _columnService = columnService;
        _taskService = taskService;
    }

    // DTO для повної дошки
    public class ColumnWithTasksDto
    {
        public Guid ColumnId { get; set; }
        public string ColumnName { get; set; } = null!;
        public List<TaskDto> Tasks { get; set; } = new();
    }

    public class BoardFullDto
    {
        public Guid BoardId { get; set; }
        public string BoardName { get; set; } = null!;
        public string? BoardDescription { get; set; }
        public Guid TeamId { get; set; }
        public List<ColumnWithTasksDto> Columns { get; set; } = new();
    }

    // --- CRUD дошок ---

    [HttpGet("team/{teamId:guid}")]
    public async Task<ActionResult<IEnumerable<BoardDto>>> GetByTeam(Guid teamId)
    {
        var boards = await _boardService.GetByTeamAsync(teamId);
        return Ok(boards);
    }

    [HttpGet("{boardId:guid}")]
    public async Task<ActionResult<BoardDto>> GetById(Guid boardId)
    {
        var board = await _boardService.GetByIdAsync(boardId);
        if (board is null)
            return NotFound();

        return Ok(board);
    }

    [HttpPost]
    public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardDto dto)
    {
        var board = await _boardService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { boardId = board.BoardId }, board);
    }

    [HttpPut("{boardId:guid}")]
    public async Task<ActionResult<BoardDto>> Update(Guid boardId, [FromBody] UpdateBoardDto dto)
    {
        var updated = await _boardService.UpdateAsync(boardId, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{boardId:guid}")]
    public async Task<IActionResult> Delete(Guid boardId)
    {
        await _boardService.DeleteAsync(boardId);
        return NoContent();
    }

    // --- FULL VIEW дошки ---

    [HttpGet("{boardId:guid}/full")]
    public async Task<ActionResult<BoardFullDto>> GetBoardFull(Guid boardId)
    {
        var board = await _boardService.GetByIdAsync(boardId);
        if (board is null)
            return NotFound();

        var columns = await _columnService.GetByBoardAsync(boardId);

        var result = new BoardFullDto
        {
            BoardId = board.BoardId,
            BoardName = board.BoardName,
            BoardDescription = board.BoardDescription,
            TeamId = board.TeamId
        };

        foreach (var column in columns)
        {
            var tasks = await _taskService.GetByColumnAsync(column.ColumnId);

            result.Columns.Add(new ColumnWithTasksDto
            {
                ColumnId = column.ColumnId,
                ColumnName = column.ColumnName,
                Tasks = tasks.ToList()
            });
        }

        return Ok(result);
    }
}
