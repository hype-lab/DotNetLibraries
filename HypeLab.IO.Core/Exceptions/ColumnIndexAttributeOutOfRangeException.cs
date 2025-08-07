using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a column index attribute is outside the valid range.
    /// </summary>
    [DebuggerDisplay("ColumnIndexAttributeOutOfRangeException: {Message}")]
    [Serializable]
    public class ColumnIndexAttributeOutOfRangeException : Exception
    {
        private const string _defaultMessage = "The specified column index attribute is out of range.";

        /// <summary>
        /// Represents an exception that is thrown when a specified column index attribute is out of the valid range.
        /// </summary>
        public ColumnIndexAttributeOutOfRangeException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is out of the valid range.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ColumnIndexAttributeOutOfRangeException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is out of the valid range.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ColumnIndexAttributeOutOfRangeException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnIndexAttributeOutOfRangeException"/> class with
        /// serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ColumnIndexAttributeOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
