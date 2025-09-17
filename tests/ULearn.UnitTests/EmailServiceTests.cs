

using Moq;
using ULearn.Domain.Interfaces.Services;

namespace ULearn.UnitTests;

public class EmailServiceTests
{
    private readonly Mock<IEmailService> _emailService;
    public EmailServiceTests()
    {
        _emailService = new Mock<IEmailService>();
    }
}