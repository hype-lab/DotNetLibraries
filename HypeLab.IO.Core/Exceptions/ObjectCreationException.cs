using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during object creation.
    /// </summary>
    [DebuggerDisplay("ObjectCreationException: {Message}")]
    [Serializable]
    public class ObjectCreationException : Exception
    {
        private const string _defaultMessage = "An error occurred while creating the object.";

        /// <summary>
        /// Represents an exception that is thrown when an error occurs during object creation.
        /// </summary>
        public ObjectCreationException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs during object creation.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ObjectCreationException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ObjectCreationException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectCreationException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ObjectCreationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
