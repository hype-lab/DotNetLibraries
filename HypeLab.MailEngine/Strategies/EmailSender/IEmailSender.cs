using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl;

namespace HypeLab.MailEngine.Strategies.EmailSender
{
    /// <summary>
    /// Represents an email sender.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="subject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<EmailSenderResponse> SendEmailAsync(string emailTo, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, params IEmailAddressInfo[]? ccs);

        /// <summary>
        /// Sends an email to multiple addresses.
        /// </summary>
        /// <param name="emailToes"></param>
        /// <param name="htmlMessage"></param>
        /// <param name="subject"></param>
        /// <param name="emailFrom"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="emailToName"></param>
        /// <param name="emailFromName"></param>
        /// <param name="ccs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<EmailSenderResponse> SendEmailAsync(ICollection<string> emailToes, string htmlMessage, string subject, string emailFrom, string? plainTextContent = null, string? emailToName = null, string? emailFromName = null, params IEmailAddressInfo[]? ccs);
    }
}
