using AutoMapper;
using KanbanWebAPI.Application.DTOs.Tags;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository tagRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> GetForBoardAsync(Guid boardId)
    {
        var tags = await _tagRepository.GetForBoardAsync(boardId);
        return _mapper.Map<List<TagDto>>(tags);
    }

    public async Task<TagDto> CreateAsync(Guid boardId, CreateTagDto dto)
    {
        var entity = _mapper.Map<Tag>(dto);
        entity.Id = Guid.NewGuid();
        entity.BoardId = boardId;

        await _tagRepository.AddAsync(entity);
        await _tagRepository.SaveChangesAsync();

        return _mapper.Map<TagDto>(entity);
    }

    public async Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto)
    {
        var entity = await _tagRepository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _tagRepository.UpdateAsync(entity);
        await _tagRepository.SaveChangesAsync();

        return _mapper.Map<TagDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _tagRepository.GetByIdAsync(id);
        if (entity is null) return;

        await _tagRepository.DeleteAsync(entity);
        await _tagRepository.SaveChangesAsync();
    }
}
