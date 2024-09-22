using System.Net.Mime;
using System.Text;

namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    /// <summary>
    /// Represents an attachment for the SmtpClient.
    /// </summary>
    /// <remarks>
    /// Constructor with name, file path and content-type.
    /// </remarks>
    /// <param name="name"></param>
    /// <param name="filePath"></param>
    /// <param name="contentType"></param>
    public class SmtpClientAttachment(string name, string filePath, string contentType) : IAttachment
    {
        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string Name { get; set; } = name;
        /// <summary>
        /// The content id of the attachment.
        /// </summary>
        public string? ContentId { get; set; }
        /// <summary>
        /// The file path of the attachment.
        /// </summary>
        public string FilePath { get; set; } = filePath;
        /// <summary>
        /// The media type of the attachment.
        /// </summary>
        public string? MediaType { get; set; }
        /// <summary>
        /// The content type of the attachment.
        /// </summary>
        public string ContentType { get; set; } = contentType;
        /// <summary>
        /// The name encoding of the attachment.
        /// </summary>
        public Encoding? NameEncoding { get; set; }
        /// <summary>
        /// The transfer encoding of the attachment.
        /// </summary>
        public TransferEncoding? TransferEncoding { get; set; }
    }
}
