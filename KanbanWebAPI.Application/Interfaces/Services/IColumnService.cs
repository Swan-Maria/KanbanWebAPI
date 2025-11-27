using KanbanWebAPI.Application.DTOs.Columns;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IColumnService
{
    Task<IEnumerable<ColumnDto>> GetByBoardAsync(Guid boardId);
    Task<ColumnDto> CreateAsync(CreateColumnDto dto);
    Task<ColumnDto?> UpdateAsync(Guid columnId, UpdateColumnDto dto);
    Task DeleteAsync(Guid columnId);
}
