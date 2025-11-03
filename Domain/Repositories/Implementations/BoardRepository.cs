using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class BoardRepository : GenericRepository<Board>, IBoardRepository
{
    public BoardRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Board>> GetByTeamIdAsync(Guid teamId)
    {
        return await _context.Boards
            .Include(b => b.Columns)
            .Where(b => b.TeamId == teamId)
            .ToListAsync();
    }

    public async Task<Board?> GetByNameAsync(Guid teamId, string name)
    {
        return await _context.Boards
            .FirstOrDefaultAsync(b => b.TeamId == teamId && b.BoardName == name);
    }
}