using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface IColumnRepository : IRepositoryBase<Column>
{
    Task<IEnumerable<Column>> GetByBoardIdAsync(Guid boardId);
}

internal class ColumnRepositoryBase(AppDbContext context) : RepositoryBase<Column>(context), IColumnRepository
{
    public async Task<IEnumerable<Column>> GetByBoardIdAsync(Guid boardId)
    {
        return await _context.Columns
            .Include(c => c.Tasks)
            .Where(c => c.BoardId == boardId)
            .ToListAsync();
    }
}