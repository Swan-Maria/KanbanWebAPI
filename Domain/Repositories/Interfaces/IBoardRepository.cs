using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface IBoardRepository : IGenericRepository<Board>
{
    Task<Board?> GetByNameAsync(Guid teamId, string name);
    Task<IEnumerable<Board>> GetByTeamIdAsync(Guid teamId);
}