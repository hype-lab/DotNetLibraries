using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Strategies.EmailSender;
using Microsoft.Extensions.DependencyInjection;

namespace HypeLab.MailEngine.Factories.Impl
{
    /// <summary>
    /// Provides functionality to create instances of email senders based on specified configurations and client
    /// identifiers.
    /// </summary>
    /// <remarks>The <see cref="EmailSenderFactory"/> class is designed to resolve and create email sender
    /// instances using dependency injection. It supports multiple or single mail access configurations, allowing
    /// flexibility in managing email sender types.</remarks>
    public class EmailSenderFactory : IEmailSenderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly string _defaultClientId;

        private readonly IMailAccessesInfo? _mailAccessesInfo;
        private readonly IMailAccessInfo? _mailAccessInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSenderFactory"/> class, configuring it with the necessary
        /// mail access information.
        /// </summary>
        /// <remarks>Either <paramref name="mailAccessesInfo"/> or <paramref name="mailAccessInfo"/> must
        /// be provided to initialize the factory. If both are provided, <paramref name="mailAccessesInfo"/> takes
        /// precedence.</remarks>
        /// <param name="serviceProvider">The service provider used to resolve dependencies required by the factory.</param>
        /// <param name="mailAccessesInfo">Optional. An object containing information about multiple mail access configurations. If provided, the
        /// factory will use this to determine the default mail access.</param>
        /// <param name="mailAccessInfo">Optional. An object containing information about a single mail access configuration. If provided, the
        /// factory will use this as the default mail access.</param>
        /// <exception cref="InvalidOperationException">Thrown if both <paramref name="mailAccessesInfo"/> and <paramref name="mailAccessInfo"/> are <see
        /// langword="null"/>.</exception>
        public EmailSenderFactory(IServiceProvider serviceProvider, IMailAccessesInfo? mailAccessesInfo = null, IMailAccessInfo? mailAccessInfo = null)
        {
            _serviceProvider = serviceProvider;

            if (mailAccessesInfo != null)
            {
                _mailAccessesInfo = mailAccessesInfo;
                _defaultClientId = mailAccessesInfo.DefaultMailAccess.ClientId;
            }
            else if (mailAccessInfo != null)
            {
                _mailAccessInfo = mailAccessInfo;
                _defaultClientId = mailAccessInfo.ClientId;
            }
            else
            {
                throw new InvalidOperationException("MailAccessesInfo and MailAccessInfo cannot be null at the same time.");
            }
        }

        /// <summary>
        /// Creates an instance of an email sender based on the specified type and optional client identifier.
        /// </summary>
        /// <remarks>This method uses dependency injection to resolve the appropriate email sender
        /// implementation. If <paramref name="clientId"/> is null, the default client identifier is used.</remarks>
        /// <param name="type">The type of email sender to create. Must be one of the defined values in <see cref="EmailSenderType"/>.</param>
        /// <param name="clientId">An optional client identifier used to resolve the email sender instance. If not provided, a default client
        /// identifier is used.</param>
        /// <returns>An implementation of <see cref="IEmailSender"/> corresponding to the specified <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="type"/> is not a valid <see cref="EmailSenderType"/> value.</exception>
        public IEmailSender CreateEmailSender(EmailSenderType type, string? clientId = null)
        {
            return type switch
            {
                EmailSenderType.Smtp => _serviceProvider.GetRequiredKeyedService<ISmtpEmailSender>(clientId ?? _defaultClientId),
                EmailSenderType.SendGrid => _serviceProvider.GetRequiredKeyedService<ISendGridEmailSender>(clientId ?? _defaultClientId),
                _ => throw new ArgumentException($"Invalid email sender type: {type} with id {clientId ?? _defaultClientId}"),
            };
        }

        /// <summary>
        /// Creates an instance of an email sender for the specified client.
        /// </summary>
        /// <remarks>This method determines the appropriate email sender type based on the provided
        /// <paramref name="clientId"/>. If <c>_mailAccessesInfo</c> is available, it retrieves the email sender type
        /// from the corresponding <c>IMailAccessInfo</c> entry. If <c>_mailAccessInfo</c> is available, it uses its
        /// email sender type. Ensure that either <c>_mailAccessesInfo</c> or <c>_mailAccessInfo</c> is properly
        /// initialized before calling this method.</remarks>
        /// <param name="clientId">The unique identifier of the client for which the email sender is created.</param>
        /// <returns>An implementation of <see cref="IEmailSender"/> configured for the specified client.</returns>
        /// <exception cref="InvalidOperationException">Thrown if both <c>_mailAccessesInfo</c> and <c>_mailAccessInfo</c> are null, as at least one must be
        /// available to determine the email sender type.</exception>
        public IEmailSender CreateEmailSender(string clientId)
        {
            EmailSenderType senderType;
            if (_mailAccessesInfo != null)
            {
                IMailAccessInfo mailAccess = _mailAccessesInfo.MailAccesses.Single(ma => ma.ClientId == clientId);
                senderType = mailAccess.EmailSenderType;
            }
            else if (_mailAccessInfo != null)
            {
                senderType = _mailAccessInfo.EmailSenderType;
            }
            else
            {
                throw new InvalidOperationException($"MailAccessesInfo and MailAccessInfo cannot be null at the same time. Given client id: {clientId}");
            }

            return CreateEmailSender(senderType, clientId);
        }
    }
}
