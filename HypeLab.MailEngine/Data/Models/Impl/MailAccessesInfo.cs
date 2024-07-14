using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the mail accesses info.
    /// </summary>
    /// <param name="defaultEmailSenderType"></param>
    /// <param name="mailAccesses"></param>
    public class MailAccessesInfo(EmailSenderType defaultEmailSenderType, IEnumerable<IMailAccessInfo> mailAccesses) : IMailAccessesInfo
    {
        /// <summary>
        /// The default email sender type.
        /// </summary>
        public EmailSenderType DefaultEmailSenderType { get; } = defaultEmailSenderType;
        /// <summary>
        /// The mail accesses.
        /// </summary>
        public IEnumerable<IMailAccessInfo> MailAccesses { get; } = mailAccesses;
        /// <summary>
        /// The default mail access.
        /// </summary>
        public IMailAccessInfo DefaultMailAccess => MailAccesses.FirstOrDefault(x => x.IsDefault)!;
    }
}
