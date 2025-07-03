namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during object creation.
    /// </summary>
    /// <remarks>This exception is typically used to indicate issues encountered while instantiating or
    /// initializing an object.</remarks>
    public class ObjectCreationException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an error occurs during object creation.
        /// </summary>
        /// <remarks>This exception is typically used to indicate a failure in constructing or
        /// initializing an object.</remarks>
        public ObjectCreationException()
            : base("An error occurred while creating the object.") { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs during object creation.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ObjectCreationException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ObjectCreationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
