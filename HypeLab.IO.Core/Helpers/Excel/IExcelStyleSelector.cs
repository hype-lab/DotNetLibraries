using HypeLab.IO.Core.Data.Models.Excel;
using System.Reflection;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Defines a mechanism for selecting an Excel cell style based on the row, column, property, and value of a cell.
    /// </summary>
    /// <remarks>Implement this interface to provide custom logic for determining the style of an Excel cell
    /// during export or rendering.</remarks>
    public interface IExcelStyleSelector
    {
        /// <summary>
        /// Selects an appropriate <see cref="ExcelCellStyle"/> for a given cell based on its row, column, property, and
        /// value.
        /// </summary>
        /// <remarks>This method allows customization of cell styles in an Excel export based on
        /// contextual information such as the cell's position, the property it represents, and its value.
        /// Implementations may use this information to apply conditional formatting or other styling rules.</remarks>
        /// <param name="rowIndex">The zero-based index of the row containing the cell.</param>
        /// <param name="columnIndex">The zero-based index of the column containing the cell.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> associated with the cell's data, representing the property of the object
        /// being rendered.</param>
        /// <param name="value">The value of the cell, which may influence the selected style. Can be <see langword="null"/>.</param>
        /// <returns>An <see cref="ExcelCellStyle"/> instance representing the style to apply to the cell, or <see
        /// langword="null"/> if no specific style is selected.</returns>
        ExcelCellStyle? SelectStyle(int rowIndex, int columnIndex, PropertyInfo property, object? value);
    }
}
