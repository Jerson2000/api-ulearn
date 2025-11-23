
using System.Net.Mail;
using ULearn.Application.DTOs;
using ULearn.Domain.Entities;

namespace ULearnq.Application.Mappers;

public static class QuizAttemptMapper
{
    public static QuizAttemptDto ToQuizAttemptDto(this QuizAttempt attempt)
    {
        return new QuizAttemptDto(attempt.Id,attempt.QuizId,attempt.StartedAt,attempt.SubmittedAt,attempt.Score,attempt.IsPassed);
    }
}