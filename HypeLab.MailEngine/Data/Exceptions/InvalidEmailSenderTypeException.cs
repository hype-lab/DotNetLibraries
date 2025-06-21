using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an invalid email sender type is encountered.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidEmailSenderType.DebuggerDisplay)]
    public class InvalidEmailSenderTypeException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InvalidEmailSenderTypeException"/> class with a default message.
        /// </summary>
        public InvalidEmailSenderTypeException()
            : base(ExceptionDefaults.InvalidEmailSenderType.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidEmailSenderTypeException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidEmailSenderTypeException(string? message)
            : base(message ?? ExceptionDefaults.InvalidEmailSenderType.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="InvalidEmailSenderTypeException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public InvalidEmailSenderTypeException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.InvalidEmailSenderType.DefaultMessage, innerException) { }

        /// <summary>
        /// Validates the type of the provided email sender information and throws an exception if it is invalid.
        /// </summary>
        /// <param name="mailAccessInfo">The email sender information to validate. Must be an instance of <see cref="SmtpAccessInfo"/> or <see
        /// cref="SendGridAccessInfo"/>.</param>
        /// <param name="message">An optional custom error message to include in the exception. If not provided, a default message will be
        /// used.</param>
        public static void ThrowIfInvalidEmailSenderType(IMailAccessInfo mailAccessInfo, string? message = null)
        {
            if (mailAccessInfo is SmtpAccessInfo)
                return;
            if (mailAccessInfo is SendGridAccessInfo)
                return;

            Throw(message ?? ExceptionDefaults.InvalidEmailSenderType.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new InvalidEmailSenderTypeException(message);
    }
}
