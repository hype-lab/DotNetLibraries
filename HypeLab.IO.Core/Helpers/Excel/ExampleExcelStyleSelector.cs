using HypeLab.IO.Core.Data.Models.Excel;
using System.Reflection;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides logic to select an appropriate style for cells in an Excel sheet based on their row, column, property,
    /// and value.
    /// </summary>
    /// <remarks>This class implements the <see cref="IExcelStyleSelector"/> interface to apply conditional
    /// styling to Excel cells. The following styles are applied: <list type="bullet"> <item><description>For the first
    /// row (header), the style includes bold text, white font color, and a blue background.</description></item>
    /// <item><description>For even-numbered rows (zebra striping), the style includes a light gray
    /// background.</description></item> <item><description>For cells containing negative numeric values, the style
    /// includes a red font color.</description></item> <item><description>For all other cases, no style is applied, and
    /// the method returns <see langword="null"/>.</description></item> </list></remarks>
    public class ExampleExcelStyleSelector : IExcelStyleSelector
    {
        /// <summary>
        /// Selects an appropriate style for a cell in an Excel sheet based on its row, column, property, and value.
        /// </summary>
        /// <remarks>The method applies the following styles: <list type="bullet"> <item><description>For
        /// the first row (header), the style includes bold text, white font color, and a blue
        /// background.</description></item> <item><description>For even-numbered rows (zebra striping), the style
        /// includes a light gray background.</description></item> <item><description>For cells containing negative
        /// numeric values, the style includes a red font color.</description></item> <item><description>For all other
        /// cases, no style is applied, and the method returns <see langword="null"/>.</description></item>
        /// </list></remarks>
        /// <param name="rowIndex">The zero-based index of the row containing the cell.</param>
        /// <param name="columnIndex">The zero-based index of the column containing the cell.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> associated with the cell's data, if applicable.</param>
        /// <param name="value">The value of the cell, which may influence the selected style.</param>
        /// <returns>An <see cref="ExcelCellStyle"/> object representing the style to apply to the cell, or <see
        /// langword="null"/> if no style is applied.</returns>
        public ExcelCellStyle? SelectStyle(int rowIndex, int columnIndex, PropertyInfo property, object? value)
        {
            // Header (prima riga)
            if (rowIndex == 1)
            {
                return new ExcelCellStyle
                {
                    Bold = true,
                    FontColorHex = "#FFFFFF",
                    BackgroundColorHex = "#007ACC"
                };
            }

            // Zebra stripe
            if (rowIndex % 2 == 0)
            {
                return new ExcelCellStyle
                {
                    BackgroundColorHex = "#F3F3F3"
                };
            }

            // Colorazione condizionale: numeri negativi in rosso
            if (value is IConvertible convertible && double.TryParse(convertible.ToString(), out double number) && number < 0)
            {
                return new ExcelCellStyle
                {
                    FontColorHex = "#FF0000"
                };
            }

            // Default (nessuno stile)
            return null;
        }
    }
}
