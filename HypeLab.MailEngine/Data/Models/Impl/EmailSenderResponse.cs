namespace HypeLab.MailEngine.Data.Models.Impl
{
    /// <summary>
    /// Represents the result of an email sender operation, encapsulating the success or failure state and an optional
    /// message.
    /// </summary>
    /// <remarks>This type is immutable and provides a structured way to represent the outcome of an email
    /// sending operation. Use <see cref="Success(string)"/> to create a successful response and <see
    /// cref="Failure(string)"/> to create a failure response.</remarks>
    public readonly struct EmailSenderResponse
    {
        /// <summary>
        /// Gets a value indicating whether the current state or configuration is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the message associated with the current operation or result.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Represents the response from an email sender operation, indicating success or failure.
        /// </summary>
        /// <param name="isValid">A value indicating whether the email sender operation was successful.  <see langword="true"/> if the
        /// operation was successful; otherwise, <see langword="false"/>.</param>
        /// <param name="message">An optional message providing additional details about the operation result.  Can be <see langword="null"/>
        /// if no message is provided.</param>
        public EmailSenderResponse(bool isValid, string? message = null)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// Creates a successful response for an email sender operation.
        /// </summary>
        /// <param name="message">The message describing the success of the operation.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> instance indicating a valid operation with the specified success
        /// message.</returns>
        public static EmailSenderResponse Success(string message)
        {
            return new EmailSenderResponse(isValid: true, message: message);
        }

        /// <summary>
        /// Creates a response indicating a failure in the email sending operation.
        /// </summary>
        /// <param name="message">A descriptive message explaining the reason for the failure.</param>
        /// <returns>An <see cref="EmailSenderResponse"/> object with <see cref="EmailSenderResponse.IsValid"/> set to <see
        /// langword="false"/> and the provided failure message.</returns>
        public static EmailSenderResponse Failure(string message)
        {
            return new EmailSenderResponse(isValid: false, message: message);
        }
    }
}
