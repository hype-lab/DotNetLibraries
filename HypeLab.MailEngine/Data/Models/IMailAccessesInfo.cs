using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models
{
    /// <summary>
    /// Represents the mail accesses info.
    /// </summary>
    public interface IMailAccessesInfo
    {
        /// <summary>
        /// The default email sender type.
        /// </summary>
        public EmailSenderType DefaultEmailSenderType { get; }
        /// <summary>
        /// The mail accesses.
        /// </summary>
        public IEnumerable<IMailAccessInfo> MailAccesses { get; }
        /// <summary>
        /// The default mail access.
        /// </summary>
        public IMailAccessInfo DefaultMailAccess { get; }
    }
}
