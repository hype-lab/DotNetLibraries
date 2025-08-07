using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the process of writing an Excel file.
    /// </summary>
    [DebuggerDisplay("ExcelWriterException: {Message}")]
    [Serializable]
    public class ExcelWriterException : Exception
    {
        private const string _defaultMessage = "An error occurred while writing the Excel file.";
        /// <summary>
        /// Represents an exception that is thrown when an error occurs while writing an Excel file.
        /// </summary>
        public ExcelWriterException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs while writing to an Excel file.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExcelWriterException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that occurs during operations performed by the Excel writer.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ExcelWriterException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWriterException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstruct the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ExcelWriterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
