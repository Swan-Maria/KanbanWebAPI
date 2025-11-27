using AutoMapper;
using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Domain.Repositories;
using KanbanWebAPI.Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class ColumnService(IColumnRepository columnRepository, IMapper mapper) : IColumnService
{
    private readonly IColumnRepository _columnRepository = columnRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ColumnDto>> GetByBoardAsync(Guid boardId)
    {
        var columns = await _columnRepository.GetByBoardIdAsync(boardId);
        return _mapper.Map<IEnumerable<ColumnDto>>(columns);
    }

    public async Task<ColumnDto> CreateAsync(CreateColumnDto dto)
    {
        var entity = _mapper.Map<Column>(dto);
        await _columnRepository.CreateAsync(entity);
        return _mapper.Map<ColumnDto>(entity);
    }

    public async Task<ColumnDto?> UpdateAsync(Guid columnId, UpdateColumnDto dto)
    {
        var entity = await _columnRepository.GetByIdAsync(columnId);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _columnRepository.UpdateAsync(entity);
        return _mapper.Map<ColumnDto>(entity);
    }

    public async Task DeleteAsync(Guid columnId)
    {
        await _columnRepository.DeleteByIdAsync(columnId);
    }
}
