


using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto(category.Id,category.Name,category.Description);
    }

    public static List<CategoryDto> ToCategoryDtoList(this List<Category> category)
    {
        return category.Select(x=>x.ToCategoryDto()).ToList();
    }

}