using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when no default email sender is found.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.DefaultEmailSenderNotFound.DebuggerDisplay)]
    public class DefaultEmailSenderNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DefaultEmailSenderNotFoundException"/> class with a default message.
        /// </summary>
        public DefaultEmailSenderNotFoundException()
            : base(ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="DefaultEmailSenderNotFoundException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DefaultEmailSenderNotFoundException(string? message)
            : base(message ?? ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="DefaultEmailSenderNotFoundException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DefaultEmailSenderNotFoundException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if no default email sender is found in the provided collection of mail access
        /// information.
        /// </summary>
        /// <remarks>This method iterates through the provided collection to determine if any item is
        /// marked as the default email sender. If none are found, an exception is thrown to indicate that a default
        /// email sender is required.</remarks>
        /// <param name="arguments">A collection of <see cref="IMailAccessInfo"/> objects to check for a default email sender.</param>
        /// <param name="message">An optional custom message for the exception. If not provided, a default message will be used.</param>
        public static void ThrowIfNoDefaultEmailSender(IEnumerable<IMailAccessInfo> arguments, string? message = null)
        {
            foreach (IMailAccessInfo argument in arguments)
            {
                if (argument.IsDefault)
                    return;
            }

            Throw(message ?? ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage);
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new MultipleDefaultEmailSendersFoundException(message);
    }
}
