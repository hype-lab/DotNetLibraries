using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the mail message type is unknown
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.InvalidEmailSenderType.DebuggerDisplay)]
    public class UnknownMailMessageTypeException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UnknownMailMessageTypeException()
            : base(ExceptionDefaults.UnknownMailMessageType.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public UnknownMailMessageTypeException(string? message)
            : base (message ?? ExceptionDefaults.UnknownMailMessageType.DefaultMessage) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnknownMailMessageTypeException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.UnknownMailMessageType.DefaultMessage, innerException) { }
    }
}
