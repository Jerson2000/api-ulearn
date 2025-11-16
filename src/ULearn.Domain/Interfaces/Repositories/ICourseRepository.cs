



using ULearn.Domain.Entities;
using ULearn.Domain.ValueObjects;

namespace ULearn.Domain.Interfaces.Repositories;

public interface ICourseRepository
{
    Task<Course?> GetWithDetailsAsync(Guid courseId);
    Task<PagedResult<Course>> GetPublishedAsync(int page, int pageSize, string? search = null);
    Task<List<Course>> GetByInstructorAsync(Guid instructorId);
    Task<int> GetEnrollmentCountAsync(Guid courseId);
}