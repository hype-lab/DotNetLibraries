namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a null object is encountered in a context where it is not allowed.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that a method or operation has received a null
    /// object as input when a non-null value is required.</remarks>
    public class NullObjectException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an object is null and a non-null value is required.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that a method or operation encountered a
        /// null object where a valid instance was expected.</remarks>
        public NullObjectException()
            : base("The object cannot be null.") { }

        /// <summary>
        /// Represents an exception that is thrown when an operation encounters a null object where a non-null object is
        /// required.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NullObjectException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when an operation encounters a null object where a non-null object is
        /// required.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public NullObjectException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
