using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the reading of an Excel file.
    /// </summary>
    [DebuggerDisplay("ExcelReaderException: {Message}")]
    [Serializable]
    public class ExcelReaderException : Exception
    {
        private const string _defaultMessage = "An error occurred while reading the Excel file.";

        /// <summary>
        /// Represents an exception that is thrown when an error occurs while reading an Excel file.
        /// </summary>
        public ExcelReaderException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs while reading an Excel file.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExcelReaderException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that occurs during the processing of Excel files.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ExcelReaderException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelReaderException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ExcelReaderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
