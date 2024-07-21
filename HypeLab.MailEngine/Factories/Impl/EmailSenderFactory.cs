using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Strategies.EmailSender;
using Microsoft.Extensions.DependencyInjection;

namespace HypeLab.MailEngine.Factories.Impl
{
    /// <summary>
    /// Represents an email sender factory.
    /// </summary>
    public class EmailSenderFactory : IEmailSenderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly string _defaultClientId;

        private readonly IMailAccessesInfo? _mailAccessesInfo;
        private readonly IMailAccessInfo? _mailAccessInfo;

        /// <summary>
        /// EmailSenderFactory constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="mailAccessesInfo"></param>
        /// <param name="mailAccessInfo"></param>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// Creates an email sender.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// Creates an email sender by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
