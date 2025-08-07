using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a column index attribute is negative.
    /// </summary>
    [DebuggerDisplay("NegativeColumnIndexAttributeException: {Message}")]
    [Serializable]
    public class NegativeColumnIndexAttributeException : Exception
    {
        private const string _defaultMessage = "The column index attribute cannot be negative.";

        /// <summary>
        /// Represents an exception that is thrown when a column index attribute is negative.
        /// </summary>
        public NegativeColumnIndexAttributeException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is negative in a context where it must be
        /// non-negative.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NegativeColumnIndexAttributeException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a column index is negative.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public NegativeColumnIndexAttributeException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeColumnIndexAttributeException"/> class with serialized
        /// data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected NegativeColumnIndexAttributeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
