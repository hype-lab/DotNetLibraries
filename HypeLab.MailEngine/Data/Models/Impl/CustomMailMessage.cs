using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the custom mail message.
    /// </summary>
    public class CustomMailMessage
    {
        /// <summary>
        /// The email to.
        /// </summary>
        public string EmailTo { get; set; } = string.Empty;
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
        public IEmailAddressInfo[]? Ccs { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomMailMessage"/> class.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <returns></returns>
        public static CustomMailMessage Create(string emailTo, string emailSubject, string emailFrom, string htmlMessage, [NotNullIfNotNull(nameof(PlainTextContent))] string? plainTextContent = null, [NotNullIfNotNull(nameof(EmailToName))] string? emailToName = null, [NotNullIfNotNull(nameof(EmailFromName))] string? emailFromName = null, IEmailAddressInfo[]? ccs = null)
        {
            return new CustomMailMessage
            {
                EmailTo = emailTo,
                EmailSubject = emailSubject,
                EmailFrom = emailFrom,
                HtmlMessage = htmlMessage,
                PlainTextContent = plainTextContent,
                EmailToName = emailToName,
                EmailFromName = emailFromName,
                Ccs = ccs
            };
        }
    }
}
