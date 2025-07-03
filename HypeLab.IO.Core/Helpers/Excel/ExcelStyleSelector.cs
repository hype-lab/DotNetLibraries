using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides functionality to generate an <see cref="ExcelCellStyle"/> based on the specified row index and style
    /// options.
    /// </summary>
    /// <remarks>This class is designed to assist in applying consistent styles to Excel cells based on
    /// row-specific or header-specific configurations provided in an <see cref="ExcelStyleOptions"/> object. The method
    /// determines the appropriate style by evaluating the row index and the options provided.</remarks>
    public static class ExcelStyleSelector
    {
        /// <summary>
        /// Creates an <see cref="ExcelCellStyle"/> instance based on the specified row index and style options.
        /// </summary>
        /// <remarks>This method applies different styles based on the row index and the provided options:
        /// <list type="bullet"> <item> <description> If the row index corresponds to the header row (rowIndex == 1) and
        /// header-specific styles are defined, the method returns a style configured for the header. </description>
        /// </item> <item> <description> For non-header rows, the method applies alternating row background colors if
        /// specified in <see cref="ExcelStyleOptions.RowBackgroundColors"/>. </description> </item> </list> If no
        /// specific style is applicable, the method returns <see langword="null"/>.</remarks>
        /// <param name="rowIndex">The zero-based index of the row for which the style is being generated. This determines how row-specific
        /// styles, such as alternating row colors, are applied.</param>
        /// <param name="options">The <see cref="ExcelStyleOptions"/> object containing style configuration for headers and rows. If <paramref
        /// name="options"/> is <see langword="null"/>, the method returns <see langword="null"/>.</param>
        /// <returns>An <see cref="ExcelCellStyle"/> instance representing the style for the specified row, or <see
        /// langword="null"/> if <paramref name="options"/> is <see langword="null"/> or no applicable style is defined.</returns>
        public static ExcelCellStyle? FromOptions(int rowIndex, ExcelStyleOptions? options)
        {
            if (options is null) // Use 'is null' to avoid CS8604 and CS8625
                return null;

            if (rowIndex == 1 && !string.IsNullOrEmpty(options.HeaderBackgroundColor))
            {
                return new ExcelCellStyle
                {
                    BackgroundColorHex = options.HeaderBackgroundColor,
                    FontColorHex = options.HeaderFontColor,
                    Bold = options.IsHeaderFontBold,
                    FontName = options.HeaderFontName ?? options.RowFontName,
                    BorderColorHex = options.HeaderBorderColor,
                    IntegerNumberFormat = options.IntegerNumberFormat,
                    DateFormat = options.DateFormat,
                    FontSize = options.HeaderFontSize > 0 ? options.HeaderFontSize : options.RowFontSize,
                };
            }

            if (options.RowBackgroundColors?.Length > 0)
            {
                string rowColor = options.RowBackgroundColors[rowIndex % options.RowBackgroundColors.Length];
                return new ExcelCellStyle
                {
                    BackgroundColorHex = rowColor,
                    FontColorHex = options.RowFontName == null ? null : options.HeaderFontColor,
                    Bold = options.IsRowFontBold,
                    FontName = options.RowFontName,
                    BorderColorHex = options.RowBorderColor,
                    IntegerNumberFormat = options.IntegerNumberFormat,
                    DateFormat = options.DateFormat,
                    FontSize = options.RowFontSize > 0 ? options.RowFontSize : options.HeaderFontSize
                };
            }

            return null;
        }
    }
}
