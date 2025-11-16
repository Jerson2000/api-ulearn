

using System.ComponentModel.DataAnnotations;

namespace ULearn.Domain.Entities;

public class AssignmentSubmission
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string FileUrl { get; set; } = string.Empty;
    public string TextSubmission { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public double Grade { get; set; }
    public string FeedBack { get; set; } = string.Empty;
    public DateTime GradedAt { get; set; }
}