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
        public string[] Headers { get; set; } = [];

        public List<string[]> Rows { get; set; } = [];

        public List<RowWarning> RowWarnings { get; set; } = [];

        public bool HasRowWarnings
            => RowWarnings.Any();

        public Dictionary<string, int> HeaderIndexMap
            => Headers.Select((h, i) => new { h, i }).ToDictionary(x => x.h, x => x.i, StringComparer.OrdinalIgnoreCase);
    }
}
