
using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the default email sender is not found
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.DefaultEmailSenderNotFound.DebuggerDisplay)]
    public class DefaultEmailSenderNotFoundException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultEmailSenderNotFoundException()
            : base(ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public DefaultEmailSenderNotFoundException(string? message)
            : base(message ?? ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public DefaultEmailSenderNotFoundException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DefaultEmailSenderNotFound.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if no default email sender is found
        /// </summary>
        /// <param name="arguments">Arguments to check</param>
        /// <param name="message">Optional error message</param>
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
