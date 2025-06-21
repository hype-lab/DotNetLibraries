namespace HypeLab.MailEngine.Data.Models.Impl.Base
{
    /// <summary>
    /// Represents an email message with properties for subject, sender, recipient, content, and additional metadata.
    /// </summary>
    /// <remarks>This interface provides a structure for defining the components of an email message,
    /// including the subject, sender and recipient details,  message content in both HTML and plain text formats, and
    /// optional metadata such as CC recipients and attachments.</remarks>
    public interface IMailMessage
    {
        /// <summary>
        /// Gets or sets the subject line of the email.
        /// </summary>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the email address from which messages are sent.
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// Gets or sets the HTML-formatted message content.
        /// </summary>
        public string HtmlMessage { get; set; }

        /// <summary>
        /// Gets or sets the plain text content of the message.
        /// </summary>
        public string? PlainTextContent { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the recipient's email address.
        /// </summary>
        public string? EmailToName { get; set; }

        /// <summary>
        /// Gets or sets the name displayed as the sender in outgoing email messages.
        /// </summary>
        public string? EmailFromName { get; set; }

        /// <summary>
        /// Gets or sets the collection of email addresses to be included as CC (carbon copy) recipients.
        /// </summary>
        public ICollection<IEmailAddressInfo>? Ccs { get; set; }

        /// <summary>
        /// Gets or sets the collection of attachments associated with the email message.
        /// </summary>
        public ICollection<IAttachment>? Attachments { get; set; }
    }
}
