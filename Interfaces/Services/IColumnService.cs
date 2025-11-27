using KanbanWebAPI.Application.DTOs.Columns;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IColumnService
{
    Task<List<ColumnDto>> GetForBoardAsync(Guid boardId);
    Task<ColumnDto> CreateAsync(Guid boardId, CreateColumnDto dto);
    Task<ColumnDto?> UpdateAsync(Guid id, UpdateColumnDto dto);
    Task DeleteAsync(Guid id);
}
