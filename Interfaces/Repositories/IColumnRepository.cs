using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface IColumnRepository
{
    Task<Column?> GetByIdAsync(Guid id);
    Task<List<Column>> GetForBoardAsync(Guid boardId);
    Task AddAsync(Column column);
    Task UpdateAsync(Column column);
    Task DeleteAsync(Column column);
    Task SaveChangesAsync();
}
