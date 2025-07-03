namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents an error encountered during the parsing of Excel data, including details about the column, row, and
    /// error message.
    /// </summary>
    /// <remarks>This class is typically used to capture and report errors that occur while processing Excel
    /// files. Each instance provides information about the specific column and row where the error occurred, along with
    /// a descriptive error message.</remarks>
    public class ExcelParseError
    {
        /// <summary>
        /// Gets or sets the name of the column in the database or data source.
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets the error message associated with the current operation or state.
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the zero-based index of the row.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelParseError"/> class with the specified column name, error
        /// message, and row index.
        /// </summary>
        /// <param name="columnName">The name of the column where the error occurred. Cannot be null or empty.</param>
        /// <param name="errorMessage">The error message describing the issue. Cannot be null or empty.</param>
        /// <param name="rowIndex">The index of the row where the error occurred. Must be zero or greater.</param>
        public ExcelParseError(string columnName, string errorMessage, int rowIndex)
        {
            ColumnName = columnName;
            ErrorMessage = errorMessage;
            RowIndex = rowIndex;
        }
    }
}
