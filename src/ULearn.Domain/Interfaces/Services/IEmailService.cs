

namespace ULearn.Domain.Interfaces.Services;


public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);
    Task SendEmailUsingTemplateAsync(string templateId, string recipient, IDictionary<string, string> dynamicParameters);
    Task ScheduleEmailAsync(string to, string subject, string body, DateTime sendAt);
    Task<bool> ConfirmEmailDeliveryAsync(Guid emailId);
}