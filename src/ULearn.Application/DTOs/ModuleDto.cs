

namespace ULearn.Application.DTOs;


public record ModuleDto(
    Guid Id,
    string Title,
    string? Description,
    int OrderIndex,
    CourseDto? Course,
    List<LessonDto>? Lessons  // Optional include
);
public record CreateModuleRequestDto(
    string Title,
    int OrderIndex,
    string? Description
);
