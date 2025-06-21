using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when multiple default email senders are found.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.MultipleDefaultEmailSendersFound.DebuggerDisplay)]
    public class MultipleDefaultEmailSendersFoundException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MultipleDefaultEmailSendersFoundException"/> class with a default message.
        /// </summary>
        public MultipleDefaultEmailSendersFoundException()
            : base(ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="MultipleDefaultEmailSendersFoundException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MultipleDefaultEmailSendersFoundException(string? message)
            : base(message ?? ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="MultipleDefaultEmailSendersFoundException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MultipleDefaultEmailSendersFoundException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if more than one default email sender is found in the provided collection.
        /// </summary>
        /// <remarks>This method iterates through the provided collection to check for multiple default
        /// email senders. If more than one default sender is found, an exception is thrown to indicate a configuration
        /// error.</remarks>
        /// <param name="arguments">A collection of <see cref="IMailAccessInfo"/> objects to evaluate.</param>
        /// <param name="message">An optional custom message for the exception. If not provided, a default message will be used.</param>
        public static void ThrowIfMultipleDefaultEmailSenders(IEnumerable<IMailAccessInfo> arguments, string? message = null)
        {
            int defaultFound = 0;
            foreach (IMailAccessInfo argument in arguments)
            {
                if (argument.IsDefault)
                    defaultFound++;

                if (defaultFound > 1)
                    Throw(message ?? ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage);
            }
        }

        [DoesNotReturn]
        private static void Throw(string message)
            => throw new MultipleDefaultEmailSendersFoundException(message);
    }
}
