using HypeLab.MailEngine.Data.Enums;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the response from the email service.
    /// </summary>
    public class EmailServiceResponse
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EmailServiceResponse() { }

        /// <summary>
        /// Constructor with IsValid.
        /// </summary>
        /// <param name="isValid"></param>
        public EmailServiceResponse(bool isValid)
            : this(isValid ? EmailServiceStatus.Success : EmailServiceStatus.Error, null) { }

        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message"></param>
        public EmailServiceResponse(string? message)
        {
            Message = message;
        }

        /// <summary>
        /// Constructor with IsValid and message.
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="message"></param>
        public EmailServiceResponse(bool isValid, string? message)
            : this(isValid ? EmailServiceStatus.Success : EmailServiceStatus.Error, message) { }

        /// <summary>
        /// Constructor with service status and message.
        /// </summary>
        /// <param name="serviceStatus"></param>
        /// <param name="message"></param>
        public EmailServiceResponse(EmailServiceStatus serviceStatus, string? message)
        {
            ServiceStatus = serviceStatus;
            Message = message;
        }

        /// <summary>
        /// The status of the email service.
        /// </summary>
        public EmailServiceStatus ServiceStatus { get; set; } = EmailServiceStatus.Unknown;

        /// <summary>
        /// The message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Indicates whether the response is valid.
        /// </summary>
        public bool HasMessage
        {
            [MemberNotNullWhen(true, nameof(Message))]
            get => !string.IsNullOrWhiteSpace(Message);
        }

        /// <summary>
        /// Indicates whether the response is valid.
        /// </summary>
        public bool IsError
            => ServiceStatus == EmailServiceStatus.Error;
    }
}
