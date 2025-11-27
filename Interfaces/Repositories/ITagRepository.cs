using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task<List<Tag>> GetForBoardAsync(Guid boardId);
    Task AddAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Tag tag);
    Task SaveChangesAsync();
}
