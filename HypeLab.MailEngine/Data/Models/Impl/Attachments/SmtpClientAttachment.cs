using System.Net.Mime;
using System.Text;

namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    /// <summary>
    /// Represents an email attachment for use with an SMTP client.
    /// </summary>
    /// <remarks>This class provides properties to define the metadata and content details of an email
    /// attachment,  including its name, file path, content type, and encoding options. It is designed to be used in
    /// scenarios where attachments need to be sent via email using an SMTP client.</remarks>
    public class SmtpClientAttachment : IAttachment
    {
        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content ID of the attachment.
        /// </summary>
        public string? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the file path of the attachment.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the media type of the attachment, if applicable.
        /// </summary>
        public string? MediaType { get; set; }

        /// <summary>
        /// Gets or sets the content type of the attachment.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the encoding of the attachment name.
        /// </summary>
        public Encoding? NameEncoding { get; set; }

        /// <summary>
        /// Gets or sets the transfer encoding of the attachment.
        /// </summary>
        public TransferEncoding? TransferEncoding { get; set; }

        /// <summary>
        /// Represents an email attachment with a specified name, file path, and content type.
        /// </summary>
        /// <param name="name">The name of the attachment as it will appear in the email.</param>
        /// <param name="filePath">The full file path to the attachment on the local file system. Must not be null or empty.</param>
        /// <param name="contentType">The MIME type of the attachment, such as "application/pdf" or "image/jpeg". Must not be null or empty.</param>
        public SmtpClientAttachment(string name, string filePath, string contentType)
        {
            Name = name;
            FilePath = filePath;
            ContentType = contentType;
        }
    }
}
