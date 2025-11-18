

using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly ULearnDbContext _db;

    public LessonRepository(ULearnDbContext db) => _db = db;

    public async Task<List<Lesson>> GetLessonsByModuleOrderedAsync(Guid moduleId)
    {
        var lessons =  await _db.Lessons
                 .AsNoTracking()
                 .Where(l => l.ModuleId == moduleId)
                 .OrderBy(l => l.OrderIndex)
                 .ToListAsync();
                 
                 Console.WriteLine($"Ataya: {lessons.Count}");
                 return lessons;
    }
       

    public async Task<Lesson?> GetLessonWithContentAsync(Guid lessonId) =>
        await _db.Lessons
                .AsNoTracking()
                .Include(l => l.Quiz)
                .Include(l => l.Assignment)
                .FirstOrDefaultAsync(l => l.Id == lessonId);
}