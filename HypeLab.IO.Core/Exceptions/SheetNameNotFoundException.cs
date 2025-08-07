using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specified sheet name cannot be found in an Excel file.
    /// </summary>
    [DebuggerDisplay("SheetNameNotFoundException: {Message}")]
    [Serializable]
    public class SheetNameNotFoundException : Exception
    {
        private const string _defaultMsg = "The specified sheet name was not found in the Excel file.";

        /// <summary>
        /// Represents an exception that is thrown when a specified sheet name cannot be found.
        /// </summary>
        public SheetNameNotFoundException()
            : base(_defaultMsg) { }

        /// <summary>
        /// Represents an exception that is thrown when a specified sheet name cannot be found.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. If null, a default message is used.</param>
        public SheetNameNotFoundException(string? message)
            : base(message ?? _defaultMsg) { }

        /// <summary>
        /// Represents an exception that is thrown when a specified sheet name cannot be found.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. If null, a default message is used.</param>
        /// <param name="innerException">The exception that caused the current exception, or null if no inner exception is specified.</param>
        public SheetNameNotFoundException(string? message, Exception? innerException)
            : base(message ?? _defaultMsg, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SheetNameNotFoundException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstitute the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data about the exception being
        /// thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected SheetNameNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
