



using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface IModuleRepository
{
    Task<List<Module>> GetModulesByCourseOrderedAsync(Guid courseId);
    Task<Module?> GetModuleAsync(Guid moduleId, bool includeCourse, bool includeLessons = false);
}