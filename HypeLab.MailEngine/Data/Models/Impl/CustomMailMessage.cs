using HypeLab.MailEngine.Data.Models.Impl.Base;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents a custom email message with additional properties and functionality for sending emails.
    /// </summary>
    /// <remarks>The <see cref="CustomMailMessage"/> class extends the functionality of <see
    /// cref="BaseMailMessage"/>  by adding required recipient information and optional metadata such as CCs,
    /// attachments, and sender/recipient names. Use this class to create and configure email messages for
    /// sending.</remarks>
    public sealed class CustomMailMessage : BaseMailMessage
    {
        /// <summary>
        /// Gets or sets the recipient's email address.
        /// </summary>
        public required string EmailTo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMailMessage"/> class with the specified recipient,
        /// subject, sender, and HTML message content.
        /// </summary>
        /// <remarks>This constructor sets the required members of the <see cref="CustomMailMessage"/>
        /// class and initializes the base class with the provided subject, sender, and message content.</remarks>
        /// <param name="emailTo">The email address of the recipient. Cannot be null or empty.</param>
        /// <param name="emailSubject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The email address of the sender. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email message. Cannot be null or empty.</param>
        [SetsRequiredMembers]
        public CustomMailMessage(string emailTo, string emailSubject, string emailFrom, string htmlMessage) : base(emailSubject, emailFrom, htmlMessage)
        {
            EmailTo = emailTo;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CustomMailMessage"/> with the specified email details.
        /// </summary>
        /// <remarks>This method provides a convenient way to create a fully configured <see
        /// cref="CustomMailMessage"/> object. Optional parameters such as <paramref name="plainTextContent"/>,
        /// <paramref name="emailToName"/>, <paramref name="emailFromName"/>, <paramref name="ccs"/>, and <paramref
        /// name="attachments"/> allow for customization of the email.</remarks>
        /// <param name="emailTo">The recipient's email address. Cannot be null or empty.</param>
        /// <param name="emailSubject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email. Cannot be null or empty.</param>
        /// <param name="plainTextContent">The plain text content of the email. If <paramref name="htmlMessage"/> is provided, this can be null.</param>
        /// <param name="emailToName">The name of the recipient. If provided, it will be used in the email's "To" field.</param>
        /// <param name="emailFromName">The name of the sender. If provided, it will be used in the email's "From" field.</param>
        /// <param name="ccs">A collection of email addresses to be included as CC (carbon copy) recipients. Can be null if no CC
        /// recipients are specified.</param>
        /// <param name="attachments">A collection of attachments to include in the email. Can be null if no attachments are specified.</param>
        /// <returns>A new instance of <see cref="CustomMailMessage"/> initialized with the specified email details.</returns>
        public static CustomMailMessage Create(string emailTo, string emailSubject, string emailFrom, string htmlMessage, [NotNullIfNotNull(nameof(PlainTextContent))] string? plainTextContent = null, [NotNullIfNotNull(nameof(EmailToName))] string? emailToName = null, [NotNullIfNotNull(nameof(EmailFromName))] string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            return new CustomMailMessage(emailTo, emailSubject, emailFrom, htmlMessage)
            {
                PlainTextContent = plainTextContent,
                EmailToName = emailToName,
                EmailFromName = emailFromName,
                Ccs = ccs,
                Attachments = attachments
            };
        }
    }
}
