using AutoMapper;
using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class ColumnService : IColumnService
{
    private readonly IColumnRepository _columnRepository;
    private readonly IMapper _mapper;

    public ColumnService(IColumnRepository columnRepository, IMapper mapper)
    {
        _columnRepository = columnRepository;
        _mapper = mapper;
    }

    public async Task<List<ColumnDto>> GetForBoardAsync(Guid boardId)
    {
        var cols = await _columnRepository.GetForBoardAsync(boardId);
        return _mapper.Map<List<ColumnDto>>(cols);
    }

    public async Task<ColumnDto> CreateAsync(Guid boardId, CreateColumnDto dto)
    {
        var entity = _mapper.Map<Column>(dto);
        entity.Id = Guid.NewGuid();
        entity.BoardId = boardId;

        await _columnRepository.AddAsync(entity);
        await _columnRepository.SaveChangesAsync();

        return _mapper.Map<ColumnDto>(entity);
    }

    public async Task<ColumnDto?> UpdateAsync(Guid id, UpdateColumnDto dto)
    {
        var entity = await _columnRepository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _columnRepository.UpdateAsync(entity);
        await _columnRepository.SaveChangesAsync();

        return _mapper.Map<ColumnDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _columnRepository.GetByIdAsync(id);
        if (entity is null) return;

        await _columnRepository.DeleteAsync(entity);
        await _columnRepository.SaveChangesAsync();
    }
}
