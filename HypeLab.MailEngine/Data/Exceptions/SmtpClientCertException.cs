using HypeLab.MailEngine.Helpers.Const;
using System.Diagnostics;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception for the SMTP client certificate.
    /// </summary>
    [DebuggerDisplay(ExceptionDefaults.SmtpClientCert.DebuggerDisplay)]
    public class SmtpClientCertException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SmtpClientCertException()
            : base(ExceptionDefaults.SmtpClientCert.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmtpClientCertException(string? message)
            : base(message ?? ExceptionDefaults.SmtpClientCert.DefaultMessage) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmtpClientCertException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.SmtpClientCert.DefaultMessage, innerException) { }
    }
}
