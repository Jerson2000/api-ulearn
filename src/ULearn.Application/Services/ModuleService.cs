
using ULearn.Application.DTOs;
using ULearn.Application.Interfaces;
using ULearn.Application.Mappers;
using ULearn.Domain.Entities;
using ULearn.Domain.Shared;

namespace ULearn.Application.Services;

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParallelUnitOfWork _parallelUnitOfWork;
    public ModuleService(IUnitOfWork unitOfWork, IParallelUnitOfWork parallelUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _parallelUnitOfWork = parallelUnitOfWork;
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

    public async Task<Result> UpdateModuleAsync(Guid moduleId, CreateModuleRequestDto dto, Guid instructorId)
    {
        if (moduleId == Guid.Empty)
            return Result.FailureBadRequest<Result>("Module ID cannot be empty.");

        if (instructorId == Guid.Empty)
            return Result.FailureForbidden<Result>("Authentication required.");

        var module = await _unitOfWork.Modules.GetModuleAsync(moduleId, true);

        if (module == null)
            return Result.FailureNotFound<Result>("Module not found.");

        if (module.Course?.InstructorId != instructorId)
            return Result.FailureForbidden<Result>("You are not authorized to update this module.");

        module.Title = dto.Title?.Trim() ?? module.Title;
        module.Description = dto.Description?.Trim() ?? module.Description;
        module.OrderIndex = dto.OrderIndex;
        
        _unitOfWork.Repository<Module>().Update(module);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<ModuleDto?>> GetModuleAsync(Guid moduleId, Guid userId)
    {
        var module = await _unitOfWork.Modules.GetModuleAsync(moduleId, true);
        if (module != null)
        {
            var isUserEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, module.CourseId);
            if (module.Course?.InstructorId != userId && !isUserEnrolled)
                return Result.FailureForbidden<ModuleDto?>();
        }

        return Result.Success(module?.ToModuleDto());
    }

    public async Task<Result<List<ModuleDto>>> GetModulesByCourseOrderedAsync(Guid courseId, Guid userId)
    {

        var (course, isUserEnrolled) =
            await _parallelUnitOfWork.ParallelQueryAsync(
                u => u.Courses.GetWithDetailsAsync(courseId),
                u => u.Enrollments.IsEnrolledAsync(userId, courseId)
            );

        if (course is null)
            return Result.FailureNotFound<List<ModuleDto>>("Course not found.");


        if (course.InstructorId != userId && !isUserEnrolled)
            return Result.FailureForbidden<List<ModuleDto>>();


        var modules = await _unitOfWork.Modules.GetModulesByCourseOrderedAsync(courseId);

        return Result.Success(modules.ToModuleDtoList());
    }
}