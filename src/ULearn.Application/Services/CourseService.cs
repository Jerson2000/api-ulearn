

using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.Shared;
using ULearn.Domain.ValueObjects;

namespace ULearn.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _uow;

    public CourseService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<CourseDto?>> GetAsync(Guid courseId)
    {
        var course = await _uow.Courses.GetWithDetailsAsync(courseId);
        var mapped = course?.ToCourseDto();
        return Result.Success(mapped);
    }

    public async Task<Result<PagedResult<CourseDto>>> GetPublishedAsync(int page, int pageSize, string? search = null)
    {
        var result = await _uow.Courses.GetPublishedAsync(page, pageSize, search);
        var mapped = result.Items.ToCourseDtoList();
        return Result.Success(new PagedResult<CourseDto>(mapped, result.Total, page, pageSize));
    }

    public async Task<Result<List<CourseDto>>> GetMyCoursesAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result.FailureUnauthorized<List<CourseDto>>();

        var courses = await _uow.Courses.GetByInstructorAsync(userId);
        var mapped = courses.ToCourseDtoList();
        return Result.Success(mapped);
    }

    public async Task<Result<Guid>> CreateAsync(CreateCourseRequestDto request, Guid instructorId)
    {
        if (instructorId == Guid.Empty)
        {
            return Result.FailureUnauthorized<Guid>();
        }


        var course = new Course
        {
            Title = request.Title,
            Description = request.Description ?? string.Empty,
            Price = request.Price,
            InstructorId = instructorId,
            CategoryId = request.CategoryId,
            IsPublished = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var genericRepo = _uow.Repository<Course>();
        await genericRepo.AddAsync(course);
        await _uow.SaveChangesAsync();

        return Result.Success(course.Id);
    }

    public async Task<Result> EnrollAsync(Guid userId, Guid courseId)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Result>(Domain.Enums.ErroCodeEnum.Unauthorized, "Unauthorized.");

        if (await _uow.Enrollments.IsEnrolledAsync(userId, courseId))
            return Result.Failure<Result>(Domain.Enums.ErroCodeEnum.BadRequest, "Already enrolled.");

        await _uow.Enrollments.EnrollAsync(userId, courseId);
        await _uow.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<bool> IsEnrolledAsync(Guid userId, Guid courseId)
        => await _uow.Enrollments.IsEnrolledAsync(userId, courseId);

  
    public async Task<Result<Guid>> AddLessonAsync(Guid moduleId, CreateLessonRequestDto dto, Guid instructorId)
    {
        var module = await _uow.Repository<Module>().GetByIdAsync(moduleId);
        if (module?.Course?.InstructorId != instructorId)
            return Result.FailureUnauthorized<Guid>();

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

        await _uow.Repository<Lesson>().AddAsync(lesson);
        await _uow.SaveChangesAsync();

        return Result.Success(lesson.Id);
    }


}