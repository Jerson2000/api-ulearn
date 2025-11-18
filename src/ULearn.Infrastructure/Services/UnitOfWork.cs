

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.Interfaces;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Repositories;

namespace ULearn.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork, IParallelUnitOfWork
{
    private readonly ULearnDbContext _db;
    private readonly IServiceProvider _serviceProvider;
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
        IServiceProvider serviceProvider,
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
        _serviceProvider = serviceProvider;
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


    public async Task<(T1, T2)> ParallelQueryAsync<T1, T2>(
        Func<IUnitOfWork, Task<T1>> query1,
        Func<IUnitOfWork, Task<T2>> query2)
    {
        var t1 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query1(uow);
        });

        var t2 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query2(uow);
        });

        await Task.WhenAll(t1, t2);
        return (await t1, await t2);
    }

    public async Task<(T1, T2, T3)> ParallelQueryAsync<T1, T2, T3>(
        Func<IUnitOfWork, Task<T1>> query1,
        Func<IUnitOfWork, Task<T2>> query2,
        Func<IUnitOfWork, Task<T3>> query3)
    {
        var t1 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query1(uow);
        });

        var t2 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query2(uow);
        });

        var t3 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query3(uow);
        });

        await Task.WhenAll(t1, t2, t3);
        return (await t1, await t2, await t3);
    }

    public async Task<(T1, T2, T3, T4)> ParallelQueryAsync<T1, T2, T3, T4>(Func<IUnitOfWork, Task<T1>> query1, Func<IUnitOfWork, Task<T2>> query2, Func<IUnitOfWork, Task<T3>> query3, Func<IUnitOfWork, Task<T4>> query4)
    {
        var t1 = Task.Run(async () =>
       {
           await using var scope = _serviceProvider.CreateAsyncScope();
           var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
           return await query1(uow);
       });

        var t2 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query2(uow);
        });

        var t3 = Task.Run(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            return await query3(uow);
        });

        var t4 = Task.Run(async () =>
       {
           await using var scope = _serviceProvider.CreateAsyncScope();
           var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
           return await query4(uow);
       });

        await Task.WhenAll(t1, t2, t3, t4);
        return (await t1, await t2, await t3, await t4);
    }
}