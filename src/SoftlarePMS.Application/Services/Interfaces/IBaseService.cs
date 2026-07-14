using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftlarePMS.Application.Services.Interfaces
{
    public interface IBaseService<TEntity, TResponseDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        Task<IEnumerable<TResponseDto>> GetAllAsync();
        Task<TResponseDto> GetByIdAsync(Guid id);
        Task<TResponseDto> CreateAsync(TCreateDto createDto);
        Task UpdateAsync(Guid id, TUpdateDto updateDto);
        Task DeleteAsync(Guid id);
    }
}