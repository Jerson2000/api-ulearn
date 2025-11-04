

using System.ComponentModel.DataAnnotations;

namespace ULearn.Domain.Entities;

public class Token
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Access { get; set; } = string.Empty;

    [Required]
    public string Refresh { get; set; } = string.Empty;

    public DateTime ValidUntil { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
    public bool IsValid => DateTime.UtcNow <= ValidUntil;
}