using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Helpers.Const;
using HypeLab.IO.Core.Helpers.Excel;
using System.Globalization;

namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    /// <summary>
    /// Represents the configuration options for writing data to an Excel file.
    /// </summary>
    /// <remarks>This class provides various settings to customize the behavior of Excel file generation, 
    /// including options for boolean value representation, shared string usage, cell styling,  worksheet naming, and
    /// culture-specific formatting. Use these options to tailor the output  to your specific requirements.</remarks>
    public class ExcelWriterOptions : IWriterOptions
    {
        /// <summary>
        /// Gets or sets the words used to represent boolean values.
        /// </summary>
        public TrueFalseWords? TrueFalseWords { get; set; }
        /// <summary>
        /// Gets a value indicating whether the <see cref="TrueFalseWords"/> object contains valid non-empty values for
        /// both the true and false words.
        /// </summary>
        public bool HasTrueFalseWords => TrueFalseWords != null && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.TrueWord) && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.FalseWord);

        /// <summary>
        /// Gets or sets the collection of multiple-choice words where each word can have a true or false association.
        /// </summary>
        public MultipleTrueFalseWords? MultipleTrueFalseWords { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether shared strings should be used. (default: <see langword="true"/>)
        /// </summary>
        public bool UseSharedStrings { get; set; } = true;

        /// <summary>
        /// Gets or sets the style selector used to determine the styles applied to Excel cells.
        /// </summary>
        public IExcelStyleSelector? StyleSelector { get; set; }

        /// <summary>
        /// Gets or sets the style options for formatting Excel cells.
        /// </summary>
        public ExcelStyleOptions StyleOptions { get; set; } = ExcelStyleOptions.Empty;

        /// <summary>
        /// Gets or sets the name of the worksheet.
        /// </summary>
        public SheetName SheetName { get; set; } = ExcelDefaults.Worksheets.Sheet1;

        /// <summary>
        /// Gets or sets the <see cref="CultureInfo"/> to use for value formatting (e.g., numbers and dates).
        /// Defaults to <see cref="CultureInfo.InvariantCulture"/>. You may override it for localized Excel output.
        /// </summary>
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    }
}
