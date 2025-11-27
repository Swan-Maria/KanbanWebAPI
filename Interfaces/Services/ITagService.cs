using KanbanWebAPI.Application.DTOs.Tags;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITagService
{
    Task<List<TagDto>> GetForBoardAsync(Guid boardId);
    Task<TagDto> CreateAsync(Guid boardId, CreateTagDto dto);
    Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto);
    Task DeleteAsync(Guid id);
}
