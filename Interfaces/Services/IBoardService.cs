using KanbanWebAPI.Application.DTOs.Boards;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IBoardService
{
    Task<List<BoardDto>> GetForTeamAsync(Guid teamId);
    Task<BoardDto?> GetByIdAsync(Guid boardId);
    Task<BoardDto> CreateAsync(CreateBoardDto dto);
    Task<BoardDto?> UpdateAsync(Guid id, UpdateBoardDto dto);
    Task DeleteAsync(Guid id);
}
