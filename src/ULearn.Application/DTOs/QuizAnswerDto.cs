


namespace ULearn.Application.DTOs;

public record QuizAnswerDto(Guid Id,Guid AttemptId, Guid QuestionId,Guid SelectedOptionId,string TextAnswer);