using HypeLab.MailEngine.Data.Enums;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the response from an email service operation, including its status and any associated message.
    /// </summary>
    /// <remarks>This class provides information about the outcome of an email service operation, including
    /// whether the operation was successful, the status of the operation, and an optional message with additional
    /// details.</remarks>
    public class EmailServiceResponse
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EmailServiceResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceResponse"/> class with a validity status.
        /// </summary>
        /// <param name="isValid">A value indicating whether the email service operation was successful.  <see langword="true"/> if the
        /// operation was successful; otherwise, <see langword="false"/>.</param>
        public EmailServiceResponse(bool isValid)
            : this(isValid ? EmailServiceStatus.Success : EmailServiceStatus.Error, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceResponse"/> class with the specified message.
        /// </summary>
        /// <param name="message">An optional message providing additional information about the response. If <see langword="null"/>, no
        /// message will be included.</param>
        public EmailServiceResponse(string? message)
            : this(EmailServiceStatus.Success, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceResponse"/> class with the specified validity and
        /// message.
        /// </summary>
        /// <param name="isValid">A value indicating whether the email service operation was successful.  <see langword="true"/> if the
        /// operation was successful; otherwise, <see langword="false"/>.</param>
        /// <param name="message">An optional message providing additional details about the operation result.  Can be <see langword="null"/>
        /// if no message is provided.</param>
        public EmailServiceResponse(bool isValid, string? message)
            : this(isValid ? EmailServiceStatus.Success : EmailServiceStatus.Error, message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceResponse"/> class with the specified service status
        /// and message.
        /// </summary>
        /// <param name="serviceStatus">The status of the email service operation.</param>
        /// <param name="message">An optional message providing additional details about the operation result. This can be <see
        /// langword="null"/> if no message is available.</param>
        public EmailServiceResponse(EmailServiceStatus serviceStatus, string? message)
        {
            ServiceStatus = serviceStatus;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the current status of the email service.
        /// </summary>
        public EmailServiceStatus ServiceStatus { get; set; } = EmailServiceStatus.Unknown;

        /// <summary>
        /// Gets or sets the message associated with the current operation or context.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Message"/> property contains a non-empty, non-whitespace
        /// string.
        /// </summary>
        public bool HasMessage
        {
            [MemberNotNullWhen(true, nameof(Message))]
            get => !string.IsNullOrWhiteSpace(Message);
        }

        /// <summary>
        /// Gets a value indicating whether the email service is currently in an error state.
        /// </summary>
        public bool IsError
            => ServiceStatus == EmailServiceStatus.Error;
    }
}
