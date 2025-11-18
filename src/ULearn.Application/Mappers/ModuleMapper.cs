

using System.Security.Cryptography.X509Certificates;
using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class ModuleMapper
{
    public static ModuleDto ToModuleDto(this Module module)
    {
        return new ModuleDto(module.Id,module.Title,module.Description,module.OrderIndex,module.Course?.ToCourseDto(),module.Lessons.ToList().ToLessonDtoList());
    }

    public static List<ModuleDto> ToModuleDtoList(this List<Module> list)
    {
        return list.Select(x => x.ToModuleDto()).ToList();
    }
}