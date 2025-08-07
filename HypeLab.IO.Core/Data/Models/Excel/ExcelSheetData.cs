using HypeLab.IO.Core.Data.Models.Common;

namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents the data structure of an Excel sheet, including headers, rows, and associated warnings.
    /// </summary>
    /// <remarks>This class provides a convenient  'raw' way to manage and access Excel sheet data, including header
    /// mappings, row data, and warnings associated with specific rows.</remarks>
    public class ExcelSheetData
    {
        /// <summary>
        /// Gets or sets the collection of header names.
        /// </summary>
        public string?[] Headers { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of rows, where each row is represented as an array of strings.
        /// </summary>
        public List<string?[]> Rows { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of warnings associated with individual rows.
        /// </summary>
        public List<RowWarning> RowWarnings { get; set; } = [];

        /// <summary>
        /// Gets a value indicating whether any row warnings are present.
        /// </summary>
        public bool HasRowWarnings
            => RowWarnings.Any();

        /// <summary>
        /// Gets a dictionary that maps header names to their corresponding index positions.
        /// </summary>
        public Dictionary<string?, int> HeaderIndexMap
            => Headers.Where(x => !string.IsNullOrEmpty(x)).Select((h, i) => new { h, i }).ToDictionary(x => x.h, x => x.i, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a value indicating whether any header in the collection has a null or empty value.
        /// </summary>
        public bool HasNullHeaderValues => Headers.Any(string.IsNullOrEmpty);
    }
}
