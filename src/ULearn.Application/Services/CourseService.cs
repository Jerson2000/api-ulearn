

using System.Reflection;
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
            return Result.FailureForbidden<Guid>();
        }

        var cat = await _uow.Repository<Category>().GetByIdAsync(request.CategoryId);
        if (cat == null)
            return Result.FailureNotFound<Guid>("The selected category was not found. Please choose a valid category.");


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
    
    public async Task<Result> UpdateAsync(Guid courseId, CreateCourseRequestDto dto,Guid instructorId)
    {
        if (courseId == Guid.Empty)
            return Result.FailureBadRequest<Result>("Course ID cannot be empty.");

        if (instructorId == Guid.Empty)
            return Result.FailureForbidden<Result>("Instructor authentication required.");

        var course = await _uow.Repository<Course>().GetByIdAsync(courseId);
        if (course == null)
            return Result.FailureNotFound<Result>("Course not found.");

        if (course.InstructorId != instructorId)
            return Result.FailureForbidden<Result>("You are not authorized to update this course.");


        if (course.CategoryId != dto.CategoryId)
        {
            var category = await _uow.Repository<Category>().GetByIdAsync(dto.CategoryId);
            if (category == null)
                return Result.FailureNotFound<Result>("The selected category was not found. Please choose a valid category.");
        }


        course.Title = dto.Title;
        course.Description = dto.Description ?? string.Empty;
        course.Price = dto.Price;
        course.CategoryId = dto.CategoryId;
        course.UpdatedAt = DateTime.UtcNow;

        var genericRepo = _uow.Repository<Course>();
        genericRepo.Update(course);
        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> EnrollAsync(Guid userId, Guid courseId)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Result>(Domain.Enums.ErrorCodeEnum.Unauthorized, "Unauthorized.");

        if (await _uow.Enrollments.IsEnrolledAsync(userId, courseId))
            return Result.Failure<Result>(Domain.Enums.ErrorCodeEnum.BadRequest, "Already enrolled.");

        await _uow.Enrollments.EnrollAsync(userId, courseId);
        await _uow.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<bool> IsEnrolledAsync(Guid userId, Guid courseId)
        => await _uow.Enrollments.IsEnrolledAsync(userId, courseId);
}