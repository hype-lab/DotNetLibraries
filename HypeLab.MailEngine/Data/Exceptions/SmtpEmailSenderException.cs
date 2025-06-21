namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during the process of sending emails using SMTP.
    /// </summary>
    public class SmtpEmailSenderException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SmtpEmailSenderException"/> class.
        /// </summary>
        public SmtpEmailSenderException() { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmtpEmailSenderException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SmtpEmailSenderException(string message)
            : base(message) { }

        /// <summary>
        /// Creates a new instance of the <see cref="SmtpEmailSenderException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public SmtpEmailSenderException(string message, Exception inner)
            : base(message, inner) { }
    }
}
