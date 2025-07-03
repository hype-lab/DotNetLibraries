namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a column index attribute is negative.
    /// </summary>
    /// <remarks>This exception is typically used to indicate invalid input where a column index attribute
    /// must be non-negative.</remarks>
    public class NegativeColumnIndexAttributeException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when a column index attribute is negative.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that a column index attribute value
        /// provided to a method or operation is invalid because it is less than zero.</remarks>
        public NegativeColumnIndexAttributeException()
            : base("The column index attribute cannot be negative.") { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is negative in a context where it must be
        /// non-negative.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NegativeColumnIndexAttributeException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is negative.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public NegativeColumnIndexAttributeException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
