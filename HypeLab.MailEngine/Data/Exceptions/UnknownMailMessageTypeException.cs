using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an unknown or unsupported mail message type is encountered.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidEmailSenderType.DebuggerDisplay)]
    public class UnknownMailMessageTypeException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="UnknownMailMessageTypeException"/> with the default error message.
        /// </summary>
        public UnknownMailMessageTypeException()
            : base(ExceptionDefaults.UnknownMailMessageType.DefaultMessage) { }

        /// <summary>
        /// Creates an instance of <see cref="UnknownMailMessageTypeException"/> with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UnknownMailMessageTypeException(string? message)
            : base (message ?? ExceptionDefaults.UnknownMailMessageType.DefaultMessage) { }

        /// <summary>
        /// Creates an instance of <see cref="UnknownMailMessageTypeException"/> with a specified error message and an inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public UnknownMailMessageTypeException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.UnknownMailMessageType.DefaultMessage, innerException) { }
    }
}
