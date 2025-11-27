using KanbanWebAPI.Application.DTOs.Boards;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetByTeamAsync(Guid teamId);
    Task<BoardDto?> GetByIdAsync(Guid boardId);
    Task<BoardDto> CreateAsync(CreateBoardDto dto);
    Task<BoardDto?> UpdateAsync(Guid boardId, UpdateBoardDto dto);
    Task DeleteAsync(Guid boardId);
}
