

using System.Net;
using ULearn.Application.Interfaces;

namespace ULearn.Application.DTOs;


public record QuizDto(Guid Id,string Title,int PassingScore,int TimeLimitInMinutes,List<QuizQuestionDto>? Questions = default,List<QuizAttemptDto>? Attempts = default);
public record CreateQuizRequestDto(string Title,int PassingScore,int TimeLimitInMinutes):IValidateRequest;