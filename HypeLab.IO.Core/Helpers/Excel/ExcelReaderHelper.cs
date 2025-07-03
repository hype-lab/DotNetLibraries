using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers.Const;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

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
        /// Loads shared strings from the specified <see cref="ZipArchive"/>.
        /// </summary>
        /// <remarks>This method reads the shared strings from the XML file within the provided archive.
        /// Shared strings  are typically used in spreadsheet documents to store text values in a centralized manner,
        /// reducing  duplication. Each shared string is extracted from the <c>si</c> elements in the XML
        /// file.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> containing the shared strings file.</param>
        /// <returns>A list of shared strings extracted from the archive. If the shared strings file is not found,  an empty list
        /// is returned.</returns>
        public static List<string> LoadSharedStrings(ZipArchive archive)
        {
            ZipArchiveEntry? entry = archive.GetEntry(ExcelDefaults.SharedStringsFileName);
            if (entry == null)
                return [];

            List<string>? strings = null;
            using (StreamReader reader = new(entry.Open()))
            {
                XDocument xDocument = XDocument.Load(reader);

                // each <si> (shared items) can contain multiple <t> (text) or <r></r> (rich text) </t> elements if it's formatted text
                //strings = xDocument.Descendants(XName.Get(ExcelDefaults.LocalNames.Si, ExcelDefaults.Namespaces.Urls.SpreadsheetML))
                strings = [.. xDocument.Descendants(ExcelDefaults.Namespaces.SpreadsheetML + ExcelDefaults.LocalNames.Si)
                    .Select(si =>
                    {
                        //string textNode = si.Elements(XName.Get(ExcelDefaults.LocalNames.T, ExcelDefaults.Namespaces.Urls.SpreadsheetML)).FirstOrDefault()?.Value
                        string? textNode = si.Elements(ExcelDefaults.Namespaces.SpreadsheetML + ExcelDefaults.LocalNames.T).FirstOrDefault()?.Value;
                        return textNode ?? string.Empty;
                    })];
            }

            return strings ?? [];
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
        /// Writes sheet data from the specified Excel sheet entry into the provided <see cref="ExcelSheetData"/>
        /// object.
        /// </summary>
        /// <remarks>This method reads the contents of an Excel sheet entry, processes its rows and cells,
        /// and populates the <see cref="ExcelSheetData"/> object with headers and rows. It supports shared strings and
        /// handles header rows based on the provided <see cref="ExcelReaderOptions"/>.</remarks>
        /// <param name="sheetEntry">The <see cref="ZipArchiveEntry"/> representing the Excel sheet to be read.</param>
        /// <param name="result">The <see cref="ExcelSheetData"/> object to populate with the sheet's data.</param>
        /// <param name="options">The options specifying how the sheet data should be processed, including header row handling.</param>
        /// <param name="sharedStrings">A list of shared strings used to resolve cell values marked as shared strings.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings or informational messages.</param>
        public static void WriteSheetData(ZipArchiveEntry sheetEntry, ExcelSheetData result, ExcelReaderOptions options, List<string> sharedStrings, ILogger? logger = null)
        {
            using Stream stream = sheetEntry.Open();
            using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });

            int currentRowIndex = 0;
            RowBuffer rowBuffer = new(_maxExpectedColumns);

            while (reader.Read())
            {
                // START ROW
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "row") // "row" is the row element
                {
                    rowBuffer.Reset(); // reset the row buffer for each new row

                    string? cellRef = null;
                    string? cellType = null;
                    string? cellValue = null;
                    using XmlReader rowSubtree = reader.ReadSubtree();
                    while (rowSubtree.Read())
                    {
                        if (rowSubtree.NodeType == XmlNodeType.Element && rowSubtree.Name == "c") // "c" is the cell element
                        {
                            cellRef = rowSubtree.GetAttribute("r"); // "r" is the cell reference, e.g., "A1"
                            cellType = rowSubtree.GetAttribute("t"); // "t" is the type attribute, e.g., "s" for shared string
                        }
                        else if (rowSubtree.NodeType == XmlNodeType.Element && rowSubtree.Name == "v") // "v" is the value element
                        {
                            cellValue = rowSubtree.ReadElementContentAsString();

                            int colIndex = ExcelParserHelper.ParseColumnIndex(cellRef);

                            // usando un RowBuffer, non ho più bisogno di gestire la dimensione della riga manualmente, perché il RowBuffer si espande automaticamente
                            // anche evitare questo passaggio dovrebbe ottimizzare le prestazioni un minimo
                            //while (rowBuffer.Count <= colIndex)
                            //{
                            //    rowBuffer.Set(colIndex, string.Empty); // auto-expand the row if necessary
                            //}

                            string? finalValue = cellValue;
                            if (string.Equals(cellType, "s", StringComparison.OrdinalIgnoreCase) // "s" indicates a shared string
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
                        result.Headers = rowBuffer.ToList(); // copy the row buffer to headers
                    }
                    else if (currentRowIndex > options.HeaderRowIndex || !options.HasHeaderRow)
                    {
                        if (options.HasHeaderRow && colCount > result.Headers.Count)
                        {
                            string msg = $"Warning: Row has more columns ({colCount}) than header ({result.Headers.Count}). Extra columns will be ignored.";
                            logger?.LogWarning("{Msg}", msg);
                            Debug.WriteLine(msg);
                            result.RowWarnings.Add(new RowWarning(currentRowIndex, msg));
                        }

                        result.Rows.Add(rowBuffer.ToList()); // copy the row buffer to rows
                    }

                    currentRowIndex++;
                }
            }

            rowBuffer.Return(); // return the row buffer to the pool
        }

        /// <summary>
        /// Processes the data from a specified Excel sheet entry and populates the provided <see cref="ExcelSheetData"/> object
        /// with the parsed rows and headers.
        /// </summary>
        /// <remarks>This method reads the XML content of the specified Excel sheet, parses its rows and cells, and
        /// populates the <paramref name="result"/> object with the extracted data. If the sheet contains a header row (as
        /// specified by <paramref name="options"/>), the headers are extracted and stored separately in the <see
        /// cref="ExcelSheetData.Headers"/> property. Any rows beyond the header row are added to the <see
        /// cref="ExcelSheetData.Rows"/> collection. <para> If a row contains more columns than the header row, a warning is
        /// logged using the provided <paramref name="logger"/> and added to the <see cref="ExcelSheetData.RowWarnings"/>
        /// collection. </para> <para> Shared strings are resolved using the <paramref name="sharedStrings"/> list, and any
        /// unresolved or missing values are replaced with empty strings. </para></remarks>
        /// <param name="sheetEntry">The <see cref="ZipArchiveEntry"/> representing the Excel sheet to be processed.</param>
        /// <param name="result">The <see cref="ExcelSheetData"/> object that will be populated with the parsed data, including headers and rows.</param>
        /// <param name="options">The <see cref="ExcelReaderOptions"/> specifying how the sheet data should be processed, including header row index
        /// and other parsing options.</param>
        /// <param name="sharedStrings">A list of shared strings used for resolving cell values in the sheet.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings or informational messages during processing.</param>
        /// <exception cref="AmbiguousHeaderIndexException">Thrown when the specified <see cref="ExcelReaderOptions.HeaderRowIndex"/> exceeds the number of available rows in
        /// the sheet.</exception>
        public static void WriteSheetDataLegacy(ZipArchiveEntry sheetEntry, ExcelSheetData result, ExcelReaderOptions options, List<string> sharedStrings, ILogger? logger = null)
        {
            using StreamReader reader = new(sheetEntry.Open());
            XDocument xDocument = XDocument.Load(reader);

            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;
            XName rowName = XName.Get(ExcelDefaults.LocalNames.Row, ns.NamespaceName);
            XName cellName = XName.Get(ExcelDefaults.LocalNames.C, ns.NamespaceName);
            XName valueName = XName.Get(ExcelDefaults.LocalNames.V, ns.NamespaceName);
            //XName typeName = XName.Get(ExcelDefaults.LocalNames.T, ns.NamespaceName)

            List<XElement> rows = [.. xDocument.Descendants(rowName)];
            if (!rows.Any())
                return; // result; // No rows found, return empty data

            if (options.HeaderRowIndex >= rows.Count)
                throw new AmbiguousHeaderIndexException($"HeaderRowIndex {options.HeaderRowIndex} exceeds available rows ({rows.Count}) in sheet '{options.SheetName}'.");

            int currentRowIndex = 0;
            foreach (XElement row in rows)
            {
                //int columnCount = result.Headers.Count > 0 ? result.Headers.Count : 20
                //List<string> rowValues = Enumerable.Repeat(string.Empty, columnCount).ToList()
                List<string> rowValues = [];
                foreach (XElement cell in row.Elements(cellName))
                {
                    string? cellRef = cell.Attribute(ExcelDefaults.LocalNames.R)?.Value;
                    int colIndex = ExcelParserHelper.ParseColumnIndex(cellRef);

                    // auto-expand la riga se necessario
                    while (rowValues.Count <= colIndex)
                        rowValues.Add(string.Empty);

                    string? cellType = cell.Attribute(ExcelDefaults.LocalNames.T)?.Value;
                    string? rawValue = cell.Element(valueName)?.Value;
                    string? finalValue = rawValue;

                    if (string.Equals(cellType, ExcelDefaults.LocalNames.S, StringComparison.OrdinalIgnoreCase)
                        && int.TryParse(rawValue, out int sharedIndex)
                        && sharedIndex >= 0 && sharedIndex < sharedStrings.Count)
                    {
                        finalValue = sharedStrings[sharedIndex];
                    }

                    rowValues[colIndex] = finalValue ?? string.Empty;
                }

                if (options.HasHeaderRow && result.Headers.Any() && rowValues.Count > result.Headers.Count)
                {
                    string msg = $"Warning: Row has more columns ({rowValues.Count}) than header ({result.Headers.Count}). Extra columns will be ignored.";
                    logger?.LogWarning("{Msg}", msg);
                    Debug.WriteLine(msg);
                    result.RowWarnings.Add(new RowWarning(currentRowIndex, msg));
                }

                if (currentRowIndex == options.HeaderRowIndex && options.HasHeaderRow)
                    result.Headers = rowValues;
                else if (currentRowIndex > options.HeaderRowIndex || !options.HasHeaderRow)
                    result.Rows.Add(rowValues);

                currentRowIndex++;
            }
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

            // 1. Carica workbook.xml per trovare il r:id del foglio cercato
            ZipArchiveEntry workbookEntry = archive.GetEntry("xl/workbook.xml")
                ?? throw new FileNotFoundException("workbook.xml not found in archive");

            XDocument workbookDoc;
            using (StreamReader reader = new(workbookEntry.Open()))
            {
                workbookDoc = XDocument.Load(reader);
            }

            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            XNamespace relNs = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

            // Cerca lo <sheet> con il nome richiesto
            XElement? sheetElement = workbookDoc
                .Root?
                .Element(ns + "sheets")?
                .Elements(ns + "sheet")
                .FirstOrDefault(e => string.Equals(e.Attribute("name")?.Value, sheetName, StringComparison.OrdinalIgnoreCase));

            if (sheetElement is null)
            {
                logger?.LogWarning("Sheet name '{SheetName}' not found in workbook.", sheetName);
                return null;
            }

            string? rId = sheetElement.Attribute(relNs + "id")?.Value;
            if (string.IsNullOrWhiteSpace(rId))
                throw new InvalidOperationException($"Sheet '{sheetName}' has no relationship ID (r:id)");

            // 2. Carica workbook.xml.rels e trova il path del file xml associato a quel rId
            ZipArchiveEntry relsEntry = archive.GetEntry("xl/_rels/workbook.xml.rels")
                ?? throw new FileNotFoundException("workbook.xml.rels not found in archive");

            XDocument relsDoc;
            using (StreamReader reader = new(relsEntry.Open()))
            {
                relsDoc = XDocument.Load(reader);
            }

            XElement? relationship = relsDoc
                .Root?
                .Elements()
                .FirstOrDefault(e => e.Attribute("Id")?.Value == rId);

            string? target = relationship?.Attribute("Target")?.Value;
            if (string.IsNullOrWhiteSpace(target))
                throw new InvalidOperationException($"No target found for rId '{rId}'");

            return $"xl/{target}";
        }
    }
}
