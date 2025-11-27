using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(Guid id);
    Task<List<Comment>> GetForTaskAsync(Guid taskId);
    Task AddAsync(Comment comment);
    Task DeleteAsync(Comment comment);
    Task SaveChangesAsync();
}
