using HypeLab.MailEngine.Data.Enums;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents information about mail accesses, including the default email sender type and a collection of mail
    /// access details.
    /// </summary>
    /// <remarks>This class provides access to the default email sender type and a collection of mail access
    /// configurations. It also exposes a property to retrieve the default mail access, which is determined by the <see
    /// cref="IMailAccessInfo.IsDefault"/> property.</remarks>
    public class MailAccessesInfo : IMailAccessesInfo
    {
        /// <summary>
        /// Gets the default email sender type.
        /// </summary>
        public EmailSenderType DefaultEmailSenderType { get; }

        /// <summary>
        /// Gets the collection of mail accesses.
        /// </summary>
        public IEnumerable<IMailAccessInfo> MailAccesses { get; }

        /// <summary>
        /// Gets the default mail access from the collection of mail accesses.
        /// </summary>
        public IMailAccessInfo DefaultMailAccess => MailAccesses.FirstOrDefault(x => x.IsDefault)!;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailAccessesInfo"/> class with the specified default email
        /// sender type and mail access information.
        /// </summary>
        /// <remarks>The <paramref name="defaultEmailSenderType"/> parameter specifies the default
        /// mechanism for sending emails. The <paramref name="mailAccesses"/> parameter provides detailed access
        /// information for multiple mail accounts or services.</remarks>
        /// <param name="defaultEmailSenderType">The default email sender type to be used.</param>
        /// <param name="mailAccesses">A collection of mail access information objects. Cannot be null.</param>
        public MailAccessesInfo(EmailSenderType defaultEmailSenderType, IEnumerable<IMailAccessInfo> mailAccesses)
        {
            DefaultEmailSenderType = defaultEmailSenderType;
            MailAccesses = mailAccesses;
        }
    }
}
