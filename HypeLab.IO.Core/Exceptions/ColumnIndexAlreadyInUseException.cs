namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specified column index is already in use.
    /// </summary>
    /// <remarks>This exception is typically used in scenarios where column indices must be unique,  and an
    /// attempt is made to assign or use a column index that has already been allocated.</remarks>
    public class ColumnIndexAlreadyInUseException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when a specified column index is already in use.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that an operation attempted to assign or
        /// use a column index that is already occupied or reserved. Ensure that the column index being specified is
        /// unique before performing the operation.</remarks>
        public ColumnIndexAlreadyInUseException()
            : base("The specified column index is already in use.") { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is already in use.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ColumnIndexAlreadyInUseException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is already in use.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ColumnIndexAlreadyInUseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
