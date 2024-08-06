using HypeLab.MailEngine.Data.Models.Impl.Base;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the custom mail message for sending email to multiple mail addresses.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the <see cref="MultipleToesMailMessage"/> class that sets required members.
    /// </remarks>
    /// <param name="emailToes"></param>
    /// <param name="emailSubject"></param>
    /// <param name="emailFrom"></param>
    /// <param name="htmlMessage"></param>
    [method: SetsRequiredMembers]
    public sealed class MultipleToesMailMessage(ICollection<string> emailToes, string emailSubject, string emailFrom, string htmlMessage) : BaseMailMessage(emailSubject, emailFrom, htmlMessage)
    {
        /// <summary>
        /// The email toes.
        /// </summary>
        public required ICollection<string> EmailToes { get; set; } = emailToes;

        /// <summary>
        /// Creates a new instance of the <see cref="MultipleToesMailMessage"/> class.
        /// </summary>
        /// <param name="emailToes"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <returns></returns>
        public static MultipleToesMailMessage Create(ICollection<string> emailToes, string emailSubject, string emailFrom, string htmlMessage, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, IEmailAddressInfo[]? ccs = null)
        {
            return new MultipleToesMailMessage(emailToes, emailSubject, emailFrom, htmlMessage)
            {
                PlainTextContent = plainTextContent,
                EmailToName = emailToName,
                EmailFromName = emailFromName,
                Ccs = ccs
            };
        }
    }
}
