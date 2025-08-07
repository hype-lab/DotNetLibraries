using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during parsing.
    /// </summary>
    [DebuggerDisplay("ParseErrorException: {Message}")]
    [Serializable]
    public class ParseErrorException : Exception
    {
        private const string _defaultMessage = "An error occurred during parsing.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a default error message.
        /// </summary>
        public ParseErrorException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParseErrorException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with a specified error message and
        /// a reference to the inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ParseErrorException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseErrorException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ParseErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
