

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>>> GetAllAsync();
    Task<Result<CategoryDto?>> GetByIdAsync(Guid id);
    Task<Result<Guid>> CreateAsync(CreateCategoryRequestDto request);
    Task<Result> UpdateAsync(Guid id, CreateCategoryRequestDto request);
    Task<Result> DeleteAsync(Guid id);
}