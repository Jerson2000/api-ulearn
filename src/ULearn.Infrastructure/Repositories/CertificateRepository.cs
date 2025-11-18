


using Microsoft.EntityFrameworkCore;
using ULearn.Domain.Entities;
using ULearn.Domain.Interfaces.Repositories;
using ULearn.Infrastructure.Data;

namespace ULearn.Infrastructure.Repositories;

public class CertificateRepository : ICertificateRepository
{
    private readonly ULearnDbContext _db;
    public CertificateRepository(ULearnDbContext db) => _db = db;

    public async Task<Certificate?> IssueAsync(Guid userId, Guid courseId, string pdfUrl)
    {
        var existing = await _db.Certificates
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);

        if (existing != null) return existing;

        var cert = new Certificate
        {
            UserId = userId,
            CourseId = courseId,
            CertificateUrl = pdfUrl,
            IssuedAt = DateTime.UtcNow
        };

        _db.Certificates.Add(cert);
        await _db.SaveChangesAsync();
        return cert;
    }

}