using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents an email access info.
    /// </summary>
    public interface IMailAccessInfo
    {
        /// <summary>
        /// The email sender type.
        /// </summary>
        public EmailSenderType EmailSenderType { get; }
        /// <summary>
        /// Indicates if the email access info is the default.
        /// </summary>
        public bool IsDefault { get; }
        /// <summary>
        /// The client ID for this email access info.
        /// </summary>
        public string ClientId { get; }
    }
}
