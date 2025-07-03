namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur during the parsing of an Excel file.
    /// </summary>
    /// <remarks>This exception is typically thrown when an issue is encountered while processing or
    /// interpreting the contents of an Excel file. It provides additional context about the error through its message
    /// and optional inner exception.</remarks>
    public class ExcelParserException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an error occurs while parsing an Excel file.
        /// </summary>
        /// <remarks>This exception is typically used to indicate issues specific to the parsing of Excel
        /// files, such as invalid file formats or unexpected data structures.</remarks>
        public ExcelParserException()
            : base("An error occurred while parsing the Excel file.") { }

        /// <summary>
        /// Represents an exception that is thrown when an error occurs during Excel file parsing.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExcelParserException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelParserException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ExcelParserException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
