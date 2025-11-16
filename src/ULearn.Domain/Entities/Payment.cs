

using System.Dynamic;
using ULearn.Domain.Enums;

namespace ULearn.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!; 
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public double Amount { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }
    public string PaymentGateway { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
    
}