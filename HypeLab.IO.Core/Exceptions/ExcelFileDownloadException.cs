using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during the download of an Excel file.
    /// </summary>
    [DebuggerDisplay("ExcelFileDownloadException: {Message}")]
    [Serializable]
    public class ExcelFileDownloadException : Exception
    {
        private const string _defaultMessage = "An error occurred while downloading the Excel file.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a default error message.
        /// </summary>
        public ExcelFileDownloadException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ExcelFileDownloadException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public ExcelFileDownloadException(string? message, Exception? innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstitute the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data about the exception being
        /// thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ExcelFileDownloadException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
