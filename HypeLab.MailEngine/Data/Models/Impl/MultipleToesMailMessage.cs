using HypeLab.MailEngine.Data.Models.Impl.Base;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents an email message that supports multiple recipients.
    /// </summary>
    /// <remarks>The <see cref="MultipleToesMailMessage"/> class is designed to handle email messages with
    /// multiple "To" recipients. It extends the functionality of <see cref="BaseMailMessage"/> by adding support for a
    /// collection of recipient email addresses. Use this class to create and manage email messages with multiple
    /// recipients, including optional properties such as CCs, attachments, and sender/recipient names.</remarks>
    public sealed class MultipleToesMailMessage : BaseMailMessage
    {
        /// <summary>
        /// Gets or sets the collection of email addresses to which the message will be sent.
        /// </summary>
        public required ICollection<string> EmailToes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleToesMailMessage"/> class with the specified recipients,
        /// subject, sender, and HTML message content.
        /// </summary>
        /// <remarks>This constructor sets the required members of the <see
        /// cref="MultipleToesMailMessage"/> class. Ensure that all parameters are valid and non-null before calling
        /// this constructor.</remarks>
        /// <param name="emailToes">A collection of email addresses representing the recipients of the message. Cannot be null or empty.</param>
        /// <param name="emailSubject">The subject of the email message. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email message. Cannot be null or empty.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="emailToes"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="emailToes"/> is less than 1.</exception>
        [SetsRequiredMembers]
        public MultipleToesMailMessage(ICollection<string> emailToes, string emailSubject, string emailFrom, string htmlMessage) : base(emailSubject, emailFrom, htmlMessage)
        {
            ArgumentNullException.ThrowIfNull(emailToes);
            ArgumentOutOfRangeException.ThrowIfLessThan(emailToes.Count, 1);

            EmailToes = emailToes;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MultipleToesMailMessage"/> with the specified email details.
        /// </summary>
        /// <param name="emailToes">A collection of recipient email addresses. Cannot be null or empty.</param>
        /// <param name="emailSubject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email. Cannot be null or empty.</param>
        /// <param name="plainTextContent">Optional plain text content of the email. If provided, it will be included as an alternative to the HTML
        /// content.</param>
        /// <param name="emailToName">Optional name of the recipient(s). If provided, it will be used in the email header.</param>
        /// <param name="emailFromName">Optional name of the sender. If provided, it will be used in the email header.</param>
        /// <param name="ccs">Optional collection of CC email addresses. If provided, these addresses will receive a copy of the email.</param>
        /// <param name="attachments">Optional collection of attachments to include in the email. If provided, these files will be attached to the
        /// email.</param>
        /// <returns>A new instance of <see cref="MultipleToesMailMessage"/> initialized with the specified email details.</returns>
        public static MultipleToesMailMessage Create(ICollection<string> emailToes, string emailSubject, string emailFrom, string htmlMessage, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, ICollection<IEmailAddressInfo>? ccs = null, ICollection<IAttachment>? attachments = null)
        {
            return new MultipleToesMailMessage(emailToes, emailSubject, emailFrom, htmlMessage)
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
