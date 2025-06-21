using HypeLab.MailEngine.Data.Models.Impl;
using HypeLab.MailEngine.Data.Models.Impl.Base;

namespace HypeLab.MailEngine.Services
{
    /// <summary>
    /// Defines a contract for sending emails asynchronously.
    /// </summary>
    /// <remarks>Implementations of this interface provide functionality to send email messages using
    /// asynchronous operations. Ensure that the email message contains valid data, including recipients, subject, and
    /// body, before invoking the <see cref="SendEmailAsync"/> method.</remarks>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously using the specified message and optional client identifier.
        /// </summary>
        /// <remarks>This method performs the email sending operation asynchronously. Ensure that the
        /// <paramref name="message"/> contains valid data, including at least one recipient, before calling this
        /// method.</remarks>
        /// <param name="message">The email message to be sent. This parameter must not be <see langword="null"/> and should contain all
        /// required fields, such as recipients, subject, and body.</param>
        /// <param name="clientId">An optional identifier for the client making the request. If <see langword="null"/>, the default client
        /// configuration will be used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="EmailServiceResponse"/> object indicating the success or failure of the email delivery.</returns>
        Task<EmailServiceResponse> SendEmailAsync(IMailMessage message, string? clientId = null);
    }
}
