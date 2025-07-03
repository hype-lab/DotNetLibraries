namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specified column is not found in the data source.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that an operation requiring a specific column
    /// could not proceed because the column does not exist in the provided data source.</remarks>
    public class ColumnNotFoundException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that an operation attempted to access a
        /// column that does not exist in the provided data source. Ensure that the column name or identifier being
        /// referenced is correct and matches the data source schema.</remarks>
        public ColumnNotFoundException()
            : base("The specified column was not found in the data source.") { }

        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        /// <param name="columnName">The name of the column that could not be found.</param>
        public ColumnNotFoundException(string columnName)
            : base($"The column '{columnName}' was not found in the data source.") { }

        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        /// <param name="columnName">The name of the column that was not found.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ColumnNotFoundException(string columnName, Exception innerException)
            : base($"The column '{columnName}' was not found in the data source.", innerException) { }
    }
}
