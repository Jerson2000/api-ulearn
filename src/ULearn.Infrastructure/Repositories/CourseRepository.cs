


using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.ValueObjects;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class CourseRepository(ULearnDbContext db) : ICourseRepository
{
    private readonly ULearnDbContext _db = db;

    public async Task<Course?> GetWithDetailsAsync(Guid courseId) =>
          await _db.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .Include(c => c.Category)
            .Include(c => c.Modules).ThenInclude(m => m.Lessons)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == courseId);

    public async Task<PagedResult<Course>> GetPublishedAsync(int page, int pageSize, string? search = null)
    {
        var q = _db.Courses
        .AsNoTracking()
        .Include(c => c.Instructor)
        .Include(c => c.Category)
        .Include(c => c.Enrollments)
        .Where(c => c.IsPublished);

        if (!string.IsNullOrWhiteSpace(search))
        {
            q = q.Where(c => c.Title.Contains(search));
        }
        var total = await q.CountAsync();
        var items = await q.OrderBy(c => c.Title)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

        return new PagedResult<Course>(items, total, page, pageSize);
    }

    public async Task<List<Course>> GetByInstructorAsync(Guid instructorId) =>
        await _db.Courses
            .Where(c => c.InstructorId == instructorId)
            .ToListAsync();

    public async Task<int> GetEnrollmentCountAsync(Guid courseId) => await _db.Enrollments.CountAsync(e => e.CourseId == courseId);

}