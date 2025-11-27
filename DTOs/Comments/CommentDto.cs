using KanbanWebAPI.Application.DTOs.Users;

namespace KanbanWebAPI.Application.DTOs.Comments;

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public UserDto User { get; set; } = default!;
}
