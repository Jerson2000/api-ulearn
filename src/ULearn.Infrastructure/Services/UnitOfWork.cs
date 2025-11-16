

using Microsoft.EntityFrameworkCore.Storage;
using ULearn.Application.Interfaces;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Repositories;

namespace ULearn.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ULearnDbContext _db;
    private IDbContextTransaction? _transaction;

    // Specific repos
    public IUserRepository Users { get; }
    public ICourseRepository Courses { get; }
    public IEnrollmentRepository Enrollments { get; }
    // public ILessonRepository Lessons { get; }
    public IQuizRepository Quizzes { get; }
    public IAssignmentRepository Assignments { get; }
    public IPaymentRepository Payments { get; }
    public ICertificateRepository Certificates { get; }
    public IModuleRepository Modules { get; }
    public ILessonRepository Lessons { get; }

    // Generic repo cache
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(
        ULearnDbContext db,
        IUserRepository userRepo,
        ICourseRepository courseRepo,
        IEnrollmentRepository enrollmentRepo,
        IQuizRepository quizRepo,
        IAssignmentRepository assignmentRepo,
        IPaymentRepository paymentRepo,
        ICertificateRepository certificateRepo,
        IModuleRepository moduleRepo,
        ILessonRepository lessonRepo
        )
    {
        _db = db;
        Users = userRepo;
        Courses = courseRepo;
        Enrollments = enrollmentRepo;
        Quizzes = quizRepo;
        Assignments = assignmentRepo;
        Payments = paymentRepo;
        Certificates = certificateRepo;
        Modules = moduleRepo;
        Lessons = lessonRepo;
    }

    // Generic Repository (lazy-loaded)
    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.ContainsKey(type))
        {
            var repo = new Repository<T>(_db);
            _repositories[type] = repo;
        }
        return (Repository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
        => await _db.SaveChangesAsync();

    public async Task BeginTransactionAsync()
        => _transaction = await _db.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _db.Dispose();
    }
}