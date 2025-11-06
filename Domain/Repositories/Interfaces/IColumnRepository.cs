using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface IColumnRepository : IGenericRepository<Column>
{
    Task<IEnumerable<Column>> GetByBoardIdAsync(Guid boardId);
}