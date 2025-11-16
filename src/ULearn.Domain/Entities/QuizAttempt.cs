

namespace ULearn.Domain.Entities;

public class QuizAttempt
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime SubmittedAt { get; set; }
    public double Score { get; set; }
    public bool IsPassed { get; set; }

    public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
}