

using System.ComponentModel.DataAnnotations;

namespace ULearn.Domain.Entities;

public class Certificate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? CertificateUrl { get; set; }
}