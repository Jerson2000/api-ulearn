


using ULearn.Domain.Entities;
namespace ULearn.Domain.Interfaces.Repositories;

public interface ILessonRepository
{
    Task<List<Lesson>> GetLessonsByModuleOrderedAsync(Guid moduleId);
    Task<Lesson?> GetLessonWithContentAsync(Guid lessonId);
    Task<Lesson?> GetLessonWithModuleAsync(Guid lessonId);
}