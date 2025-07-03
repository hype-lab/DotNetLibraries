using HypeLab.IO.Core.Data.Models.Excel;

namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    /// <summary>
    /// Represents configuration options for reading data from an Excel file.
    /// </summary>
    /// <remarks>This class provides various settings to control how data is read from an Excel file, such as
    /// specifying the sheet to read, handling missing sheets, and determining whether the first row contains headers.
    /// These options allow for flexible and customizable Excel data reading.</remarks>
    public class ExcelReaderOptions : IReaderOptions
    {
        /// <summary>
        /// Represents configuration options for reading Excel files.
        /// </summary>
        /// <remarks>Use this class to specify settings that control how Excel files are read, such as
        /// parsing behavior or supported formats. These options can be passed to an Excel reader to customize its
        /// behavior.</remarks>
        public ExcelReaderOptions() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelReaderOptions"/> class with the specified sheet name and
        /// behavior for missing sheets.
        /// </summary>
        /// <param name="sheetName">The name of the sheet to be read. Can be null to indicate that the default sheet should be used.</param>
        /// <param name="throwExceptionIfSheetNameNotFound">A value indicating whether an exception should be thrown if the specified sheet name is not found.  <see
        /// langword="true"/> to throw an exception; otherwise, <see langword="false"/>.</param>
        public ExcelReaderOptions(SheetName sheetName, bool throwExceptionIfSheetNameNotFound = true)
        {
            SheetName = sheetName;
            ThrowExceptionIfSheetNameNotFound = throwExceptionIfSheetNameNotFound;
        }

        /// <summary>
        /// Gets or sets the name of the sheet associated with the current object.
        /// </summary>
        public SheetName? SheetName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an exception should be thrown if a specified sheet name is not
        /// found.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, attempting to access a sheet by name that does
        /// not exist will result in an exception. When set to <see langword="false"/>, no exception will be thrown, and
        /// the behavior will depend on the implementation.</remarks>
        public bool ThrowExceptionIfSheetNameNotFound { get; set; }

        /// <summary>
        /// Whether to treat the first row as header (default: <see langword="true"/>)
        /// </summary>
        public bool HasHeaderRow { get; set; } = true;

        /// <summary>
        /// Zero-based index of the row that contains headers (default: 0)
        /// Ignored if <see cref="HasHeaderRow"/> is false.
        /// </summary>
        public int HeaderRowIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether validation is enabled. (default: <see langword="true"/>)
        /// </summary>
        public bool EnableValidation { get; set; } = true;
    }
}
