using HypeLab.MailEngine.Data.Exceptions;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.SmtpClients;
using System.Net.Mail;
using System.Text;

namespace HypeLab.MailEngine.Strategies.EmailSender.Impl
{
    /// <summary>
    /// Represents an SMTP email sender.
    /// </summary>
    /// <remarks>
    /// Constructor for SmtpEmailSender.
    /// </remarks>
    /// <param name="smtpClient"></param>
    public sealed class SmtpEmailSender(HypeLabSmtpClient smtpClient) : ISmtpEmailSender
    {

        // Implementation for sending email using SmtpClient
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

                if (ccs?.Length > 0)
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

                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

                return EmailSenderResponse.Success("Email sent successfully.");
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
    }
}
