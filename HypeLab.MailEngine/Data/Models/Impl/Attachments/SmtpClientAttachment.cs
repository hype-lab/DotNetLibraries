using System.Net.Mime;
using System.Text;

namespace HypeLab.MailEngine.Data.Models.Impl.Attachments
{
    public class SmtpClientAttachment : IAttachment
    {
        public SmtpClientAttachment() { }

        public SmtpClientAttachment(string name, string filePath, string contentType)
        {
            Name = name;
            FilePath = filePath;
            ContentType = contentType;
        }

        public SmtpClientAttachment(string name, string filePath, string contentType, string? contentId)
        {
            Name = name;
            FilePath = filePath;
            ContentType = contentType;
            ContentId = contentId;
        }

        public SmtpClientAttachment(string name, string filePath, string contentType, string? mediaType, Encoding? nameEncoding, TransferEncoding? transferEncoding)
        {
            Name = name;
            FilePath = filePath;
            ContentType = contentType;
            MediaType = mediaType;
            NameEncoding = nameEncoding;
            TransferEncoding = transferEncoding;
        }

        public string Name { get; set; }
        public string? ContentId { get; set; }

        public string FilePath { get; set; }
        public string? MediaType { get; set; }
        public string ContentType { get; set; }
        public Encoding? NameEncoding { get; set; }
        public TransferEncoding? TransferEncoding { get; set; }
    }
}
