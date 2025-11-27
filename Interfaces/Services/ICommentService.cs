using KanbanWebAPI.Application.DTOs.Comments;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ICommentService
{
    Task<List<CommentDto>> GetForTaskAsync(Guid taskId);
    Task<CommentDto> CreateAsync(Guid taskId, Guid userId, CreateCommentDto dto);
    Task DeleteAsync(Guid commentId);
}
