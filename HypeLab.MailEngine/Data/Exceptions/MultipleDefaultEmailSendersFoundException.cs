using HypeLab.MailEngine.Data.Models;
using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when there are multiple default email senders found
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.MultipleDefaultEmailSendersFound.DebuggerDisplay)]
    public class MultipleDefaultEmailSendersFoundException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MultipleDefaultEmailSendersFoundException()
            : base(ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public MultipleDefaultEmailSendersFoundException(string? message)
            : base(message ?? ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        public MultipleDefaultEmailSendersFoundException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.MultipleDefaultEmailSendersFound.DefaultMessage, innerException) { }

        /// <summary>
        /// Throws an exception if there are multiple default email senders found
        /// </summary>
        /// <param name="arguments">Arguments to check</param>
        /// <param name="message">Optional error message</param>
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
