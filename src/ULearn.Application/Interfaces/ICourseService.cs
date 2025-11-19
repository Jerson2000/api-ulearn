

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;
using ULearn.Domain.ValueObjects;

namespace ULearn.Application.Interfaces;

public interface ICourseService
{
    Task<Result<CourseDto?>> GetAsync(Guid courseId);
    Task<Result<PagedResult<CourseDto>>> GetPublishedAsync(int page, int pageSize, string? search = null);
    Task<Result<List<CourseDto>>> GetMyCoursesAsync(Guid userId);
    Task<Result<Guid>> CreateAsync(CreateCourseRequestDto request, Guid instructorId);
    Task<Result> UpdateAsync(Guid courseId, CreateCourseRequestDto dto,Guid instructorId);
    Task<Result> EnrollAsync(Guid userId, Guid courseId);
    Task<bool> IsEnrolledAsync(Guid userId, Guid courseId);
}