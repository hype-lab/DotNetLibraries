using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when there is an issue with the SMTP client certificate.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.SmtpClientCert.DebuggerDisplay)]
    public class SmtpClientCertException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SmtpClientCertException"/> class with a default message.
        /// </summary>
        public SmtpClientCertException()
            : base(ExceptionDefaults.SmtpClientCert.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmtpClientCertException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SmtpClientCertException(string? message)
            : base(message ?? ExceptionDefaults.SmtpClientCert.DefaultMessage) { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmtpClientCertException"/> class with a specified message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SmtpClientCertException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.SmtpClientCert.DefaultMessage, innerException) { }
    }
}
