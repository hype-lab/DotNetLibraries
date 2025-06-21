using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents the access information required for sending emails, including sender type, client identification, and
    /// default status.
    /// </summary>
    public interface IMailAccessInfo
    {
        /// <summary>
        /// Gets the type of email sender used for sending emails.
        /// </summary>
        public EmailSenderType EmailSenderType { get; }
        /// <summary>
        /// Gets a value indicating whether this email access info is the default one.
        /// </summary>
        public bool IsDefault { get; }
        /// <summary>
        /// Gets the client ID associated with this email access info.
        /// </summary>
        public string ClientId { get; }
    }
}
