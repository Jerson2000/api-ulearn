

using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ULearnDbContext _db;
    public EnrollmentRepository(ULearnDbContext db) => _db = db;

    public async Task<bool> IsEnrolledAsync(Guid userId, Guid courseId) =>
        await _db.Enrollments.AsNoTracking().AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

    public async Task<Enrollment?> GetProgressAsync(Guid userId, Guid courseId) =>
        await _db.Enrollments
            .AsNoTracking()
            .Include(e => e.Course).ThenInclude(c => c.Modules).ThenInclude(m => m.Lessons)
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

    public async Task EnrollAsync(Guid userId, Guid courseId)
    {
        if (await IsEnrolledAsync(userId, courseId)) return;

        var enrollment = new Enrollment
        {
            UserId = userId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow,
            EnrollmentStatus = EnrollmentStatusEnum.Active
        };

        _db.Enrollments.Add(enrollment);
    }

    public async Task CompleteCourseAsync(Guid userId, Guid courseId)
    {
        var enrollment = await _db.Enrollments.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
        if (enrollment != null)
        {
            enrollment.EnrollmentStatus = EnrollmentStatusEnum.Completed;
            enrollment.CompletedAt = DateTime.UtcNow;
            enrollment.ProgressPercentage = 100;
        }
    }
}