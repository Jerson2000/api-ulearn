

using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;


public static class CourseMapper
{

    public static CourseDto ToCourseDto(this Course course)
    {
        return new CourseDto(course.Id, course.Title, course.Description, course.Thumbnail, course.Price, course.Instructor?.FullName??string.Empty, course.Enrollments.Count, course.Category?.Name);
    }

    public static List<CourseDto> ToCourseDtoList(this List<Course> courses)
    {
        return courses.Select( course => course.ToCourseDto()).ToList();
    }


}