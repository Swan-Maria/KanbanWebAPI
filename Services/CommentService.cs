using AutoMapper;
using KanbanWebAPI.Application.DTOs.Comments;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> GetForTaskAsync(Guid taskId)
    {
        var comments = await _commentRepository.GetForTaskAsync(taskId);
        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<CommentDto> CreateAsync(Guid taskId, Guid userId, CreateCommentDto dto)
    {
        var entity = new Comment
        {
            Id = Guid.NewGuid(),
            TaskId = taskId,
            UserId = userId,
            Text = dto.Text,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(entity);
        await _commentRepository.SaveChangesAsync();

        return _mapper.Map<CommentDto>(entity);
    }

    public async Task DeleteAsync(Guid commentId)
    {
        var entity = await _commentRepository.GetByIdAsync(commentId);
        if (entity is null) return;

        await _commentRepository.DeleteAsync(entity);
        await _commentRepository.SaveChangesAsync();
    }
}
