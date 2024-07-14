using HypeLab.MailEngine.Data.Models.Impl;

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
        Task<EmailServiceResponse> SendEmailAsync(CustomMailMessage message);
        /// <summary>
        /// Sends an email by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<EmailServiceResponse> SendEmailAsync(string clientId, CustomMailMessage message);
    }
}
