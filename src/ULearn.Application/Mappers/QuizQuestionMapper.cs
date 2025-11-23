

using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearn.Application.Mappers;

public static class QuizQuestionMapper
{
    
    public static QuizQuestionDto ToQuizQuestionDto(this  QuizQuestion question)
    {
        return new QuizQuestionDto(question.Id,question.QuestionText,question.QuestionType,question.Points,question.OrderIndex,Options:question.Options.ToList().ToQuizOptionDtoList());
    }

    public static List<QuizQuestionDto> ToQuizQuestionDtoList(this List<QuizQuestion> list)
    {
        return list.Select(x=>x.ToQuizQuestionDto()).ToList();
    }


}