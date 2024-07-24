using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Services
{
    /// <summary>
    /// Represents an email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<EmailServiceResponse> SendEmailAsync(IMailMessage message, string? clientId = null);
    }
}
