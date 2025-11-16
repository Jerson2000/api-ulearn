


using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Domain.Shared;
namespace ULearn.Application.Interfaces;

public class LessonService : ILessonService
{
    public Task<Result<Guid>> AddLessonAsync(Guid moduleId, CreateLessonRequestDto dto, Guid instructorId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<LessonDto>>> GetLessonsByModuleOrderedAsync(Guid moduleId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<LessonDto?>> GetLessonWithContentAsync(Guid lessonId)
    {
        throw new NotImplementedException();
    }
}