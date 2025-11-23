using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ULearn.Application.Interfaces;
using ULearn.Application.Services;

namespace ULearn.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        #region Services

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStorageAppService, StorageService>();
        services.AddScoped<ICourseService,CourseService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddScoped<IModuleService,ModuleService>();
        services.AddScoped<ILessonService,LessonService>();
        services.AddScoped<IQuizService,QuizService>();
        #endregion

        services.AddValidatorsRegistration(config);
        return services;
    }
}
