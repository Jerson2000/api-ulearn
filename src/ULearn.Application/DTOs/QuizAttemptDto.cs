

using System.Security.Cryptography.X509Certificates;

namespace ULearn.Application.DTOs;

public record QuizAttemptDto(Guid Id,Guid QuizId,DateTime StartedAt, DateTime SubmittedAt,double Score,bool IsPassed,List<QuizAnswerDto>? Answers = default);



