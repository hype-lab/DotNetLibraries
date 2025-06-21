using HypeLab.MailEngine.Data.Enums;
using HypeLab.MailEngine.Strategies.EmailSender;

namespace HypeLab.MailEngine.Factories
{
    /// <summary>
    /// Defines a factory for creating instances of <see cref="IEmailSender"/>.
    /// </summary>
    /// <remarks>This interface provides methods to create email sender instances based on specific
    /// configurations, such as the type of email sender or a client identifier. Implementations of this factory are
    /// responsible for determining the appropriate <see cref="IEmailSender"/> implementation to return based on the
    /// provided parameters.</remarks>
    public interface IEmailSenderFactory
    {
        /// <summary>
        /// Creates an instance of an email sender based on the specified type.
        /// </summary>
        /// <param name="type">The type of email sender to create. This determines the implementation used for sending emails.</param>
        /// <param name="clientId">An optional client identifier used by certain email sender types.  If <paramref name="clientId"/> is not
        /// required for the specified <paramref name="type"/>, it can be null.</param>
        /// <returns>An object implementing <see cref="IEmailSender"/> that can be used to send emails. The specific
        /// implementation depends on the provided <paramref name="type"/>.</returns>
        IEmailSender CreateEmailSender(EmailSenderType type, string? clientId = null);

        /// <summary>
        /// Creates an instance of an email sender configured for the specified client.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client for whom the email sender is being created. Must not be null or empty.</param>
        /// <returns>An object implementing <see cref="IEmailSender"/> that can be used to send emails.</returns>
        IEmailSender CreateEmailSender(string clientId);
    }
}
