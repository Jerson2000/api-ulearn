
using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;


public class ModuleRepository : IModuleRepository
{
    private readonly ULearnDbContext _db;

    public ModuleRepository(ULearnDbContext db) => _db = db;

    public async Task<List<Module>> GetModulesByCourseOrderedAsync(Guid courseId) =>
        await _db.Modules
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.OrderIndex)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Module?> GetModuleAsync(Guid moduleId, bool includeCourse = false, bool includeLessons = false)
    {
        var query = _db.Modules.AsQueryable();

        if (includeCourse)
            query = query.Include(m => m.Course);

        if (includeLessons)
            query = query.Include(m => m.Lessons);

        return await query.FirstOrDefaultAsync(m => m.Id == moduleId);
    }
}