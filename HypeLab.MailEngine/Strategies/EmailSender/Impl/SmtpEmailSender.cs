using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Attachments;
using HypeLab.MailEngine.SmtpClients;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace HypeLab.MailEngine.Strategies.EmailSender.Impl
{
    /// <summary>
    /// Provides functionality for sending emails using the SMTP protocol.
    /// </summary>
    /// <remarks>This class is designed to send emails asynchronously using an underlying SMTP client. It
    /// supports sending emails with HTML content,  optional plain text content, multiple recipients, CC and BCC
    /// addresses, and file attachments. The class ensures that required parameters  are validated before sending emails
    /// and provides detailed responses indicating the success or failure of the operation.</remarks>
    public sealed class SmtpEmailSender : ISmtpEmailSender
    {
        private readonly HypeLabSmtpClient _smtpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailSender"/> class, using the specified SMTP client.
        /// </summary>
        /// <param name="smtpClient">The SMTP client used to send emails. This parameter cannot be <see langword="null"/>.</param>
        public SmtpEmailSender(HypeLabSmtpClient smtpClient)
        {
            ArgumentNullException.ThrowIfNull(smtpClient, nameof(smtpClient));
            _smtpClient = smtpClient;
        }

        // Implementation for sending email using SmtpClient
        /// <summary>
        /// Sends an email asynchronously using the specified parameters.
        /// </summary>
        /// <remarks>This method supports sending emails with optional plain text content, CC and BCC recipients, and file
        /// attachments. The email body is sent as HTML by default. Ensure that all required parameters are provided and
        /// valid.</remarks>
        /// <param name="emailTo">The recipient's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email body. Cannot be null or empty.</param>
        /// <param name="subject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content for the email body. If provided, it may be used as an alternative to the HTML content.</param>
        /// <param name="emailToName">Optional display name for the recipient.</param>
        /// <param name="emailFromName">Optional display name for the sender.</param>
        /// <param name="ccs">Optional collection of CC, BCC, or additional "To" recipients. Each recipient must implement <see
        /// cref="IEmailAddressInfo"/>.</param>
        /// <param name="attachments">Optional collection of file attachments. Each attachment must implement <see cref="IAttachment"/>.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> indicating the success or failure of the email sending operation. If
        /// successful, the response contains a success message. If failed, the response contains an error message.</returns>
        /// <exception cref="ArgumentException">Thrown if any required parameter is null.</exception>
        /// <exception cref="SmtpEmailSenderException">Thrown if there is an error while sending the email.</exception>
        public async Task<EmailSenderResponse> SendEmailAsync(string emailTo, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(emailTo);
            ArgumentException.ThrowIfNullOrEmpty(htmlMessage);
            ArgumentException.ThrowIfNullOrEmpty(subject);
            ArgumentException.ThrowIfNullOrEmpty(emailFrom);

            try
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(emailFrom, emailFromName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                mailMessage.To.Add(new MailAddress(emailTo, emailToName));

                if (ccs?.Count > 0)
                {
                    foreach (IEmailAddressInfo cc in ccs)
                    {
                        if (cc.IsTo)
                            mailMessage.To.Add(new MailAddress(cc.Email, cc.Name));
                        else if (cc.IsCc)
                            mailMessage.CC.Add(new MailAddress(cc.Email, cc.Name));
                        else if (cc.IsBcc)
                            mailMessage.Bcc.Add(new MailAddress(cc.Email, cc.Name));
                        else
                            throw new SmtpEmailSenderException("Invalid email address type.");
                    }
                }

                if (attachments?.Count > 0)
                {
                    foreach (IAttachment attachment in attachments)
                    {
                        if (attachment is not SmtpClientAttachment smtpAttachment)
                            throw new SmtpEmailSenderException("Invalid attachment type.");

                        mailMessage.Attachments.Add(new Attachment(smtpAttachment.FilePath, smtpAttachment.MediaType)
                        {
                            ContentType = new ContentType(smtpAttachment.ContentType),
                            Name = smtpAttachment.Name,
                            ContentId = smtpAttachment.ContentId ?? Guid.NewGuid().ToString(),
                            NameEncoding = smtpAttachment.NameEncoding ?? Encoding.UTF8,
                            TransferEncoding = smtpAttachment.TransferEncoding ?? TransferEncoding.Base64,
                        });
                    }
                }

                await _smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

                return EmailSenderResponse.Success("Email sent.");
            }
            catch (SmtpEmailSenderException ex)
            {
                return EmailSenderResponse.Failure($"Failed to send email: {ex.Message} {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return EmailSenderResponse.Failure($"Failed to send email: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Sends an email message to one or more recipients with the specified content, subject, and optional
        /// attachments.
        /// </summary>
        /// <remarks>This method supports sending HTML-formatted emails and optionally plain text content.
        /// It allows specifying multiple recipients, CCs, BCCs, and attachments. Ensure that all required parameters are provided and
        /// valid.</remarks>
        /// <param name="emailToes">A collection of recipient email addresses. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email body. Cannot be null or empty.</param>
        /// <param name="subject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content of the email body. If provided, it is included as an alternative view.</param>
        /// <param name="emailToName">Optional display name for the recipient(s).</param>
        /// <param name="emailFromName">Optional display name for the sender.</param>
        /// <param name="ccs">Optional collection of CC and BCC email addresses. Each address must specify whether it is a CC or BCC.</param>
        /// <param name="attachments">Optional collection of attachments to include in the email. Attachments must conform to the expected format.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> indicating the success or failure of the email sending operation. If
        /// successful, the response contains a success message; otherwise, it contains an error message.</returns>
        /// <exception cref="ArgumentException">Thrown if any required parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the collection of recipient email addresses is empty.</exception>
        /// <exception cref="SmtpEmailSenderException">Thrown if there is an error while sending the email.</exception>
        public async Task<EmailSenderResponse> SendEmailAsync(ICollection<string> emailToes, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            ArgumentNullException.ThrowIfNull(emailToes);
            ArgumentOutOfRangeException.ThrowIfZero(emailToes.Count);
            ArgumentException.ThrowIfNullOrEmpty(htmlMessage);
            ArgumentException.ThrowIfNullOrEmpty(subject);
            ArgumentException.ThrowIfNullOrEmpty(emailFrom);

            try
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(emailFrom, emailFromName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };

                foreach (string emailTo in emailToes)
                    mailMessage.To.Add(new MailAddress(emailTo, emailToName));

                if (ccs?.Count > 0)
                {
                    foreach (IEmailAddressInfo cc in ccs)
                    {
                        if (cc.IsCc)
                            mailMessage.CC.Add(new MailAddress(cc.Email, cc.Name));
                        else if (cc.IsBcc)
                            mailMessage.Bcc.Add(new MailAddress(cc.Email, cc.Name));
                        else
                            throw new SmtpEmailSenderException("Invalid email address type.");
                    }
                }

                if (attachments?.Count > 0)
                {
                    foreach (IAttachment attachment in attachments)
                    {
                        if (attachment is not SmtpClientAttachment smtpAttachment)
                            throw new SmtpEmailSenderException("Attachment is not valid.");

                        mailMessage.Attachments.Add(new Attachment(smtpAttachment.FilePath, smtpAttachment.MediaType)
                        {
                            ContentType = new ContentType(smtpAttachment.ContentType),
                            Name = smtpAttachment.Name,
                            ContentId = smtpAttachment.ContentId ?? Guid.NewGuid().ToString(),
                            NameEncoding = smtpAttachment.NameEncoding ?? Encoding.UTF8,
                            TransferEncoding = smtpAttachment.TransferEncoding ?? TransferEncoding.Base64,
                        });
                    }
                }

                await _smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

                return EmailSenderResponse.Success("Emails sent.");
            }
            catch (SmtpEmailSenderException ex)
            {
                return EmailSenderResponse.Failure($"Failed to send emails: {ex.Message} {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return EmailSenderResponse.Failure($"Failed to send emails: {ex.Message} {ex.InnerException?.Message}");
            }
        }
    }
}
