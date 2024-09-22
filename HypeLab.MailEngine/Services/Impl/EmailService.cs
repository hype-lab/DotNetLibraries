using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;
using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Helpers;
using HypeLab.MailEngine.Strategies.EmailSender;
using System.Net.Mail;

namespace HypeLab.MailEngine.Services.Impl
{
    /// <summary>
    /// The email service class that handles sending emails.
    /// </summary>
    public class EmailService : IEmailService
    {
        private IEmailSender _emailSender;

        private readonly IEmailSenderFactory _emailSenderFactory;

        /// <summary>
        /// The constructor for multipled registered IMailAccessInfo (managed by IMailAccessesInfo).
        /// </summary>
        /// <param name="mailAccessesInfo"></param>
        /// <param name="emailSenderFactory"></param>
        public EmailService(IMailAccessesInfo mailAccessesInfo, IEmailSenderFactory emailSenderFactory)
        {
            _emailSenderFactory = emailSenderFactory;
            _emailSender = _emailSenderFactory.CreateEmailSender(mailAccessesInfo.DefaultEmailSenderType);
        }

        /// <summary>
        /// The constructor for a single registered IMailAccessInfo.
        /// </summary>
        /// <param name="mailAccessInfo"></param>
        /// <param name="emailSenderFactory"></param>
        public EmailService(IMailAccessInfo mailAccessInfo, IEmailSenderFactory emailSenderFactory)
        {
            _emailSenderFactory = emailSenderFactory;
            _emailSender = _emailSenderFactory.CreateEmailSender(mailAccessInfo.EmailSenderType);
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<EmailServiceResponse> SendEmailAsync(IMailMessage message, string? clientId = null)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
                _emailSender = clientId.DetermineSenderByClientId(_emailSenderFactory);

            if (_emailSender is null)
                throw new InvalidOperationException("Email sender is not set.");

            EmailSenderResponse response = message switch
            {
                CustomMailMessage singleMailMsg => await _emailSender
                                        .SendEmailAsync(singleMailMsg.EmailTo, message.HtmlMessage, message.EmailSubject, message.EmailFrom, message.PlainTextContent, message.EmailToName, message.EmailFromName, message.Ccs, message.Attachments)
                                        .ConfigureAwait(false),

                MultipleToesMailMessage multipleToesMsg => await _emailSender
                                        .SendEmailAsync(multipleToesMsg.EmailToes, message.HtmlMessage, message.EmailSubject, message.EmailFrom, message.PlainTextContent, message.EmailToName, message.EmailFromName, message.Ccs, message.Attachments)
                                        .ConfigureAwait(false),

                _ => throw new InvalidOperationException("Unknown message type."),
            };

            if (!response.IsValid)
                return new EmailServiceResponse(isValid: false, message: response.Message);

            return new EmailServiceResponse(isValid: true, message: response.Message);
        }
    }
}
