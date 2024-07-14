namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the SMTP email sender fails
    /// </summary>
    public class SmtpEmailSenderException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SmtpEmailSenderException() { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmtpEmailSenderException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        public SmtpEmailSenderException(string message, Exception inner)
            : base(message, inner) { }
    }
}
