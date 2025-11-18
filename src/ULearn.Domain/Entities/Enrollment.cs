


using System.Diagnostics.Contracts;
using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public double ProgressPercentage { get; set; }
    public DateTime CompletedAt { get; set; }
    public EnrollmentStatusEnum EnrollmentStatus { get; set; } = EnrollmentStatusEnum.Active;
}