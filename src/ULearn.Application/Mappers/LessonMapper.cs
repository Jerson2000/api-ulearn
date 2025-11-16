

using System.Collections.ObjectModel;
using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class LessonMapper
{
    public static LessonDto ToLessonDto(this Lesson lesson)
    {
        return new LessonDto(lesson.Id, lesson.Title, lesson.ContentType, lesson.ContentUrl, lesson.ContentText, lesson.OrderIndex, lesson.IsPreview, lesson.Quiz, lesson.Assignment);
    }

    public static List<LessonDto> ToLessonDtoList(this List<Lesson> list)
    {
        return list.Select(x => x.ToLessonDto()).ToList();
    }

}