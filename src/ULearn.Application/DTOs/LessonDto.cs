

using ULearn.Application.Interfaces;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;

namespace ULearn.Application.DTOs;

public record LessonDto(
    Guid Id,
    string Title,
    LessonContentTypeEnum ContentType,
    string? ContentUrl,
    string? ContentText,
    int OrderIndex,
    bool IsPreview,
    QuizDto? Quiz,
    Assignment? Assignment
);

public record CreateLessonRequestDto(
    string Title,
    LessonContentTypeEnum ContentType,
    int OrderIndex,
    string? ContentUrl,
    string? ContentText,
    int? DurationMinutes,
    bool IsPreview
):IValidateRequest;