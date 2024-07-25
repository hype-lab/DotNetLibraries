using HypeLab.MailEngine.Data.Models.Impl.Base;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the custom mail message to a single mail address.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the <see cref="CustomMailMessage"/> class that sets required members.
    /// </remarks>
    /// <param name="emailTo"></param>
    /// <param name="emailSubject"></param>
    /// <param name="emailFrom"></param>
    /// <param name="htmlMessage"></param>
    [method: SetsRequiredMembers]
    public sealed class CustomMailMessage(string emailTo, string emailSubject, string emailFrom, string htmlMessage) : BaseMailMessage(emailSubject, emailFrom, htmlMessage)
    {
        /// <summary>
        /// The email to.
        /// </summary>
        public required string EmailTo { get; set; } = emailTo;

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
            return new CustomMailMessage(emailTo, emailSubject, emailFrom, htmlMessage)
            {
                PlainTextContent = plainTextContent,
                EmailToName = emailToName,
                EmailFromName = emailFromName,
                Ccs = ccs
            };
        }
    }
}
