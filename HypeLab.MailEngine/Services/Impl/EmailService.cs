using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;
using HypeLab.MailEngine.Factories;
using HypeLab.MailEngine.Helpers;
using HypeLab.MailEngine.Strategies.EmailSender;

namespace HypeLab.MailEngine.Services.Impl
{
    /// <summary>
    /// Provides functionality for sending emails using various sender types and configurations.
    /// </summary>
    /// <remarks>The <see cref="EmailService"/> class is designed to facilitate email sending operations by
    /// utilizing configurable email sender types. It supports sending emails to single or multiple recipients and
    /// allows customization of sender behavior through factories and mail access information.</remarks>
    public class EmailService : IEmailService
    {
        private IEmailSender _emailSender;

        private readonly IEmailSenderFactory _emailSenderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class, configuring the email sender based on
        /// the provided mail access information.
        /// </summary>
        /// <remarks>This constructor initializes the email service by creating an email sender using the
        /// default sender type specified in <paramref name="mailAccessesInfo"/>.</remarks>
        /// <param name="mailAccessesInfo">An object containing information about mail access configurations, including the default email sender type.</param>
        /// <param name="emailSenderFactory">A factory used to create instances of <see cref="IEmailSender"/> based on the specified sender type.</param>
        public EmailService(IMailAccessesInfo mailAccessesInfo, IEmailSenderFactory emailSenderFactory)
        {
            _emailSenderFactory = emailSenderFactory;
            _emailSender = _emailSenderFactory.CreateEmailSender(mailAccessesInfo.DefaultEmailSenderType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class, configuring it to send emails using the
        /// specified sender type.
        /// </summary>
        /// <remarks>This constructor sets up the email service by creating an email sender using the
        /// provided factory and mail access information. Ensure that <paramref name="mailAccessInfo"/> contains valid
        /// configuration details for the desired email sender type.</remarks>
        /// <param name="mailAccessInfo">The mail access information containing details about the email sender type and configuration.</param>
        /// <param name="emailSenderFactory">The factory used to create an instance of the email sender based on the specified sender type.</param>
        public EmailService(IMailAccessInfo mailAccessInfo, IEmailSenderFactory emailSenderFactory)
        {
            _emailSenderFactory = emailSenderFactory;
            _emailSender = _emailSenderFactory.CreateEmailSender(mailAccessInfo.EmailSenderType);
        }

        /// <summary>
        /// Sends an email asynchronously using the specified message and optional client identifier.
        /// </summary>
        /// <remarks>This method supports sending emails to single or multiple recipients, depending on
        /// the type of the <paramref name="message"/>. Ensure that the <paramref name="message"/> is properly
        /// configured with all required fields, such as recipients, subject, and content.</remarks>
        /// <param name="message">The email message to be sent. Must be an instance of <see cref="IMailMessage"/>. Supported message types
        /// include <see cref="CustomMailMessage"/> and <see cref="MultipleToesMailMessage"/>.</param>
        /// <param name="clientId">An optional client identifier used to determine the email sender. If not provided, the default sender is
        /// used.</param>
        /// <returns>An <see cref="EmailServiceResponse"/> indicating the result of the email sending operation. The response
        /// contains a validity flag and a message describing the outcome.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the email sender is not set or if the message type is unsupported.</exception>
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
