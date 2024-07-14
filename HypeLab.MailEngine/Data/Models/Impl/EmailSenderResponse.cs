namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the response from the email sender.
    /// </summary>
    public readonly struct EmailSenderResponse
    {
        /// <summary>
        /// Indicates whether the response is valid.
        /// </summary>
        public bool IsValid { get; }
        /// <summary>
        /// The message.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// EmailSenderResponse constructor.
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="message"></param>
        public EmailSenderResponse(bool isValid, string? message = null)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of the EmailSenderResponse with success.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static EmailSenderResponse Success(string message)
        {
            return new EmailSenderResponse(isValid: true, message: message);
        }

        /// <summary>
        /// Creates a new instance of the EmailSenderResponse with failure.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static EmailSenderResponse Failure(string message)
        {
            return new EmailSenderResponse(isValid: false, message: message);
        }
    }
}
