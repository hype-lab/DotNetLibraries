namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during the download of an Excel file.
    /// </summary>
    /// <remarks>This exception is typically used to indicate issues specific to the process of downloading
    /// Excel files, such as network errors, file access issues, or invalid file formats. It provides constructors for
    /// specifying a custom error message and an optional inner exception for additional context.</remarks>
    public class ExcelFileDownloadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a default error message.
        /// </summary>
        public ExcelFileDownloadException()
            : base("An error occurred while downloading the Excel file.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ExcelFileDownloadException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileDownloadException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public ExcelFileDownloadException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
