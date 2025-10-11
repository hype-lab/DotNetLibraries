using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when multiple columns in an Excel sheet are mapped to the same index,
    /// resulting in ambiguity during parsing.
    /// </summary>
    [DebuggerDisplay("AmbiguousHeaderIndexException: {Message}")]
    [Serializable]
    public class AmbiguousHeaderIndexException : Exception
    {
        private const string _defaultMessage = "An error occurred due to ambiguous header indices in the Excel sheet data. This could happens when multiple columns are mapped to the same index, causing confusion during parsing.";

        /// <summary>
        /// Represents an exception that is thrown when multiple columns in an Excel sheet are mapped to the same index,
        /// resulting in ambiguity during parsing.
        /// </summary>
        public AmbiguousHeaderIndexException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbiguousHeaderIndexException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AmbiguousHeaderIndexException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbiguousHeaderIndexException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AmbiguousHeaderIndexException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbiguousHeaderIndexException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected AmbiguousHeaderIndexException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
