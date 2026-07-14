using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SoftlarePMS.Application.Services.Interfaces;
using SoftlarePMS.Domain.Exceptions; 
using SoftlarePMS.Domain.Repositories;
using SoftlarePMS.Domain.UnitOfWork;

namespace SoftlarePMS.Application.Services.Implementations
{
    public class BaseService<TEntity, TResponseDto, TCreateDto, TUpdateDto>
        : IBaseService<TEntity, TResponseDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TResponseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TResponseDto>>(entities);
        }

        public virtual async Task<TResponseDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} not found.", id);

            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task<TResponseDto> CreateAsync(TCreateDto createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TUpdateDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} not found.", id);

            _mapper.Map(updateDto, entity);

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} not found.", id);

            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}