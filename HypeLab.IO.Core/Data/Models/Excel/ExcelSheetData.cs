using HypeLab.IO.Core.Data.Models.Common;

namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents a data structure for managing tabular data, including headers, rows, and associated warnings.
    /// </summary>
    /// <remarks>The <see cref="ExcelSheetData"/> class provides functionality for working with tabular data, such
    /// as headers, rows,  and row-specific warnings. It is designed to facilitate operations like data validation,
    /// processing, and  mapping headers to their respective index positions. This class is particularly useful for
    /// scenarios involving  structured data, such as CSV files or spreadsheet-like data.</remarks>
    public class ExcelSheetData
    {
        /// <summary>
        /// Gets or sets the collection of headers associated with the request or response.
        /// </summary>
        /// <remarks>Use this property to access or modify the headers for the current operation. Ensure
        /// that header names and values conform to the expected format and standards for HTTP headers.</remarks>
        public List<string> Headers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of rows, where each row is represented as a list of strings.
        /// </summary>
        public List<List<string>> Rows { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of warnings associated with individual rows.
        /// </summary>
        /// <remarks>This property is typically used to track and review row-specific warnings generated
        /// during data processing or validation operations.</remarks>
        public List<RowWarning> RowWarnings { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether there are any row warnings.
        /// </summary>
        public bool HasRowWarnings
            => RowWarnings.Count > 0;

        /// <summary>
        /// Gets a dictionary that maps header names to their corresponding index positions.
        /// </summary>
        /// <remarks>The header names are compared using <see
        /// cref="StringComparer.OrdinalIgnoreCase"/>.</remarks>
        public Dictionary<string, int> HeaderIndexMap
            => Headers.Select((h, i) => new { h, i }).ToDictionary(x => x.h, x => x.i, StringComparer.OrdinalIgnoreCase);
    }
}
