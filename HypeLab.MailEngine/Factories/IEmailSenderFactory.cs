using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Strategies.EmailSender;

namespace HypeLab.MailEngine.Factories
{
    /// <summary>
    /// Represents an email sender factory.
    /// </summary>
    public interface IEmailSenderFactory
    {
        /// <summary>
        /// Creates an email sender.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEmailSender CreateEmailSender(EmailSenderType type, string? clientId = null);
        /// <summary>
        /// Creates an email sender by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEmailSender CreateEmailSender(string clientId);
    }
}
