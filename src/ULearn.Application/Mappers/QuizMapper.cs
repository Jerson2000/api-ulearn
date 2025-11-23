

using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class QuizMapper
{
    public static QuizDto ToQuizDto(this Quiz quiz)
    {
        return new QuizDto(quiz.Id,quiz.Title,quiz.PassingScore,quiz.TimeLimitMinutes);
    }

    public static Quiz ToQuizEntity(this QuizDto dto)
    {
        return new Quiz{Id = dto.Id,Title = dto.Title,PassingScore = dto.PassingScore,TimeLimitMinutes = dto.TimeLimitInMinutes};
    }
    
    public static List<QuizDto> ToQuizDtoList(this List<Quiz> list)
    {
        return list.Select(x=>x.ToQuizDto()).ToList();
    }
}