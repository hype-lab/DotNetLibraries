namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents the result of an Excel parsing operation, including successfully parsed data and any errors
    /// encountered.
    /// </summary>
    /// <remarks>This class provides a way to encapsulate the outcome of an Excel parsing operation, including
    /// both the parsed data and the errors encountered during the process. Use the <see cref="ParsedData"/> property to
    /// access the successfully parsed items, and the <see cref="Errors"/> property to review any issues that
    /// occurred.</remarks>
    /// <typeparam name="T">The type of the parsed data items. Must be a reference type.</typeparam>
    public class ExcelParseResult<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelParseResult{T}"/> class, containing parsed data and
        /// associated errors.
        /// </summary>
        /// <param name="parsedData">The collection of successfully parsed data items.</param>
        /// <param name="errors">The collection of errors encountered during parsing.</param>
        public ExcelParseResult(IEnumerable<T> parsedData, IEnumerable<ExcelParseError>? errors)
        {
            ParsedData = parsedData;
            Errors = errors;
        }

        /// <summary>
        /// Gets or sets the parsed data resulting from the processing operation.
        /// </summary>
        public IEnumerable<T> ParsedData { get; set; }

        /// <summary>
        /// Gets or sets the collection of errors encountered during parsing.
        /// </summary>
        public IEnumerable<ExcelParseError>? Errors { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current object has any errors.
        /// </summary>
        public bool HasErrors => Errors?.Any() == true;
    }
}
