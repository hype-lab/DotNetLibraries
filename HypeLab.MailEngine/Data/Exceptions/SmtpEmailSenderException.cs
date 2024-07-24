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
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message"></param>
        public SmtpEmailSenderException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SmtpEmailSenderException(string message, Exception inner)
            : base(message, inner) { }
    }
}
