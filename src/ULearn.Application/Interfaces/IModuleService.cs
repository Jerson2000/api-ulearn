

using ULearn.Application.DTOs;
using ULearn.Domain.Shared;

namespace ULearn.Application.Interfaces;

public interface IModuleService
{
    Task<Result<ModuleDto?>> GetModuleAsync(Guid moduleId, Guid userId);
    Task<Result<List<ModuleDto>>> GetModulesByCourseOrderedAsync(Guid courseId, Guid userId);
    Task<Result<Guid>> AddModuleAsync(Guid courseId, CreateModuleRequestDto dto, Guid instructorId);
    Task<Result> UpdateModuleAsync(Guid moduleId, CreateModuleRequestDto dto, Guid instructorId);
}