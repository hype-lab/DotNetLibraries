using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Helpers.Const;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides helper methods for reading and processing Excel files in OpenXML format.
    /// </summary>
    /// <remarks>The <see cref="ExcelReaderHelper"/> class includes methods for extracting shared strings,
    /// converting Excel files into structured data, and resolving file paths for specific sheets.  It is designed to
    /// work with Excel files provided as byte arrays or <see cref="ZipArchive"/> objects.</remarks>
    public static class ExcelReaderHelper
    {
        private const int _maxExpectedColumns = 64; // Default number of columns to expect if not specified

        /// <summary>
        /// Loads the shared strings from the specified Excel workbook archive.
        /// </summary>
        /// <remarks>Shared strings are typically used in Excel files to store text values in a
        /// centralized location,  allowing for efficient reuse across multiple cells. This method reads the shared
        /// strings file  (if present) and parses its contents into a list of strings.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> representing the Excel workbook from which to load shared strings.</param>
        /// <returns>A list of shared strings extracted from the workbook. Returns an empty list if the shared strings file is
        /// not present in the archive.</returns>
        public static List<string> LoadSharedStrings(ZipArchive archive)
        {
            ZipArchiveEntry? entry = archive.GetEntry(ExcelDefaults.SharedStringsFileName);

            if (entry == null)
                return [];

            List<string> sharedStrings = [];
            using XmlReader reader = XmlReader.Create(entry.Open());
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "si")
                {
                    StringBuilder currentText = new();

                    using (XmlReader subtree = reader.ReadSubtree())
                    {
                        while (subtree.Read())
                        {
                            if (subtree.NodeType == XmlNodeType.Element && subtree.Name == "t")
                            {
                                currentText.Append(subtree.ReadElementContentAsString());
                            }
                        }
                    }

                    sharedStrings.Add(currentText.ToString());
                }
            }

            return sharedStrings;
        }

        /// <summary>
        /// Converts the specified Excel file, provided as a byte array, into a <see cref="ExcelSheetData"/> object.
        /// </summary>
        /// <remarks>This method processes the first worksheet in the Excel file that matches the
        /// specified <paramref name="sheetFileName"/>. It uses shared strings for cell value resolution and applies the
        /// provided <paramref name="options"/> to customize the reading behavior.</remarks>
        /// <param name="excelFileBytes">The byte array representing the contents of the Excel file.</param>
        /// <param name="sheetFileName">The name of the worksheet to process. Defaults to "sheet1". The name is case-sensitive and must match the
        /// worksheet name in the Excel file.</param>
        /// <param name="options">Optional configuration settings for reading the Excel file. If not provided, default options will be used.</param>
        /// <returns>A <see cref="ExcelSheetData"/> object containing the parsed data from the specified worksheet.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified worksheet file cannot be found in the Excel file.</exception>
        public static ExcelSheetData ToSheetData(byte[] excelFileBytes, string sheetFileName = "sheet1", ExcelReaderOptions? options = null)
        {
            using MemoryStream memoryStream = new(excelFileBytes);
            using ZipArchive archive = new(memoryStream, ZipArchiveMode.Read);
            List<string> sharedStrings = LoadSharedStrings(archive);
            ExcelSheetData sheetData = new();
            // Get the first worksheet (assuming only one for simplicity)
            ZipArchiveEntry sheetEntry = archive.GetEntry($"xl/worksheets/{sheetFileName}.xml")
                ?? throw new FileNotFoundException("Sheet1 not found in the Excel file.");

            WriteSheetData(sheetEntry, sheetData, options ?? new ExcelReaderOptions(), sharedStrings);

            return sheetData;
        }

        /// <summary>
        /// Reads and processes sheet data from an Excel file and writes the parsed content into the provided <see
        /// cref="ExcelSheetData"/> object.
        /// </summary>
        /// <remarks>This method processes the XML content of the specified sheet entry, extracting rows
        /// and cells based on the provided options. If the sheet contains a header row, it will be stored in the <see
        /// cref="ExcelSheetData.Headers"/> property. Subsequent rows will be added to the <see
        /// cref="ExcelSheetData.Rows"/> collection.  If a row contains more columns than the header row, a warning will
        /// be logged and extra columns will be ignored. Shared string references in cells are resolved using the
        /// <paramref name="sharedStrings"/> list.</remarks>
        /// <param name="sheetEntry">The <see cref="ZipArchiveEntry"/> representing the sheet to be processed.</param>
        /// <param name="result">The <see cref="ExcelSheetData"/> object where the parsed sheet data will be stored.</param>
        /// <param name="options">The <see cref="ExcelReaderOptions"/> specifying how the sheet data should be read and processed.</param>
        /// <param name="sharedStrings">A list of shared strings used for resolving cell values in the sheet.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings or informational messages during processing.</param>
        public static void WriteSheetData(ZipArchiveEntry sheetEntry, ExcelSheetData result, ExcelReaderOptions options, List<string> sharedStrings, ILogger? logger = null)
        {
            using Stream stream = sheetEntry.Open();
            using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });

            int currentRowIndex = 0;
            RowBuffer rowBuffer = new(_maxExpectedColumns);

            while (reader.Read())
            {
                // START ROW
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "row")
                {
                    rowBuffer.Reset(); // reset the row buffer for each new row

                    int rowDepth = reader.Depth; // store the current row depth to handle nested elements correctly

                    string? cellRef = null;
                    string? cellType = null;
                    string? cellValue = null;

                    while (reader.Read() && reader.Depth > rowDepth) // check if we are still within the current row
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "c")
                        {
                            // reset per ogni nuova cella
                            cellRef = reader.GetAttribute("r");
                            cellType = reader.GetAttribute("t");
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "v")
                        {
                            cellValue = reader.ReadElementContentAsString();

                            int colIndex = ExcelParserHelper.ParseColumnIndex(cellRef);

                            string? finalValue = cellValue;
                            if (string.Equals(cellType, "s", StringComparison.OrdinalIgnoreCase)
                                && int.TryParse(cellValue, out int sharedIndex)
                                && sharedIndex >= 0 && sharedIndex < sharedStrings.Count)
                            {
                                finalValue = sharedStrings[sharedIndex];
                            }

                            rowBuffer.Set(colIndex, finalValue ?? string.Empty);
                        }
                    }

                    int colCount = rowBuffer.Count;

                    if (options.HasHeaderRow && currentRowIndex == options.HeaderRowIndex)
                    {
                        result.Headers = rowBuffer.ToArray(); // copy the row buffer to headers
                    }
                    else if (currentRowIndex > options.HeaderRowIndex || !options.HasHeaderRow)
                    {
                        if (options.HasHeaderRow && colCount > result.Headers.Length)
                        {
                            string msg = $"Warning: Row has more columns ({colCount}) than header ({result.Headers.Length}). Extra columns will be ignored.";
                            logger?.LogWarning("{Msg}", msg);
                            Debug.WriteLine(msg);

                            result.RowWarnings.Add(new RowWarning(currentRowIndex, msg));
                        }

                        result.Rows.Add(rowBuffer.ToArray()); // copy the row buffer to rows
                    }

                    currentRowIndex++;
                }
            }

            rowBuffer.Return(); // return the row buffer to the pool
        }

        /// <summary>
        /// Resolves the file path of a specific sheet within an Excel workbook archive.
        /// </summary>
        /// <remarks>This method searches for the specified sheet by name in the workbook's metadata and
        /// resolves its associated XML file path. If the sheet is not found, a warning is logged (if a logger is
        /// provided), and the method returns <see langword="null"/>.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> containing the Excel workbook files.</param>
        /// <param name="sheetName">The name of the sheet to locate. This parameter is case-insensitive. Cannot be null, empty, or whitespace.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings if the sheet is not found.</param>
        /// <returns>The relative file path to the XML file representing the specified sheet, prefixed with "xl/",  or <see
        /// langword="null"/> if the sheet is not found.</returns>
        /// <exception cref="FileNotFoundException">Thrown if required files ("workbook.xml" or "workbook.xml.rels") are missing from the archive.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the specified sheet exists but lacks a relationship ID, or if no target file is associated with
        /// the relationship ID.</exception>
        public static string? ResolveSheetFilePath(ZipArchive archive, string? sheetName, ILogger? logger = null)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
                return null;

            // 1. Trova r:id del foglio richiesto
            ZipArchiveEntry workbookEntry = archive.GetEntry("xl/workbook.xml")
                ?? throw new FileNotFoundException("workbook.xml not found");

            string? rId = null;
            using (XmlReader reader = XmlReader.Create(workbookEntry.Open()))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "sheet") // "sheet" is the element for a worksheet
                    {
                        string? nameAttr = reader.GetAttribute("name"); // "name" is the sheet name attribute
                        if (string.Equals(nameAttr, sheetName, StringComparison.OrdinalIgnoreCase))
                        {
                            rId = reader.GetAttribute("r:id"); // "r:id" is the relationship ID for the sheet
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(rId))
            {
                logger?.LogWarning("Sheet '{SheetName}' not found in workbook.xml", sheetName);
                return null;
            }

            // 2. Trova il file XML associato a quell'r:id
            ZipArchiveEntry relsEntry = archive.GetEntry("xl/_rels/workbook.xml.rels")
                ?? throw new FileNotFoundException("workbook.xml.rels not found");

            string? target = null;
            using (XmlReader reader = XmlReader.Create(relsEntry.Open()))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Relationship") // "Relationship" is the element for a relationship
                    {
                        string? id = reader.GetAttribute("Id"); // "Id" is the relationship ID attribute
                        if (id == rId)
                        {
                            target = reader.GetAttribute("Target"); // "Target" is the target file path for the relationship
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(target))
                throw new InvalidOperationException($"No target found for rId '{rId}'");

            return $"xl/{target}";
        }
    }
}
