using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<Board?> GetByIdAsync(Guid id);
    Task<List<Board>> GetForTeamAsync(Guid teamId);
    Task AddAsync(Board board);
    Task UpdateAsync(Board board);
    Task DeleteAsync(Board board);
    Task SaveChangesAsync();
}
