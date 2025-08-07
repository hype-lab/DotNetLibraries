using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a row index exceeds the header index in a data structure or operation.
    /// </summary>
    [DebuggerDisplay("RowLargerThanHeaderException: {Message}")]
    [Serializable]
    public class RowLargerThanHeaderException : Exception
    {
        private const string _defaultMessage = "The row index is larger than the header index.";

        /// <summary>
        /// Represents an exception that is thrown when a row index exceeds the header index.
        /// </summary>
        public RowLargerThanHeaderException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a row exceeds the size of the header in a data structure or
        /// operation.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RowLargerThanHeaderException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a row exceeds the size or constraints of its header.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public RowLargerThanHeaderException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowLargerThanHeaderException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected RowLargerThanHeaderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
