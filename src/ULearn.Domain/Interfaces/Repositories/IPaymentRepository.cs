

namespace ULearn.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task RecordPaymentAsync(Guid userId,Guid courseId,double amount, string transactionId, string gateway);
}