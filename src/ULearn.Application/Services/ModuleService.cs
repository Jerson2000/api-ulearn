
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork _unitOfWork;
    public ModuleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> AddModuleAsync(Guid courseId, CreateModuleRequestDto dto, Guid instructorId)
    {

        var course = await _unitOfWork.Courses.GetWithDetailsAsync(courseId);
        if (course?.InstructorId != instructorId)
            return Result.FailureForbidden<Guid>();

        var module = new Module
        {
            CourseId = courseId,
            Title = dto.Title,
            Description = dto.Description ?? string.Empty,
            OrderIndex = dto.OrderIndex,
        };

        await _unitOfWork.Repository<Module>().AddAsync(module);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(module.Id);
    }

    public async Task<Result<ModuleDto?>> GetModuleAsync(Guid moduleId, Guid userId)
    {
        var module = await _unitOfWork.Modules.GetModuleAsync(moduleId, true);
        if (module != null)
        {
            var isUserEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, module.CourseId);
            if (module.Course.InstructorId != userId || !isUserEnrolled)
                return Result.FailureForbidden<ModuleDto?>();
        }

        return Result.Success(module?.ToModuleDto());
    }

    public async Task<Result<List<ModuleDto>?>> GetModulesByCourseOrderedAsync(Guid courseId, Guid userId)
    {
        var courseTask = _unitOfWork.Courses.GetWithDetailsAsync(courseId);
        var enrolledTask = _unitOfWork.Enrollments.IsEnrolledAsync(userId, courseId);

        await Task.WhenAll(courseTask, enrolledTask);

        var (course, isUserEnrolled) = (await courseTask, await enrolledTask);

        if (course is null)
            return Result.FailureNotFound<List<ModuleDto>?>("Course not found.");


        if (course.InstructorId != userId || !isUserEnrolled)
            return Result.FailureForbidden<List<ModuleDto>?>();

        var modules = await _unitOfWork.Modules.GetModulesByCourseOrderedAsync(courseId);

        return Result.Success(modules?.ToModuleDtoList());
    }
}