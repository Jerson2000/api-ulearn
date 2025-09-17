

namespace ULearn.Domain.Interfaces.Services;


public interface IEmailService
{
    /// <summary>
        /// Sends a simple email with a subject and body to a single recipient.
        /// </summary>
        /// <param name="to">Recipient's email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Body content of the email</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendEmailAsync(string to, string subject, string body);

        /// <summary>
        /// Sends an email to a list of recipients with a subject and body.
        /// </summary>
        /// <param name="to">List of recipient email addresses</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Body content of the email</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);

        /// <summary>
        /// Sends an email using a predefined template and dynamic parameters for personalization.
        /// </summary>
        /// <param name="templateId">ID of the email template</param>
        /// <param name="recipient">Recipient's email address</param>
        /// <param name="dynamicParameters">Dynamic values to replace placeholders in the template</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task SendEmailUsingTemplateAsync(string templateId, string recipient, IDictionary<string, string> dynamicParameters);

        /// <summary>
        /// Schedules an email to be sent at a later time.
        /// </summary>
        /// <param name="to">Recipient's email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Body content of the email</param>
        /// <param name="sendAt">Date and time when the email should be sent</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task ScheduleEmailAsync(string to, string subject, string body, DateTime sendAt);

        /// <summary>
        /// Confirms if the email was successfully sent.
        /// </summary>
        /// <param name="emailId">ID of the email that was sent</param>
        /// <returns>Task that returns a boolean indicating success or failure</returns>
        Task<bool> ConfirmEmailDeliveryAsync(Guid emailId);
}