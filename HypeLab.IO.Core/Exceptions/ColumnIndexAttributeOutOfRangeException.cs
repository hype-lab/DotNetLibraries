namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a column index attribute is outside the valid range.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that a column index provided in an operation is
    /// invalid or exceeds the allowable range. It can be used in scenarios where data processing or validation involves
    /// column indices.</remarks>
    public class ColumnIndexAttributeOutOfRangeException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when a specified column index attribute is out of the valid range.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that a column index attribute provided
        /// to a method or operation is invalid because it falls outside the acceptable range of values.</remarks>
        public ColumnIndexAttributeOutOfRangeException()
            : base("The specified column index attribute is out of range.") { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is out of the valid range.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ColumnIndexAttributeOutOfRangeException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is out of the valid range.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ColumnIndexAttributeOutOfRangeException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
