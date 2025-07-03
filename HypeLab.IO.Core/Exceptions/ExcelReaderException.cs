namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the reading of an Excel file.
    /// </summary>
    /// <remarks>This exception is typically thrown when an issue is encountered while processing an Excel
    /// file, such as invalid file format, corrupted data, or other unexpected conditions during reading.</remarks>
    public class ExcelReaderException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an error occurs while reading an Excel file.
        /// </summary>
        public ExcelReaderException()
            : base("An error occurred while reading the Excel file.") { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs while reading an Excel file.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExcelReaderException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that occurs during the processing of Excel files.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ExcelReaderException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
