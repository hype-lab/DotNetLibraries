using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers.Const;
using HypeLab.IO.Core.Helpers.Excel;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Xml;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides functionality for reading rows from an Excel file in a streaming manner.
    /// </summary>
    /// <remarks>The <see cref="ExcelRowReader"/> class is designed to read rows from an Excel file
    /// efficiently by processing the file's XML structure. It supports reading shared strings, handling column
    /// indexing, and buffering row data. This class implements the <see cref="IExcelRowReader"/> interface, allowing
    /// users to access rows as arrays of nullable strings and iterate through the rows sequentially.  Instances of this
    /// class are created using the <see cref="Open(Stream, ExcelStreamingOptions?, ILogger?)"/> method, which initializes
    /// the reader with the specified input stream and options. The class also provides a static method, <see
    /// cref="ReadRows(Stream, ExcelStreamingOptions?, ILogger?)"/>, for enumerating rows directly.  The class is
    /// disposable and should be properly disposed of after use to release resources such as streams and XML
    /// readers.</remarks>
    public class ExcelRowReader : IExcelRowReader
    {
        private readonly ZipArchive _zipArchive;
        private readonly Stream _sheetStream;
        private readonly XmlReader _xmlReader;
        private readonly ExcelStreamingOptions _excelStreamingOptions;
        private readonly List<string>? _sharedStrings;

        private BoxableRowBuffer _rowBuffer;

        private int _rowIndex = -1; // Initialize to -1 to indicate no rows read yet
        private const int _maxExpectedColumns = 64;
        private string?[]? _currentRow;
        private bool _isHeader;

        private ExcelRowReader(ZipArchive zipArchive, Stream sheetStream, XmlReader reader, ExcelStreamingOptions excelReaderOptions, List<string>? sharedStrings = null)
        {
            _zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
            _sheetStream = sheetStream ?? throw new ArgumentNullException(nameof(sheetStream));
            _xmlReader = reader ?? throw new ArgumentNullException(nameof(reader));
            _sharedStrings = sharedStrings;
            _rowBuffer = new BoxableRowBuffer(_maxExpectedColumns);
            _excelStreamingOptions = excelReaderOptions ?? new ExcelStreamingOptions();
        }
        #region IExcelRowReader impl
        /// <summary>
        /// Gets the current row of data as an array of nullable strings.
        /// </summary>
        public string?[] Current => _currentRow ?? throw new InvalidOperationException("No row read. Call ReadNext() first.");

        /// <summary>
        /// Gets the zero-based index of the row.
        /// </summary>
        public int RowIndex => _rowIndex;

        /// <summary>
        /// Gets the number of columns in the current row buffer.
        /// </summary>
        public int ColumnCount => _rowBuffer.Count;

        /// <summary>
        /// Gets a value indicating whether the current row is a header row.
        /// </summary>
        public bool IsHeader => _isHeader; // consider the first row as header

        /// <summary>
        /// Opens an Excel file from the specified stream and initializes a row reader for reading its contents.
        /// </summary>
        /// <remarks>This method reads the Excel file in the provided stream and resolves the target sheet
        /// based on the  <paramref name="options"/>. If no sheet name is specified, the default sheet is used. The
        /// method supports  logging diagnostic information if a <paramref name="logger"/> is provided.</remarks>
        /// <param name="input">The input stream containing the Excel file. The stream must be readable.</param>
        /// <param name="options">Optional settings for configuring the Excel reader, such as the target sheet name and error handling
        /// behavior. If not provided, default options are used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or debug information during the reading process.</param>
        /// <returns>An <see cref="IExcelRowReader"/> instance for reading rows from the specified Excel file.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> is null or not readable.</exception>
        /// <exception cref="SheetNameNotFoundException">Thrown if the specified sheet name in <paramref name="options"/> is not found in the workbook and  <see
        /// cref="ExcelStreamingOptions.ThrowExceptionIfSheetNameNotFound"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the resolved sheet file is not found in the Excel file.</exception>
        public static IExcelRowReader Open(Stream input, ExcelStreamingOptions? options = null, ILogger? logger = null)
        {
            if (input?.CanRead != true)
                throw new ArgumentException("Input stream must be readable.", nameof(input));

            options ??= new ExcelStreamingOptions();

            ZipArchive archive = new(input, ZipArchiveMode.Read, leaveOpen: true);
            string? sheetPath = ExcelReaderHelper.ResolveSheetFilePath(archive, options.SheetName, logger);
            if (sheetPath is null)
            {
                if (options.ThrowExceptionIfSheetNameNotFound)
                    throw new SheetNameNotFoundException($"Sheet '{options.SheetName}' not found in workbook.");
                else
                    sheetPath = ExcelDefaults.Worksheets.Sheet1PathName; // fallback
            }

            ZipArchiveEntry sheetEntry = archive.GetEntry(sheetPath)
                ?? throw new FileNotFoundException($"Sheet file '{sheetPath}' not found in the Excel file.", sheetPath);

            Stream sheetStream = sheetEntry.Open();
            XmlReaderSettings settings = new()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                DtdProcessing = DtdProcessing.Ignore,
                Async = false
            };

            XmlReader xmlReader = XmlReader.Create(sheetStream, settings);
            List<string>? sharedStrings = ExcelReaderHelper.LoadSharedStrings(archive);

            return new ExcelRowReader(archive, sheetStream, xmlReader, options, sharedStrings);
        }

        /// <summary>
        /// Opens an Excel file for reading and returns an <see cref="IExcelRowReader"/> to read its contents.
        /// </summary>
        /// <param name="filePath">The full path to the Excel file to open. The file must exist and be accessible.</param>
        /// <param name="options">Optional settings to configure the behavior of the reader. If <see langword="null"/>, default options are
        /// used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during the operation. If <see
        /// langword="null"/>, no logging is performed.</param>
        /// <returns>An <see cref="IExcelRowReader"/> instance for reading rows from the specified Excel file.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath"/> is <see langword="null"/>, empty, or consists only of whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the file specified by <paramref name="filePath"/> does not exist.</exception>
        public static IExcelRowReader Open(string filePath, ExcelStreamingOptions? options = null, ILogger? logger = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The specified file does not exist: {filePath}", filePath);

            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Open(fileStream, options, logger);
        }

        /// <summary>
        /// Opens an Excel file from the specified byte array and returns a reader for accessing its rows.
        /// </summary>
        /// <param name="fileBytes">The byte array containing the Excel file data. Must not be null or empty.</param>
        /// <param name="options">Optional configuration options for customizing the behavior of the reader. Can be null.</param>
        /// <param name="logger">Optional logger for capturing diagnostic or error information. Can be null.</param>
        /// <returns>An <see cref="IExcelRowReader"/> instance for reading rows from the Excel file.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="fileBytes"/> is null or empty.</exception>
        public static IExcelRowReader Open(byte[] fileBytes, ExcelStreamingOptions? options = null, ILogger? logger = null)
        {
            if (fileBytes is null || fileBytes.Length == 0)
                throw new ArgumentException("File bytes cannot be null or empty.", nameof(fileBytes));

            using MemoryStream memoryStream = new(fileBytes, writable: false);
            return Open(memoryStream, options, logger);
        }

        /// <summary>
        /// Reads the next row from the XML data source and populates the internal row buffer.
        /// </summary>
        /// <remarks>This method processes XML data to extract row and cell information. It reads the XML
        /// structure to identify rows and their corresponding cells, handling shared string references and column
        /// indexing. The method updates the internal row buffer with the parsed cell values and advances the row
        /// index.</remarks>
        /// <returns><see langword="true"/> if a row was successfully read and processed; otherwise, <see langword="false"/> if
        /// there are no more rows to read.</returns>
        public bool ReadNext()
        {
            _rowBuffer.Reset();
            _isHeader = _excelStreamingOptions.HasHeaderRow && _excelStreamingOptions.HeaderRowIndex == (_rowIndex + 1);

            while (_xmlReader.Read())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Name == "row")
                {
                    _rowIndex++;
                    int fallbackIndex = 0;

                    using XmlReader rowReader = _xmlReader.ReadSubtree();
                    while (rowReader.Read())
                    {
                        if (rowReader.NodeType == XmlNodeType.Element && rowReader.Name == "c")
                        {
                            string? cellRef = rowReader.GetAttribute("r");
                            string? cellType = rowReader.GetAttribute("t");

                            int colIndex;
                            int index = ExcelParserHelper.ParseColumnIndex(cellRef);
                            if (index >= 0)
                            {
                                colIndex = index;
                                fallbackIndex = index + 1; // sincronizza fallback
                            }
                            else
                            {
                                colIndex = fallbackIndex++;
                            }


                            string? finalValue = null;
                            while (rowReader.Read())
                            {
                                if (rowReader.NodeType == XmlNodeType.Element && rowReader.Name == "v")
                                {
                                    finalValue = rowReader.ReadElementContentAsString();
                                    break;
                                }
                            }

                            if (!string.IsNullOrEmpty(finalValue) && cellType == "s" && _sharedStrings is { Count: > 0 })
                            {
                                if (int.TryParse(finalValue, out int stringIndex) && stringIndex >= 0 && stringIndex < _sharedStrings.Count)
                                    finalValue = _sharedStrings[stringIndex];
                                else
                                    finalValue = null; // gestione fallback
                            }

                            _rowBuffer.Set(colIndex, finalValue ?? string.Empty);
                        }
                    }

                    // restituisco la riga corrente subito dopo aver letto le celle, NON dentro while rowReader.Read()
                    _currentRow = _rowBuffer.ToArray();
                    return true;
                }
            }

            return false;
        }
        #endregion

        /// <summary>
        /// Reads rows from an Excel file stream and returns them as a sequence of string arrays.
        /// </summary>
        /// <remarks>The method reads rows lazily, meaning rows are only read from the stream as the
        /// enumerable is iterated. Ensure the input stream remains open and readable for the duration of the
        /// enumeration.</remarks>
        /// <param name="input">The input stream containing the Excel file. The stream must be readable.</param>
        /// <param name="options">Optional settings to configure the behavior of the Excel reader. If null, default options are used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during the reading process. If null, no
        /// logging is performed.</param>
        /// <returns>An enumerable sequence of rows, where each row is represented as an array of nullable strings.  Each array
        /// corresponds to the values in a single row of the Excel file.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> is null or not readable.</exception>
        public static IEnumerable<string?[]> ReadRows(Stream input, ExcelStreamingOptions? options = null, ILogger? logger = null)
        {
            if (input?.CanRead != true)
                throw new ArgumentException("Input stream must be readable.", nameof(input));

            return AsEnumerable();

            IEnumerable<string?[]> AsEnumerable()
            {
                using IExcelRowReader reader = Open(input, options, logger);
                while (reader.ReadNext())
                {
                    yield return reader.Current;
                }
            }
        }

        #region disposing
        private bool _disposed = false;

        /// <summary>
        /// Releases the resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources used by the current instance of the class.
        /// </summary>
        /// <param name="disposing">Determines whether the method is called from the Dispose method or from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // managed resources here
                _rowBuffer.Return();
                _xmlReader?.Dispose();
                _sheetStream?.Dispose();
                _zipArchive?.Dispose();
            }

            // unmanaged resources here

            _disposed = true;
        }

        /// <summary>
        /// Finalizes the <see cref="ExcelRowReader"/> instance by releasing unmanaged resources.
        /// </summary>
        /// <remarks>This destructor ensures that unmanaged resources are released when the object is
        /// finalized by the garbage collector. It calls the <see cref="Dispose(bool)"/> method with <see
        /// langword="false"/> to indicate that the object is being finalized and not disposed explicitly.</remarks>
        ~ExcelRowReader()
        {
            Dispose(false);
        }
        #endregion
    }
}
