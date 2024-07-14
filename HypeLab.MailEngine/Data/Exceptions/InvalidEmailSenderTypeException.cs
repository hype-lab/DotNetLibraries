using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Data.Models.Impl.Credentials;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the email sender type is invalid
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidEmailSenderType.DebuggerDisplay)]
    public class InvalidEmailSenderTypeException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidEmailSenderTypeException()
            : base(ExceptionDefaults.InvalidEmailSenderType.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public InvalidEmailSenderTypeException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public InvalidEmailSenderTypeException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Throws an exception if the email sender type is invalid
        /// </summary>
        /// <param name="mailAccessInfo">>Argument to check</param>
        /// <param name="message">Optional error message</param>
        public static void ThrowIfInvalidEmailSenderType(IMailAccessInfo mailAccessInfo, string? message = null)
        {
            ArgumentNullException.ThrowIfNull(mailAccessInfo);

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
