using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface IBoardRepository : IRepositoryBase<Board>
{
    Task<Board?> GetByNameAsync(string boardName);
    Task<IEnumerable<Board>> GetByTeamIdAsync(Guid teamId);
}

internal class BoardRepositoryBase(AppDbContext context) : RepositoryBase<Board>(context), IBoardRepository
{
    public async Task<Board?> GetByNameAsync(string boardName)
    {
        return await _context.Boards
            .FirstOrDefaultAsync(b => b.BoardName == boardName);
    }

    public async Task<IEnumerable<Board>> GetByTeamIdAsync(Guid teamId)
    {
        return await _context.Boards
            .Where(b => b.TeamId == teamId)
            .ToListAsync();
    }
}