using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using ULearn.Domain.Interfaces.Services;

namespace ULearn.Infrastructure.Services.Emails;

public class SmtpEmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPassword) : IEmailService
{
    private readonly string _smtpServer = smtpServer;
    private readonly int _smtpPort = smtpPort;
    private readonly string _smtpUser = smtpUser;
    private readonly string _smtpPassword = smtpPassword;
    private readonly Dictionary<Guid, bool> _emailDeliveryStatus = [];

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var emailId = Guid.NewGuid();
       
            var message = CreateMessage(to, subject, body);
            await SendMessageAsync(message, emailId);
       
    }

    public async Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body)
    {
        var recipients = to.ToList();
        foreach (var batch in recipients)
        {
            var tasks = batch.Select(recipient => SendEmailAsync(batch, subject, body));
            await Task.WhenAll(tasks);
        }
    }

    public Task SendEmailUsingTemplateAsync(string templateId, string recipient, 
        IDictionary<string, string> dynamicParameters)
    {
        throw new NotImplementedException();
    }

    public async Task ScheduleEmailAsync(string to, string subject, string body, DateTime sendAt)
    {
        var delay = sendAt > DateTime.UtcNow ? sendAt - DateTime.UtcNow : TimeSpan.Zero;
        
        if (delay > TimeSpan.Zero)
        {
            await Task.Delay(delay);
        }
        
        await SendEmailAsync(to, subject, body);
    }

    public Task<bool> ConfirmEmailDeliveryAsync(Guid emailId)
    {
        return Task.FromResult(_emailDeliveryStatus.TryGetValue(emailId, out var status) && status);
    }

    private MimeMessage CreateMessage(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ULearn", _smtpUser));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };
        return message;
    }

    private async Task SendMessageAsync(MimeMessage message, Guid emailId)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpServer, _smtpPort);
            await client.AuthenticateAsync(_smtpUser, _smtpPassword);
            await client.SendAsync(message);
            _emailDeliveryStatus[emailId] = true;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }

    private string GetSubjectFromTemplateId(string templateId)
    {
        return templateId switch
        {
            "welcome" => "Welcome to ULearn!",
            "reset" => "Password Reset Request",
            _ => "ULearn Notification"
        };
    }
}
