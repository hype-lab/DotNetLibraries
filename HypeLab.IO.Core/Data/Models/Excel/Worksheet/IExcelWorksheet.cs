using HypeLab.IO.Core.Data.Options.Impl.Excel;
using System.Collections;

namespace HypeLab.IO.Core.Data.Models.Excel.Worksheet
{
    /// <summary>
    /// Represents an Excel worksheet, providing access to its name, options, data, and associated model type.
    /// </summary>
    /// <remarks>This interface is designed to facilitate interaction with an Excel worksheet, including
    /// retrieving data, configuring options, and identifying the underlying data model type.</remarks>
    public interface IExcelWorksheet
    {
        /// <summary>
        /// Gets or sets the name of the worksheet.
        /// </summary>
        SheetName Name { get; set; } // the name of the worksheet
        /// <summary>
        /// Gets or sets the options for configuring the behavior of the Excel worksheet.
        /// </summary>
        ExcelWorksheetOptions? Options { get; set; } // options for writing the sheet, can be null to use default options
        /// <summary>
        /// Gets the type of the model associated with this instance.
        /// </summary>
        Type ModelType { get; }
        /// <summary>
        /// Retrieves a collection of data items.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerable"/> containing the data items. The collection may be empty if no
        /// data is available.</returns>
        IEnumerable GetData(); // <- non generico
    }
}
