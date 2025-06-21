using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;

namespace HypeLab.MailEngine.Strategies.EmailSender
{
    /// <summary>
    /// Represents an email sender.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email asynchronously with the specified content, recipients, and optional metadata.
        /// </summary>
        /// <remarks>This method sends an email using the provided parameters. The email can include both
        /// HTML and plain text content, as well as optional CC recipients and attachments. Ensure that all required
        /// parameters are valid and non-empty.</remarks>
        /// <param name="emailTo">The email address of the primary recipient. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email. Cannot be null or empty.</param>
        /// <param name="subject">The subject line of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The email address of the sender. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content of the email. If provided, it will be included as an alternative to the HTML
        /// content.</param>
        /// <param name="emailToName">Optional name of the primary recipient. Used for personalization in the email headers.</param>
        /// <param name="emailFromName">Optional name of the sender. Used for personalization in the email headers.</param>
        /// <param name="ccs">Optional collection of CC recipients. Each recipient must implement <see cref="IEmailAddressInfo"/>.</param>
        /// <param name="attachments">Optional collection of attachments to include in the email. Each attachment must implement <see
        /// cref="IAttachment"/>.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of type <see
        /// cref="EmailSenderResponse"/>. The response contains details about the success or failure of the email
        /// sending operation.</returns>
        Task<EmailSenderResponse> SendEmailAsync(string emailTo, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null);

        /// <summary>
        /// Sends an email asynchronously to one or more recipients with the specified content, subject, and optional
        /// metadata.
        /// </summary>
        /// <remarks>This method supports sending emails with both HTML and plain text content, as well as
        /// optional attachments and CC recipients. Ensure that all email addresses provided are valid and properly
        /// formatted.</remarks>
        /// <param name="emailToes">A collection of recipient email addresses. Each address must be a valid email format.</param>
        /// <param name="htmlMessage">The HTML content of the email body. This parameter cannot be null or empty.</param>
        /// <param name="subject">The subject line of the email. This parameter cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Must be a valid email format.</param>
        /// <param name="plainTextContent">Optional plain text content for the email body. If provided, this will be included as an alternative to the
        /// HTML content.</param>
        /// <param name="emailToName">Optional display name for the recipient(s). If specified, this name will appear alongside the recipient
        /// email address.</param>
        /// <param name="emailFromName">Optional display name for the sender. If specified, this name will appear alongside the sender email
        /// address.</param>
        /// <param name="ccs">Optional collection of email addresses to be included as CC (carbon copy) recipients. Each address must be a
        /// valid email format.</param>
        /// <param name="attachments">Optional collection of attachments to include in the email. Each attachment must conform to the <see
        /// cref="IAttachment"/> interface.</param>
        /// <returns>A <see cref="EmailSenderResponse"/> object containing the result of the email sending operation, including
        /// success status and any error details.</returns>
        Task<EmailSenderResponse> SendEmailAsync(ICollection<string> emailToes, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null);
    }
}
