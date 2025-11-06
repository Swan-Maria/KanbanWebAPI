using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class ColumnRepository : GenericRepository<Column>, IColumnRepository
{
    public ColumnRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Column>> GetByBoardIdAsync(Guid boardId)
    {
        return await _context.Columns
            .Include(c => c.Tasks)
            .Where(c => c.BoardId == boardId)
            .ToListAsync();
    }
}