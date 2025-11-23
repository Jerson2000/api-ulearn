

using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;

public record QuizOptionDto(Guid Id,string OptionText,bool IsCorrect);
public record CreateQuestionOptionRequestDto(string OptionText,bool IsCorrect):IValidateRequest;