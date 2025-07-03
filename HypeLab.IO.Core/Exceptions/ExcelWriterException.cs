namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the process of writing an Excel file.
    /// </summary>
    /// <remarks>This exception is typically thrown when an operation in the Excel writing process fails, 
    /// such as invalid data, file access issues, or other unexpected conditions.  Use the <see
    /// cref="ExcelWriterException"/> to identify and handle errors specific to Excel file generation.</remarks>
    public class ExcelWriterException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an error occurs while writing an Excel file.
        /// </summary>
        /// <remarks>This exception is typically used to indicate issues specific to the process of
        /// writing data to an Excel file, such as file access errors or invalid data formats.</remarks>
        public ExcelWriterException()
            : base("An error occurred while writing the Excel file.") { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs while writing to an Excel file.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExcelWriterException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that occurs during operations performed by the Excel writer.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ExcelWriterException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
