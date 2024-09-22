using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Base
{
    /// <summary>
    /// Represents the base mail message.
    /// </summary>
    public abstract class BaseMailMessage : IMailMessage
    {
        /// <summary>
        /// Base mail message constructor.
        /// </summary>
        protected BaseMailMessage() { }

        /// <summary>
        /// Base mail message constructor that sets required members.
        /// </summary>
        /// <param name="emailSubject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="htmlMessage"></param>
        [SetsRequiredMembers]
        protected BaseMailMessage(string emailSubject, string emailFrom, string htmlMessage)
        {
            EmailSubject = emailSubject;
            EmailFrom = emailFrom;
            HtmlMessage = htmlMessage;
        }

        /// <summary>
        /// The email subject.
        /// </summary>
        public required string EmailSubject { get; set; }
        /// <summary>
        /// The email from.
        /// </summary>
        public required string EmailFrom { get; set; }
        /// <summary>
        /// The html message.
        /// </summary>
        public required string HtmlMessage { get; set; }
        /// <summary>
        /// The plain text content.
        /// </summary>
        public string? PlainTextContent { get; set; }
        /// <summary>
        /// The email to name.
        /// </summary>
        public string? EmailToName { get; set; }
        /// <summary>
        /// Represents the email from name.
        /// </summary>W
        public string? EmailFromName { get; set; }

        /// <summary>
        /// The other recipients.
        /// </summary>
        public ICollection<IEmailAddressInfo>? Ccs { get; set; }

        /// <summary>
        /// The attachments.
        /// </summary>
        public ICollection<IAttachment>? Attachments { get; set; }
    }
}
