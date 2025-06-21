using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Attachments;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace HypeLab.MailEngine.Strategies.EmailSender.Impl
{
    /// <summary>
    /// Provides functionality for sending emails using the SendGrid service.
    /// </summary>
    /// <remarks>This class serves as a wrapper around the SendGrid API, enabling the sending of emails with
    /// support for HTML and plain text content,  CC/BCC recipients, and file attachments. It validates input parameters
    /// and handles errors related to email sending operations.</remarks>
    public sealed class SendGridEmailSender : ISendGridEmailSender
    {
        private readonly ISendGridClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridEmailSender"/> class.
        /// </summary>
        /// <param name="client">The <see cref="ISendGridClient"/> instance used to send emails via the SendGrid service. This parameter
        /// cannot be null.</param>
        public SendGridEmailSender(ISendGridClient client)
        {
            _client = client;
        }

        // Implementation for sending email using SendGrid
        /// <summary>
        /// Sends an email asynchronously using the SendGrid service.
        /// </summary>
        /// <remarks>This method uses the SendGrid API to send emails. It supports HTML and plain text
        /// content, optional CC/BCC recipients, and file attachments. Ensure that all required parameters are provided
        /// and valid.</remarks>
        /// <param name="emailTo">The recipient's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email. Cannot be null or empty.</param>
        /// <param name="subject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content of the email. If provided, it will be included alongside the HTML content.</param>
        /// <param name="emailToName">Optional name of the recipient. If provided, it will be used in the email's "To" field.</param>
        /// <param name="emailFromName">Optional name of the sender. If provided, it will be used in the email's "From" field.</param>
        /// <param name="ccs">Optional collection of CC, BCC, or additional "To" email addresses. Each address must specify its type
        /// (e.g., CC, BCC, or To). If provided, these addresses will be added to the email.</param>
        /// <param name="attachments">Optional collection of attachments to include in the email. Each attachment must conform to the expected
        /// format and type.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> indicating the result of the email sending operation. Returns a success
        /// response if the email is sent successfully, or a failure response with details if the operation fails.</returns>
        /// <exception cref="ArgumentException">Thrown if any of the required parameters are null or empty.</exception>
        /// <exception cref="SendGridEmailSenderException">Thrown if there is an error while sending the email, such as invalid email address types or attachment issues.</exception>
        public async Task<EmailSenderResponse> SendEmailAsync(string emailTo, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(emailTo);
            ArgumentException.ThrowIfNullOrEmpty(htmlMessage);
            ArgumentException.ThrowIfNullOrEmpty(subject);
            ArgumentException.ThrowIfNullOrEmpty(emailFrom);

            try
            {
                SendGridMessage msg = new()
                {
                    From = new EmailAddress(emailFrom, emailFromName),
                    Subject = subject,
                    HtmlContent = htmlMessage
                };

                if (!string.IsNullOrWhiteSpace(plainTextContent))
                    msg.PlainTextContent = plainTextContent;

                msg.AddTo(new EmailAddress(emailTo, emailToName));

                if (ccs?.Count > 0)
                {
                    foreach (IEmailAddressInfo cc in ccs)
                    {
                        if (cc.IsTo)
                            msg.AddTo(new EmailAddress(cc.Email, cc.Name));
                        else if (cc.IsCc)
                            msg.AddCc(new EmailAddress(cc.Email, cc.Name));
                        else if (cc.IsBcc)
                            msg.AddBcc(new EmailAddress(cc.Email, cc.Name));
                        else
                            throw new SendGridEmailSenderException("Invalid email address type.");
                    }
                }

                if (attachments?.Count > 0)
                {
                    foreach (IAttachment attachment in attachments)
                    {
                        if (attachment is not SendGridAttachment sendGridAttachment)
                            throw new SendGridEmailSenderException("Invalid attachment type.");

                        msg.AddAttachment(new Attachment()
                        {
                            Content = Convert.ToBase64String(sendGridAttachment.Content),
                            Filename = sendGridAttachment.Name,
                            Type = sendGridAttachment.Type,
                            Disposition = sendGridAttachment.Disposition ?? "attachment",
                            ContentId = sendGridAttachment.ContentId ?? Guid.NewGuid().ToString()
                        });
                    }
                }

                Response response = await _client.SendEmailAsync(msg).ConfigureAwait(false);
                string content = await response.Body.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                {
                    return EmailSenderResponse.Success($"Email sent.\n{content}");
                }
                else
                {
                    return EmailSenderResponse.Failure($"Failed to send email. Status Code: {response.StatusCode} Content: {content}");
                }
            }
            catch (SendGridEmailSenderException ex)
            {
                return EmailSenderResponse.Failure($"Failed to send email: {ex.Message} {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return EmailSenderResponse.Failure($"Failed to send email: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Sends an email asynchronously to one or more recipients with the specified content, subject, and optional
        /// parameters.
        /// </summary>
        /// <remarks>This method uses the SendGrid API to send emails. It supports HTML and plain text
        /// content, CC and BCC recipients, and file attachments. The method validates input parameters and throws
        /// exceptions for invalid or missing values.</remarks>
        /// <param name="emailToes">A collection of recipient email addresses. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email. Cannot be null or empty.</param>
        /// <param name="subject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content of the email. If provided, it will be included in the email.</param>
        /// <param name="emailToName">Optional name of the recipient(s). If provided, it will be associated with the recipient email addresses.</param>
        /// <param name="emailFromName">Optional name of the sender. If provided, it will be associated with the sender's email address.</param>
        /// <param name="ccs">Optional collection of CC and BCC email addresses. Each address must specify whether it is a CC or BCC.</param>
        /// <param name="attachments">Optional collection of file attachments. Attachments must be of type <see cref="IAttachment"/>.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> indicating the success or failure of the email sending operation. If
        /// successful, the response contains a success message; otherwise, it contains an error message.</returns>
        /// <exception cref="ArgumentException">Thrown if any of the required parameters are null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the collection of recipient email addresses is empty.</exception>
        /// <exception cref="SendGridEmailSenderException">Thrown if there is an error while sending the email, such as invalid email address types or attachment issues.</exception>
        public async Task<EmailSenderResponse> SendEmailAsync(ICollection<string> emailToes, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            ArgumentNullException.ThrowIfNull(emailToes);
            ArgumentOutOfRangeException.ThrowIfZero(emailToes.Count);
            ArgumentException.ThrowIfNullOrEmpty(htmlMessage);
            ArgumentException.ThrowIfNullOrEmpty(subject);
            ArgumentException.ThrowIfNullOrEmpty(emailFrom);

            try
            {
                SendGridMessage msg = new()
                {
                    From = new EmailAddress(emailFrom, emailFromName),
                    Subject = subject,
                    HtmlContent = htmlMessage
                };

                if (!string.IsNullOrWhiteSpace(plainTextContent))
                    msg.PlainTextContent = plainTextContent;

                foreach (string emailTo in emailToes)
                    msg.AddTo(new EmailAddress(emailTo, emailToName));

                if (ccs?.Count > 0)
                {
                    foreach (IEmailAddressInfo cc in ccs)
                    {
                        if (cc.IsCc)
                            msg.AddCc(new EmailAddress(cc.Email, cc.Name));
                        else if (cc.IsBcc)
                            msg.AddBcc(new EmailAddress(cc.Email, cc.Name));
                        else
                            throw new SendGridEmailSenderException("Invalid email address type.");
                    }
                }

                if (attachments?.Count > 0)
                {
                    foreach (IAttachment attachment in attachments)
                    {
                        if (attachment is not SendGridAttachment sendGridAttachment)
                            throw new SendGridEmailSenderException("Invalid attachment type.");

                        msg.AddAttachment(new Attachment()
                        {
                            Content = Convert.ToBase64String(sendGridAttachment.Content),
                            Filename = sendGridAttachment.Name,
                            Type = sendGridAttachment.Type,
                            Disposition = sendGridAttachment.Disposition ?? "attachment",
                            ContentId = sendGridAttachment.ContentId ?? Guid.NewGuid().ToString()
                        });
                    }
                }

                Response response = await _client.SendEmailAsync(msg).ConfigureAwait(false);
                string content = await response.Body.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                {
                    return EmailSenderResponse.Success($"Emails sent. {content}");
                }
                else
                {
                    return EmailSenderResponse.Failure($"Failed to send emails. Status Code: {response.StatusCode} Content: {content}");
                }
            }
            catch (SendGridEmailSenderException ex)
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
