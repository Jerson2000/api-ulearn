using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.Interfaces;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Domain.Interfaces.Services;
using ULearn.Domain.Shared;
using ULearn.Infrastructure.Configurations;
using ULearn.Infrastructure.Data;
using ULearn.Infrastructure.Repositories;
using ULearn.Infrastructure.Services;
using ULearn.Infrastructure.Services.Storages;

namespace ULearn.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ULearnDbContext>(option =>
        {
            option.UseSqlServer(EnvironmentValues.DB_CONNECTION);
        });

        #region Configs

        services.AddJWTConfig(config);
        services.AddCacheConfig();
        services.AddRateLimitConfig(config);
        // services.AddEmailConfig();

        #endregion


        #region  Repositories

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICertificateRepository, CertificateRepository>();
        services.AddScoped<IModuleRepository,ModuleRepository>();
        services.AddScoped<ILessonRepository,LessonRepository>();

        #endregion


        #region  Services

        services.AddTransient<ICacheService, CacheService>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddKeyedTransient<IStorageService, LocalStorageService>("local");
        services.AddKeyedTransient<IStorageService, ImagekitStorageService>("imagekitStorage");
        // unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IParallelUnitOfWork, UnitOfWork>();

        #endregion

        return services;
    }
}