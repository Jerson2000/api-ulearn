


using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;
namespace ULearn.Application.Interfaces;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParallelUnitOfWork _parallelUnitOfWork;
    public LessonService(IUnitOfWork unitOfWork, IParallelUnitOfWork parallelUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _parallelUnitOfWork = parallelUnitOfWork;
    }
    public async Task<Result<Guid>> AddLessonAsync(Guid moduleId, CreateLessonRequestDto dto, Guid instructorId)
    {
        var module = await _unitOfWork.Modules.GetModuleAsync(moduleId, true);
        if (module?.Course?.InstructorId != instructorId)
            return Result.FailureForbidden<Guid>();


        var lesson = new Lesson
        {
            ModuleId = moduleId,
            Title = dto.Title,
            ContentType = dto.ContentType,
            ContentUrl = dto.ContentUrl ?? string.Empty,
            ContentText = dto.ContentText ?? string.Empty,
            DurationMinutes = dto.DurationMinutes ?? 0,
            OrderIndex = dto.OrderIndex,
            IsPreview = dto.IsPreview
        };

        await _unitOfWork.Repository<Lesson>().AddAsync(lesson);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(lesson.Id);

    }

    public async Task<Result<List<LessonDto>>> GetLessonsByModuleOrderedAsync(Guid courseId,Guid moduleId,Guid userId)
    {
         var (course, isUserEnrolled) =
            await _parallelUnitOfWork.ParallelQueryAsync(
                u => u.Courses.GetWithDetailsAsync(courseId),
                u => u.Enrollments.IsEnrolledAsync(userId, courseId)
            );

        if (course is null)
            return Result.FailureNotFound<List<LessonDto>>("Course not found.");

        if (course.InstructorId != userId && !isUserEnrolled)
            return Result.FailureForbidden<List<LessonDto>>();
        
        var lessons = await _unitOfWork.Lessons.GetLessonsByModuleOrderedAsync(moduleId);

        return Result.Success(lessons.ToLessonDtoList());
    }

    public Task<Result<LessonDto?>> GetLessonWithContentAsync(Guid lessonId)
    {
        throw new NotImplementedException();
    }
}