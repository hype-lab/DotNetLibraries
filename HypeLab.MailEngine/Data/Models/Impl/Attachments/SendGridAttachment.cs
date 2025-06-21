namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    /// <summary>
    /// Represents an attachment that can be included in an email sent via SendGrid.
    /// </summary>
    /// <remarks>This class provides properties to define the attachment's name, type, content, and other
    /// metadata. Attachments are typically used to include files such as documents, images, or other data in an
    /// email.</remarks>
    public class SendGridAttachment : IAttachment
    {
        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content ID of the attachment, which can be used for referencing the attachment in the email body.
        /// </summary>
        public string? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the content type of the attachment.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the content of the attachment as a byte array.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets the disposition of the attachment, which can indicate how the attachment should be handled by the email client (e.g., inline or attachment).
        /// </summary>
        public string? Disposition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridAttachment"/> class, representing an attachment for a
        /// SendGrid email.
        /// </summary>
        /// <param name="name">The name of the attachment, typically the file name.</param>
        /// <param name="type">The MIME type of the attachment, such as "application/pdf" or "image/png".</param>
        /// <param name="content">The binary content of the attachment. Cannot be null.</param>
        public SendGridAttachment(string name, string type, byte[] content)
        {
            Name = name;
            Type = type;
            Content = content;
        }
    }
}
