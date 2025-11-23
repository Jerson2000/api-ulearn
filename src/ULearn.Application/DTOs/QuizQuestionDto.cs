

using ULearn.Application.Interfaces;
using ULearn.Domain.Enums;

namespace ULearn.Application.DTOs;

public record QuizQuestionDto(Guid Id,string Question,QuizQuestionTypeEnum QuestionType,int Points,int OrderIndex,List<QuizOptionDto>? Options = default,List<QuizAnswerDto>? Answers = default);

public record CreateQuestionRequesDto(string Question,QuizQuestionTypeEnum QuestionType,int Points,int OrderIndex):IValidateRequest;