namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a warning associated with a specific row in a dataset or table.
    /// </summary>
    /// <remarks>This class encapsulates information about a warning, including the zero-based index of the
    /// row where the warning occurred and a descriptive message providing details about the issue. It is commonly used
    /// to track and report issues in data processing scenarios, such as validation errors or inconsistencies in tabular
    /// data.</remarks>
    public class RowWarning
    {
        /// <summary>
        /// Gets the zero-based index of the row associated with this instance.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Gets the message associated with the current operation or event.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Represents a warning associated with a specific row in a dataset or table.
        /// </summary>
        /// <param name="rowIndex">The zero-based index of the row where the warning occurred.</param>
        /// <param name="message">A descriptive message providing details about the warning.</param>
        public RowWarning(int rowIndex, string message)
        {
            RowIndex = rowIndex;
            Message = message;
        }

        /// <summary>
        /// Returns a string representation of the object, including the row index and message.
        /// </summary>
        /// <returns>A string in the format "Row {RowIndex}: {Message}", where <c>RowIndex</c> is the index of the row and
        /// <c>Message</c> is the associated message.</returns>
        public override string ToString() => $"Row {RowIndex}: {Message}";
    }
}
