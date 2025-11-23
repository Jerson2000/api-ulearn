


using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class QuizOptionMapper
{
    public static QuizOptionDto ToQuizOptionDto(this QuizOption option)
    {
        return new QuizOptionDto(option.Id,option.OptionText,option.IsCorrect);
    }

    public static List<QuizOptionDto> ToQuizOptionDtoList(this List<QuizOption> list)
    {
        return list.Select(x=>x.ToQuizOptionDto()).ToList();
    }
}