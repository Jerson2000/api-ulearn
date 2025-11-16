using ULearn.Domain.Entities;

namespace ULearn.Domain.Interfaces.Repositories;

public interface ICertificateRepository
{
    Task<Certificate?> IssueAsync(Guid userId, Guid courseId, string pdfUrl);
}