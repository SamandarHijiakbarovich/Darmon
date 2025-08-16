using AutoMapper;
using Darmon.Application.DTOs.CategotyDtos;
using Darmon.Application.Interfaces;
using Darmon.Domain.Entities;
using Darmon.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Services;

public class CategoryService:ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var entity = _mapper.Map<Category>(dto);
        await _categoryRepository.AddAsync(entity);
        await _categoryRepository.SaveChangesAsync();
        return _mapper.Map<CategoryDto>(entity);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;

        _mapper.Map(dto, category);
        await _categoryRepository.UpdateAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var success = await _categoryRepository.DeleteAsync(id);
        await _categoryRepository.SaveChangesAsync();
        return success;
    }
}
