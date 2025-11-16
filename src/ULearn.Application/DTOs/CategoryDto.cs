

namespace ULearn.Application.DTOs;

public record CategoryDto(Guid Id, string Name, string? Description);
public record CreateCategoryRequestDto(string Name, string? Description);