

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;


public interface ILessonService
{
    Task<Result<List<LessonDto>>> GetLessonsByModuleOrderedAsync(Guid courseId,Guid moduleId,Guid userId);
    Task<Result<LessonDto?>> GetLessonWithContentAsync(Guid lessonId);
    Task<Result<Guid>> AddLessonAsync(Guid moduleId, CreateLessonRequestDto dto, Guid instructorId);
    Task<Result> UpdateLessonAsync(Guid lessonId, CreateLessonRequestDto dto, Guid instructorId);
}