using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace HypeLab.MailEngine.Strategies.EmailSender.Impl
{
    /// <summary>
    /// Represents a SendGrid email sender.
    /// </summary>
    /// <remarks>
    /// Constructor for SendGridEmailSender.
    /// </remarks>
    public sealed class SendGridEmailSender(ISendGridClient client) : ISendGridEmailSender
    {

        // Implementation for sending email using SendGrid
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="subject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <returns></returns>
        public async Task<EmailSenderResponse> SendEmailAsync(string emailTo, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, params IEmailAddressInfo[]? ccs)
        {
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

                if (ccs?.Length > 0)
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

                Response response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                string content = await response.Body.ReadAsStringAsync().ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                {
                    return EmailSenderResponse.Success($"Email sent. {content}");
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
    }
}
