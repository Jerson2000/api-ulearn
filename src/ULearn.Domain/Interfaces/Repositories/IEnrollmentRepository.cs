

using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface IEnrollmentRepository
{
    Task<bool> IsEnrolledAsync(Guid userId, Guid courseId);
    Task<Enrollment?> GetProgressAsync(Guid userId, Guid courseId);
    Task EnrollAsync(Guid userId, Guid courseId);
    Task CompleteCourseAsync(Guid userId, Guid courseId);
}