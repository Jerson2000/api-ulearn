

using System.Dynamic;

namespace ULearn.Domain.Entities;

public class UserProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;
    public DateTime CompletedAt { get; set; }
    public int TimeSpentMinutes { get; set; }

}