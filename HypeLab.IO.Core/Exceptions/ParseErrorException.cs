namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during parsing.
    /// </summary>
    /// <remarks>This exception is typically used to indicate issues encountered while processing or
    /// interpreting input data.</remarks>
    public class ParseErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a default error message.
        /// </summary>
        public ParseErrorException()
            : base("An error occurred during parsing.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParseErrorException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a specified error message and
        /// a reference to the inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ParseErrorException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
