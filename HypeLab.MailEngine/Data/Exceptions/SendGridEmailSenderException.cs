namespace HypeLab.MailEngine.Data.Exceptions
{
    /// <summary>
    /// Exception class for when the SendGrid email sender fails
    /// </summary>
    public class SendGridEmailSenderException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SendGridEmailSenderException() { }

        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="message"></param>
        public SendGridEmailSenderException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SendGridEmailSenderException(string message, Exception inner)
            : base(message, inner) { }
    }
}
