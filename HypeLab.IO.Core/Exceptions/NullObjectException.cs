using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a null object is encountered in a context where it is not allowed.
    /// </summary>
    [DebuggerDisplay("NullObjectException: {Message}")]
    [Serializable]
    public class NullObjectException : Exception
    {
        private const string _defaultMessage = "The object cannot be null.";

        /// <summary>
        /// Represents an exception that is thrown when an object is null and a non-null value is required.
        /// </summary>
        public NullObjectException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an operation encounters a null object where a non-null object is
        /// required.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NullObjectException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an operation encounters a null object where a non-null object is
        /// required.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public NullObjectException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected NullObjectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
