

namespace ULearn.Application.Interfaces;

using ULearn.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    // === Specific Repositories ===
    IUserRepository Users { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    // ILessonRepository Lessons { get; }
    IQuizRepository Quizzes { get; }
    IAssignmentRepository Assignments { get; }
    IPaymentRepository Payments { get; }
    ICertificateRepository Certificates { get; }
    IModuleRepository Modules { get; }
    ILessonRepository Lessons { get; }

    // === Generic Repository (for simple entities) ===
    IRepository<T> Repository<T>() where T : class;

    // === Persistence ===
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}