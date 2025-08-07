using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a reference to an invalid or non-existent cell is encountered in a spreadsheet
    /// or table.
    /// </summary>
    [DebuggerDisplay("InvalidCellReferenceException: {Message}")]
    [Serializable]
    public class InvalidCellReferenceException : Exception
    {
        private const string _defaultMessage = "The cell reference is invalid or does not exist.";

        /// <summary>
        /// Represents an exception that is thrown when a reference to an invalid or non-existent cell is encountered.
        /// </summary>
        public InvalidCellReferenceException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a cell reference in a spreadsheet or table is invalid.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidCellReferenceException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a reference to an invalid or non-existent cell is encountered.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidCellReferenceException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCellReferenceException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected InvalidCellReferenceException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
