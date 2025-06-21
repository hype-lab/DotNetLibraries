using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Base
{
    /// <summary>
    /// Represents the base class for constructing and managing email messages.
    /// </summary>
    /// <remarks>This abstract class provides common properties and functionality for email messages,
    /// including required fields such as the subject, sender, and HTML content, as well as optional fields like plain
    /// text content, recipient names, CCs, and attachments. Derived classes can extend this functionality to implement
    /// specific email-related behaviors.</remarks>
    public abstract class BaseMailMessage : IMailMessage
    {
        /// <summary>
        /// Base mail message constructor.
        /// </summary>
        protected BaseMailMessage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMailMessage"/> class with the specified subject, sender,
        /// and HTML message content.
        /// </summary>
        /// <remarks>This constructor sets required members of the <see cref="BaseMailMessage"/> class.
        /// Ensure that all parameters are valid and non-empty before calling this constructor.</remarks>
        /// <param name="emailSubject">The subject of the email. Cannot be null or empty.</param>
        /// <param name="emailFrom">The sender's email address. Cannot be null or empty.</param>
        /// <param name="htmlMessage">The HTML content of the email message. Cannot be null or empty.</param>
        /// <exception cref="ArgumentException">Thrown when any of the parameters are null or empty.</exception>
        [SetsRequiredMembers]
        protected BaseMailMessage(string emailSubject, string emailFrom, string htmlMessage)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(emailSubject);
            ArgumentException.ThrowIfNullOrWhiteSpace(emailFrom);
            ArgumentException.ThrowIfNullOrWhiteSpace(htmlMessage);

            EmailSubject = emailSubject;
            EmailFrom = emailFrom;
            HtmlMessage = htmlMessage;
        }

        /// <summary>
        /// Gets or sets the subject line of the email.
        /// </summary>
        public required string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        public required string EmailFrom { get; set; }

        /// <summary>
        /// Gets or sets the HTML content of the email message.
        /// </summary>
        public required string HtmlMessage { get; set; }

        /// <summary>
        /// Gets or sets the plain text content of the email message.
        /// </summary>
        public string? PlainTextContent { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient for the email.
        /// </summary>
        public string? EmailToName { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender for the email.
        /// </summary>W
        public string? EmailFromName { get; set; }

        /// <summary>
        /// Gets or sets the collection of email addresses to be included as CC (carbon copy) recipients.
        /// </summary>
        public ICollection<IEmailAddressInfo>? Ccs { get; set; }

        /// <summary>
        /// Gets or sets the collection of attachments to be included in the email.
        /// </summary>
        public ICollection<IAttachment>? Attachments { get; set; }
    }
}
