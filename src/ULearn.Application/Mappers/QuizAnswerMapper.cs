

using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearnq.Application.Mappers;

public static class QuizAnswerMapper
{
    public static QuizAnswerDto ToQuizAnswerDto(this QuizAnswer answer)
    {
        return new QuizAnswerDto(answer.Id, answer.AttemptId, answer.QuestionId, answer.SelectedOptionId, answer.TextAnswer);
    }

    public static QuizAnswer ToQuizAnswerEntity(this QuizAnswerDto dto)
    {
        return new QuizAnswer { Id = dto.Id, AttemptId = dto.QuestionId,QuestionId = dto.QuestionId, SelectedOptionId= dto.SelectedOptionId,TextAnswer = dto.TextAnswer};
    }

    public static List<QuizAnswerDto> ToQuizAnswerDtoList(this List<QuizAnswer> list)
    {
        return list.Select(x => x.ToQuizAnswerDto()).ToList();
    }
    public static List<QuizAnswer> ToQuizAnswerEntityList(this List<QuizAnswerDto> list)
    {
        return list.Select(x => x.ToQuizAnswerEntity()).ToList();
    }
}