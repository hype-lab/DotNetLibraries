using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents information about mail access configurations, including default settings and available mail access
    /// options.
    /// </summary>
    /// <remarks>This interface provides properties to retrieve details about the default email sender type,
    /// the collection of mail access configurations, and the default mail access configuration. It is intended to be
    /// used for managing and querying mail access settings in applications.</remarks>
    public interface IMailAccessesInfo
    {
        /// <summary>
        /// Gets the default email sender type used by the application.
        /// </summary>
        public EmailSenderType DefaultEmailSenderType { get; }

        /// <summary>
        /// Gets the collection of mail access configurations available in the application.
        /// </summary>
        public IEnumerable<IMailAccessInfo> MailAccesses { get; }

        /// <summary>
        /// Gets the default mail access configuration from the collection of mail accesses.
        /// </summary>
        public IMailAccessInfo DefaultMailAccess { get; }
    }
}
