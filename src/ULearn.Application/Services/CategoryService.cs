


using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class CategoryService : ICategoryService
{

    private readonly IUnitOfWork _uow;

    public CategoryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<Guid>> CreateAsync(CreateCategoryRequestDto request)
    {
        var exists = await _uow.Repository<Category>().ListAsync(c => c.Name.Trim().ToLower() == request.Name.Trim().ToLower());


        if (exists.Count != 0)
            return Result.Failure<Guid>(Domain.Enums.ErrorCodeEnum.BadRequest, "Category name already exists.");

        var category = new Category
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
        };

        await _uow.Repository<Category>().AddAsync(category);
        await _uow.SaveChangesAsync();

        return Result.Success(category.Id);
    }

    public Task<Result> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _uow.Repository<Category>().ListAsync();
        var mapped = categories.ToCategoryDtoList();
        return Result.Success(mapped);
    }

    public async Task<Result<CategoryDto?>> GetByIdAsync(Guid id)
    {
        var category = await _uow.Repository<Category>().GetByIdAsync(id);
        var mapped = category?.ToCategoryDto();
        return Result.Success(mapped);
    }

    public async Task<Result> UpdateAsync(Guid id, CreateCategoryRequestDto request)
    {
        var category = await _uow.Repository<Category>().GetByIdAsync(id);

        if (category == null)
            return Result.Failure<Result>(Domain.Enums.ErrorCodeEnum.BadRequest, "Category not found");


        var duplicate = await _uow.Repository<Category>()
            .ListAsync(c => c.Name.Trim().Equals(request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase) && c.Id != id);

        if (duplicate.Count != 0)
            return Result.Failure<Guid>(Domain.Enums.ErrorCodeEnum.BadRequest, "Category name already exists.");

        category.Name = request.Name.Trim();
        category.Description = request.Description?.Trim() ?? string.Empty;

        _uow.Repository<Category>().Update(category);
        await _uow.SaveChangesAsync();

        return Result.Success();
    }
}