

using ULearn.Domain.Entities;
using ULearn.Domain.Enums;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ULearnDbContext _db;
    public PaymentRepository(ULearnDbContext db) => _db = db;

    public async Task RecordPaymentAsync(Guid userId,Guid courseId, double amount, string transactionId, string gateway)
    {
        var payment = new Payment
        {
            UserId = userId,
            CourseId = courseId,
            Amount = amount,
            TransactionId = transactionId,
            PaymentGateway = gateway,
            PaymentStatus = PaymentStatusEnum.Completed,
            PaidAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();
    }
}