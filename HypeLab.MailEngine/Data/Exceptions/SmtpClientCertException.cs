using HypeLab.MailEngine.Helpers.Const;

namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception for the SMTP client certificate.
    /// </summary>
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
