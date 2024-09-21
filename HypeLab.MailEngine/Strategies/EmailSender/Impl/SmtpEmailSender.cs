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
        /// <param name="attachments"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="SmtpEmailSenderException"></exception>
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

                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

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
        /// Sends an email to multiple addresses.
        /// </summary>
        /// <param name="emailToes"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="subject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        /// <exception cref="SmtpEmailSenderException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
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

                await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

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
