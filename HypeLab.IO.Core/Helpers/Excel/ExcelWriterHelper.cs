using HypeLab.IO.Core.Data.Attributes.Excel;
using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Common.Colors;
using HypeLab.IO.Core.Data.Models.Common.Fonts;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Models.Excel.Worksheet;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers.Const;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides helper methods for generating and adding Excel-compatible XML files to a ZIP archive.
    /// </summary>
    /// <remarks>This static class contains methods for creating various components of an Excel workbook in
    /// the OpenXML SpreadsheetML format. It supports generating worksheets, styles, shared strings, and other required
    /// files, and adding them to a ZIP archive. The methods are designed to handle data serialization, style
    /// application, and efficient string management.</remarks>
    public static class ExcelWriterHelper
    {
        /// <summary>
        /// Adds a worksheet XML file to the specified <see cref="ZipArchive"/> based on the provided data and options.
        /// </summary>
        /// <remarks>This method generates an XML representation of a worksheet and adds it to the
        /// specified <see cref="ZipArchive"/>.  The worksheet includes a header row based on the public readable
        /// properties of the type <typeparamref name="T"/>  and subsequent rows for the data provided. Properties
        /// marked with the <see cref="ExcelIgnoreAttribute"/>  (with <c>OnWrite</c> set to <c>true</c>) are excluded
        /// from the output.  The method uses the <paramref name="sharedStringBuilder"/> to optimize string storage and
        /// the  <paramref name="styleIndexBuilder"/> to apply styles to cells. Logging is performed at various stages
        /// if a <paramref name="logger"/> is provided.</remarks>
        /// <typeparam name="T">The type of the data objects to be written to the worksheet. Must be a reference type.</typeparam>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the worksheet XML file will be added.</param>
        /// <param name="data">The collection of data objects to be written to the worksheet. Each object represents a row in the
        /// worksheet.</param>
        /// <param name="options">The <see cref="ExcelWriterOptions"/> that define the configuration for writing the worksheet, such as
        /// formatting and behavior.</param>
        /// <param name="sharedStringBuilder">The <see cref="SharedStringBuilder"/> used to manage shared strings in the workbook. This ensures efficient
        /// handling of repeated text values.</param>
        /// <param name="styleIndexBuilder">The <see cref="ExcelStyleIndexBuilder"/> used to manage and apply styles to cells in the worksheet.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the worksheet generation
        /// process. If null, no logging will occur.</param>
        public static void AddWorksheetXml<T>(ZipArchive archive, IEnumerable<T> data, ExcelWriterOptions options, SharedStringBuilder sharedStringBuilder, ExcelStyleIndexBuilder styleIndexBuilder, ILogger? logger = null)
            where T : class
        {
            logger?.LogDebug("Adding worksheet XML for type {TypeName}", typeof(T).Name);
            ZipArchiveEntry sheetEntry = archive.CreateEntry(ExcelDefaults.Worksheets.Sheet1PathName);

            using StreamWriter writer = new(sheetEntry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;
            xml.WriteStartDocument();
            xml.WriteStartElement("worksheet", ns.NamespaceName);
            xml.WriteStartElement("sheetData");

            PropertyInfo[] props = [.. typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnWrite)))];

            logger?.LogDebug("Properties to write: {PropertyCount}", props.Length);

            int rowIndex = 1;

            // header
            logger?.LogDebug("Processing header row at index {RowIndex}", rowIndex);
            xml.WriteStartElement("row", ns.NamespaceName);
            xml.WriteAttributeString("r", rowIndex.ToString());

            for (int colIndex = 0; colIndex < props.Length; colIndex++)
            {
                logger?.LogDebug("Processing column {ColumnIndex}: {PropertyName}", colIndex, props[colIndex].Name);
                PropertyInfo prop = props[colIndex];
                ExcelColumnAttribute? columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>(true);

                string? headerText = columnAttr?.OnWrite == true && !string.IsNullOrWhiteSpace(columnAttr.Name) ? columnAttr.Name : prop.Name;

                XElement cell = CreateCellXml(headerText, prop.Name, sharedStringBuilder, options);

                if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, headerText) is int styleIndex)
                {
                    logger?.LogDebug("Applying style index {StyleIndex} to header cell at ({RowIndex}, {ColumnIndex})", styleIndex, rowIndex, colIndex);
                    int s;
                    if (styleIndex == 0)
                        s = 1;
                    else
                        s = styleIndex;

                    cell.SetAttributeValue("s", s);
                }

                cell.SetAttributeValue("r", GetCellReference(1, colIndex)); // Excel è 1-based per riga
                cell.WriteTo(xml);
            }

            logger?.LogDebug("Finished processing header row at index {RowIndex}", rowIndex);
            xml.WriteEndElement(); // </row>
            rowIndex++;

            // data
            logger?.LogDebug("Processing data rows, total count: {RowCount}", data.Count());
            List<T> dataList = [.. data];
            for (int rIndex = 0; rIndex < dataList.Count; rIndex++)
            {
                logger?.LogDebug("Processing row {RowIndex}", rIndex + 2); // +2 for header
                T item = dataList[rIndex];
                xml.WriteStartElement("row", ns.NamespaceName);
                xml.WriteAttributeString("r", (rIndex + 2).ToString()); // +2 per l'header

                for (int colIndex = 0; colIndex < props.Length; colIndex++)
                {
                    logger?.LogDebug("Processing row {RowIndex}, column {ColumnIndex}: {PropertyName}", rIndex + 2, colIndex, props[colIndex].Name);
                    PropertyInfo prop = props[colIndex];
                    string name = prop.Name;

                    object? value = prop.GetValue(item);

                    XElement cell = CreateCellXml(value, name, sharedStringBuilder, options);

                    if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, value) is int styleIndex)
                    {
                        logger?.LogDebug("Applying style index {StyleIndex} to data cell at ({RowIndex}, {ColumnIndex})", styleIndex, rIndex + 2, colIndex);
                        cell.SetAttributeValue("s", styleIndex + 1);
                    }

                    cell.SetAttributeValue("r", GetCellReference(rIndex + 2, colIndex)); // +2 per Excel
                    cell.WriteTo(xml);
                }

                xml.WriteEndElement(); // </row>

                rowIndex++;
            }

            logger?.LogDebug("Finished processing all data rows.");
            xml.WriteEndElement(); // </sheetData>
            xml.WriteEndElement(); // </worksheet>
            xml.WriteEndDocument();
        }

        /// <summary>
        /// Adds a worksheet XML file to the specified <see cref="ZipArchive"/> based on the provided data.
        /// </summary>
        /// <remarks>This method generates an XML representation of a worksheet and adds it to the
        /// specified <see cref="ZipArchive"/>. The worksheet includes a header row based on the public readable
        /// properties of the type <typeparamref name="T"/>. Data rows are generated from the provided <paramref
        /// name="data"/> collection, with each object representing a row.  The method processes rows in batches defined
        /// by <paramref name="chunkBatchSize"/> to optimize memory usage. It supports cancellation via the <paramref
        /// name="cancellationToken"/> parameter.  Properties marked with the <see cref="ExcelIgnoreAttribute"/> (with
        /// <c>OnWrite</c> set to <c>true</c>) are excluded from the worksheet.</remarks>
        /// <typeparam name="T">The type of the data objects to be written to the worksheet. Must be a reference type.</typeparam>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the worksheet XML file will be added.</param>
        /// <param name="data">The collection of data objects to be written to the worksheet. Each object represents a row in the
        /// worksheet.</param>
        /// <param name="chunkBatchSize">The maximum number of rows to process in a single batch. Must be a positive integer.</param>
        /// <param name="options">The <see cref="ExcelWriterOptions"/> that specify configuration settings for writing the worksheet.</param>
        /// <param name="sharedStringBuilder">The <see cref="SharedStringBuilder"/> used to manage shared strings in the worksheet.</param>
        /// <param name="styleIndexBuilder">The <see cref="ExcelStyleIndexBuilder"/> used to resolve and apply styles to cells in the worksheet.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task AddWorksheetXmlAsync<T>(ZipArchive archive, IEnumerable<T> data, int chunkBatchSize, ExcelWriterOptions options, SharedStringBuilder sharedStringBuilder, ExcelStyleIndexBuilder styleIndexBuilder, ILogger? logger = null, CancellationToken cancellationToken = default)
            where T : class
        {
            logger?.LogDebug("Adding worksheet XML for type {TypeName}", typeof(T).Name);

            await Task.Yield(); // Yield to allow other tasks to run

            //ZipArchiveEntry sheetEntryd = archive.CreateEntry(ExcelDefaults.Worksheets.Sheet1PathName);

            ZipArchiveEntry sheetEntry = archive.CreateEntry(ExcelDefaults.Worksheets.Sheet1PathName);

            using StreamWriter writer = new(sheetEntry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;
            xml.WriteStartDocument();
            xml.WriteStartElement("worksheet", ns.NamespaceName);
            xml.WriteStartElement("sheetData");

            PropertyInfo[] props = [.. typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnWrite)))];

            logger?.LogDebug("Properties to write: {PropertyCount}", props.Length);

            int rowIndex = 1;

            // header
            logger?.LogDebug("Processing header row at index {RowIndex}", rowIndex);
            xml.WriteStartElement("row", ns.NamespaceName);
            xml.WriteAttributeString("r", rowIndex.ToString());

            for (int colIndex = 0; colIndex < props.Length; colIndex++)
            {
                logger?.LogDebug("Processing column {ColumnIndex}: {PropertyName}", colIndex, props[colIndex].Name);
                PropertyInfo prop = props[colIndex];
                ExcelColumnAttribute? columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>(true);

                string? headerText = columnAttr?.OnWrite == true && !string.IsNullOrWhiteSpace(columnAttr.Name) ? columnAttr.Name : prop.Name;

                XElement cell = CreateCellXml(headerText, prop.Name, sharedStringBuilder, options);

                if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, headerText) is int styleIndex)
                {
                    logger?.LogDebug("Applying style index {StyleIndex} to header cell at ({RowIndex}, {ColumnIndex})", styleIndex, rowIndex, colIndex);
                    int s;
                    if (styleIndex == 0)
                        s = 1;
                    else
                        s = styleIndex;

                    cell.SetAttributeValue("s", s);
                }

                cell.SetAttributeValue("r", GetCellReference(1, colIndex)); // Excel è 1-based per riga
                cell.WriteTo(xml);
            }

            logger?.LogDebug("Finished processing header row at index {RowIndex}", rowIndex);
            xml.WriteEndElement(); // </row>
            rowIndex++;

            // data
            logger?.LogDebug("Processing data rows, total count: {RowCount}", data.Count());
            int dataCount = data.Count();
            for (int startIndex = 0; startIndex < dataCount; startIndex += chunkBatchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // creo endIndex invece di usare direttamente chunkBatchSize per evitare di superare il numero totale di righe
                //int endIndex = Math.Min(startIndex + chunkBatchSize, dataCount);
                logger?.LogDebug("Processing rows from {StartIndex} to {EndIndex}", startIndex, chunkBatchSize);
                IEnumerable<T> chunk = data.Skip(startIndex).Take(chunkBatchSize);

                List<T> dataList = [.. chunk];
                for (int rIndex = 0; rIndex < dataList.Count; rIndex++)
                {
                    int wholeRowIndex = rIndex + startIndex; // adjusted row index for chunking to not break the Excel row numbering
                    logger?.LogDebug("Processing row {RowIndex}", wholeRowIndex + 2); // +2 for header
                    T item = dataList[rIndex];
                    xml.WriteStartElement("row", ns.NamespaceName);
                    xml.WriteAttributeString("r", (wholeRowIndex + 2).ToString()); // +2 for header

                    for (int colIndex = 0; colIndex < props.Length; colIndex++)
                    {
                        logger?.LogDebug("Processing row {RowIndex}, column {ColumnIndex}: {PropertyName}", wholeRowIndex + 2, colIndex, props[colIndex].Name);
                        PropertyInfo prop = props[colIndex];

                        object? value = prop.GetValue(item);

                        XElement cell = CreateCellXml(value, prop.Name, sharedStringBuilder, options);

                        if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, value) is int styleIndex)
                        {
                            logger?.LogDebug("Applying style index {StyleIndex} to data cell at ({RowIndex}, {ColumnIndex})", styleIndex, rowIndex, colIndex);
                            cell.SetAttributeValue("s", styleIndex + 1);
                        }

                        cell.SetAttributeValue("r", GetCellReference(wholeRowIndex + 2, colIndex)); // +2 per Excel
                        cell.WriteTo(xml);
                    }

                    xml.WriteEndElement(); // </row>

                    rowIndex++;
                }

                logger?.LogDebug("Processed rows from {StartIndex} to {EndIndex}", startIndex, chunkBatchSize);
                //rowIndex += (endIndex - startIndex); // Increment rowIndex by the number of rows processed in this chunk
                await Task.Yield(); // Yield to allow other tasks to run
            }

            logger?.LogDebug("Finished processing all data rows.");
            xml.WriteEndElement(); // </sheetData>
            xml.WriteEndElement(); // </worksheet>
            xml.WriteEndDocument();
        }

        /// <summary>
        /// Adds a worksheet XML file to the specified ZIP archive, representing the provided data as rows and columns
        /// in an Excel-compatible format.
        /// </summary>
        /// <remarks>This method generates an Excel worksheet in XML format, adhering to the OpenXML
        /// SpreadsheetML standard. The worksheet includes a header row derived from the property names of <typeparamref
        /// name="T"/> (or custom names specified via <see cref="ExcelColumnAttribute"/>), followed by rows representing
        /// the data objects. Properties marked with <see cref="ExcelIgnoreAttribute"/> (with <c>OnWrite</c> set to
        /// <c>true</c>) are excluded.  The generated worksheet is added to the archive at the path
        /// <c>xl/worksheets/{sheetFileName}.xml</c>.</remarks>
        /// <typeparam name="T">The type of the data objects to be written to the worksheet. Each public, readable property of <typeparamref
        /// name="T"/> will be represented as a column.</typeparam>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the worksheet XML file will be added.</param>
        /// <param name="sheetFileName">The name of the worksheet file (without extension) to be created within the archive.</param>
        /// <param name="data">The collection of data objects to be written to the worksheet. Each object represents a row in the
        /// worksheet.</param>
        /// <param name="options">Options that control the behavior of the worksheet generation, such as formatting and style settings.</param>
        /// <param name="sharedStringBuilder">A <see cref="SharedStringBuilder"/> used to manage shared strings in the workbook, ensuring efficient string
        /// reuse across worksheets.</param>
        /// <param name="styleIndexBuilder">A <see cref="ExcelStyleIndexBuilder"/> used to resolve and apply styles to cells in the worksheet.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the worksheet generation
        /// process. If <c>null</c>, no logging will occur.</param>
        public static void AddWorksheetsXml<T>(ZipArchive archive, string sheetFileName, IEnumerable<T> data, ExcelWorksheetOptions options, SharedStringBuilder sharedStringBuilder, ExcelStyleIndexBuilder styleIndexBuilder, ILogger? logger = null)
         where T : class
        {
            logger?.LogDebug("Adding worksheet XML for type {TypeName} with file name {SheetFileName}", typeof(T).Name, sheetFileName);
            ZipArchiveEntry sheetEntry = archive.CreateEntry($"xl/worksheets/{sheetFileName}.xml");

            using StreamWriter writer = new(sheetEntry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;
            xml.WriteStartDocument();
            xml.WriteStartElement("worksheet", ns.NamespaceName);
            xml.WriteStartElement("sheetData");

            PropertyInfo[] props = [.. typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnWrite)))];

            logger?.LogDebug("Properties to write: {PropertyCount}", props.Length);

            int rowIndex = 1;

            // header
            logger?.LogDebug("Processing header row at index {RowIndex}", rowIndex);
            xml.WriteStartElement("row", ns.NamespaceName);
            xml.WriteAttributeString("r", rowIndex.ToString());

            for (int colIndex = 0; colIndex < props.Length; colIndex++)
            {
                logger?.LogDebug("Processing column {ColumnIndex}: {PropertyName}", colIndex, props[colIndex].Name);
                PropertyInfo prop = props[colIndex];
                ExcelColumnAttribute? columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>(true);

                string? headerText = columnAttr?.OnWrite == true && !string.IsNullOrWhiteSpace(columnAttr.Name) ? columnAttr.Name : prop.Name;

                XElement cell = CreateCellXml(headerText, prop.Name, sharedStringBuilder, options);

                if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, headerText) is int styleIndex)
                {
                    logger?.LogDebug("Applying style index {StyleIndex} to header cell at ({RowIndex}, {ColumnIndex})", styleIndex, rowIndex, colIndex);
                    //int s;
                    //if (styleIndex == 0)
                    //    s = 1;
                    //else
                    //    s = styleIndex;

                    cell.SetAttributeValue("s", styleIndex + 1);
                    //cell.SetAttributeValue("s", s);
                }

                cell.SetAttributeValue("r", GetCellReference(1, colIndex)); // Excel è 1-based per riga
                cell.WriteTo(xml);
            }

            logger?.LogDebug("Finished processing header row at index {RowIndex}", rowIndex);
            xml.WriteEndElement(); // </row>
            rowIndex++;

            // data
            logger?.LogDebug("Processing data rows, total count: {RowCount}", data.Count());
            List<T> dataList = [.. data];
            for (int rIndex = 0; rIndex < dataList.Count; rIndex++)
            {
                T item = dataList[rIndex];
                xml.WriteStartElement("row", ns.NamespaceName);
                xml.WriteAttributeString("r", (rIndex + 2).ToString()); // +2 per l'header

                for (int colIndex = 0; colIndex < props.Length; colIndex++)
                {
                    logger?.LogDebug("Processing row {RowIndex}, column {ColumnIndex}: {PropertyName}", rIndex + 2, colIndex, props[colIndex].Name);
                    PropertyInfo prop = props[colIndex];

                    object? value = prop.GetValue(item);

                    XElement cell = CreateCellXml(value, prop.Name, sharedStringBuilder, options);

                    if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, value) is int styleIndex)
                        cell.SetAttributeValue("s", styleIndex + 1);

                    cell.SetAttributeValue("r", GetCellReference(rIndex + 2, colIndex)); // +2 per Excel
                    cell.WriteTo(xml);
                }

                xml.WriteEndElement(); // </row>

                rowIndex++;
            }

            logger?.LogDebug("Finished processing all data rows.");
            xml.WriteEndElement(); // </sheetData>
            xml.WriteEndElement(); // </worksheet>
            xml.WriteEndDocument();
        }

        /// <summary>
        /// Adds a worksheet XML file to the specified ZIP archive, containing data serialized from the provided
        /// collection.
        /// </summary>
        /// <remarks>This method generates an Excel worksheet XML file based on the provided data and adds
        /// it to the specified ZIP archive. The worksheet includes a header row derived from the properties of the type
        /// <typeparamref name="T"/> and subsequent rows containing the serialized data. Properties marked with the <see
        /// cref="ExcelIgnoreAttribute"/> are excluded from the output.  The method processes data in chunks, as
        /// specified by <paramref name="chunkBatchSize"/>, to optimize memory usage for large datasets. It supports
        /// applying custom styles to cells and uses shared strings to reduce file size when repeated strings are
        /// present.  The caller is responsible for ensuring that the <paramref name="sheetFileName"/> is unique within
        /// the archive and that the <paramref name="data"/> collection is not null. If the operation is canceled via
        /// <paramref name="cancellationToken"/>, an <see cref="OperationCanceledException"/> will be thrown.</remarks>
        /// <typeparam name="T">The type of objects in the data collection. Each object represents a row in the worksheet.</typeparam>
        /// <param name="archive">The ZIP archive to which the worksheet XML file will be added.</param>
        /// <param name="sheetFileName">The name of the worksheet file to be created within the archive. Do not include the file extension.</param>
        /// <param name="data">The collection of data objects to be written to the worksheet. Each object corresponds to a row in the
        /// worksheet.</param>
        /// <param name="chunkBatchSize">The maximum number of rows to process in a single batch. This helps manage memory usage for large datasets.</param>
        /// <param name="options">Options for configuring the worksheet generation, such as formatting and column behavior.</param>
        /// <param name="sharedStringBuilder">A builder for managing shared strings in the worksheet, which optimizes string storage.</param>
        /// <param name="styleIndexBuilder">A builder for managing style indices applied to cells in the worksheet.</param>
        /// <param name="logger">An optional logger for capturing debug information during the worksheet generation process.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests, allowing the operation to be canceled if needed.</param>
        /// <returns>A task that represents the asynchronous operation of adding the worksheet XML file to the archive.</returns>
        public static async Task AddWorksheetsXmlAsync<T>(ZipArchive archive, string sheetFileName, IEnumerable<T> data, int chunkBatchSize, ExcelWorksheetOptions options, SharedStringBuilder sharedStringBuilder, ExcelStyleIndexBuilder styleIndexBuilder, ILogger? logger = null, CancellationToken cancellationToken = default)
         where T : class
        {
            logger?.LogDebug("Adding worksheet XML for type {TypeName} with file name {SheetFileName}", typeof(T).Name, sheetFileName);

            await Task.Yield(); // Yield to allow other tasks to run

            ZipArchiveEntry sheetEntry = archive.CreateEntry($"xl/worksheets/{sheetFileName}.xml");

            using StreamWriter writer = new(sheetEntry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;
            xml.WriteStartDocument();
            xml.WriteStartElement("worksheet", ns.NamespaceName);
            xml.WriteStartElement("sheetData");

            PropertyInfo[] props = [.. typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>(true) is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnWrite)))];

            logger?.LogDebug("Properties to write: {PropertyCount}", props.Length);

            int rowIndex = 1;

            // header
            logger?.LogDebug("Processing header row at index {RowIndex}", rowIndex);
            xml.WriteStartElement("row", ns.NamespaceName);
            xml.WriteAttributeString("r", rowIndex.ToString());

            for (int colIndex = 0; colIndex < props.Length; colIndex++)
            {
                logger?.LogDebug("Processing column {ColumnIndex}: {PropertyName}", colIndex, props[colIndex].Name);
                PropertyInfo prop = props[colIndex];
                ExcelColumnAttribute? columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>(true);

                string? headerText = columnAttr?.OnWrite == true && !string.IsNullOrWhiteSpace(columnAttr.Name) ? columnAttr.Name : prop.Name;

                XElement cell = CreateCellXml(headerText, prop.Name, sharedStringBuilder, options);

                if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, headerText) is int styleIndex)
                {
                    logger?.LogDebug("Applying style index {StyleIndex} to header cell at ({RowIndex}, {ColumnIndex})", styleIndex, rowIndex, colIndex);

                    cell.SetAttributeValue("s", styleIndex + 1);
                }

                cell.SetAttributeValue("r", GetCellReference(1, colIndex)); // Excel è 1-based per riga
                cell.WriteTo(xml);
            }

            logger?.LogDebug("Finished processing header row at index {RowIndex}", rowIndex);
            xml.WriteEndElement(); // </row>
            rowIndex++;

            // data
            int totalRows = data.Count();
            logger?.LogDebug("Processing data rows, total count: {RowCount}", totalRows);
            for (int startIndex = 0; startIndex < totalRows; startIndex += chunkBatchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();

                logger?.LogDebug("Processing rows from {StartIndex} to {EndIndex}", startIndex, chunkBatchSize);
                IEnumerable<T> chunk = data.Skip(startIndex).Take(chunkBatchSize);

                List<T> dataList = [.. chunk];
                for (int rIndex = 0; rIndex < dataList.Count; rIndex++)
                {
                    int wholeRowIndex = rIndex + startIndex; // adjusted row index for chunking to not break the Excel row numbering
                    T item = dataList[rIndex];
                    xml.WriteStartElement("row", ns.NamespaceName);
                    xml.WriteAttributeString("r", (wholeRowIndex + 2).ToString()); // +2 per l'header

                    for (int colIndex = 0; colIndex < props.Length; colIndex++)
                    {
                        logger?.LogDebug("Processing row {RowIndex}, column {ColumnIndex}: {PropertyName}", wholeRowIndex + 2, colIndex, props[colIndex].Name);
                        PropertyInfo prop = props[colIndex];

                        object? value = prop.GetValue(item);

                        XElement cell = CreateCellXml(value, prop.Name, sharedStringBuilder, options);

                        if (TryResolveStyle(options, styleIndexBuilder, rowIndex, colIndex, prop, value) is int styleIndex)
                            cell.SetAttributeValue("s", styleIndex + 1);

                        cell.SetAttributeValue("r", GetCellReference(wholeRowIndex + 2, colIndex)); // +2 per Excel
                        cell.WriteTo(xml);
                    }

                    xml.WriteEndElement(); // </row>

                    rowIndex++;
                }

                logger?.LogDebug("Processed rows from {StartIndex} to {EndIndex}", startIndex, chunkBatchSize);
                //rowIndex += (endIndex - startIndex); // Increment rowIndex by the number of rows processed in this chunk
                await Task.Yield(); // Yield to avoid blocking the thread for large datasets
            }

            logger?.LogDebug("Finished processing all data rows.");
            xml.WriteEndElement(); // </sheetData>
            xml.WriteEndElement(); // </worksheet>
            xml.WriteEndDocument();
        }

        /// <summary>
        /// Adds the styles XML file to the specified Excel archive, defining styles such as fonts, fills, borders, and
        /// number formats.
        /// </summary>
        /// <remarks>This method generates an XML document that defines the styles used in an Excel
        /// workbook, including fonts, fills, borders, and number formats. The styles are validated and deduplicated
        /// before being written to the XML file. Default styles are also included to ensure compatibility.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the styles XML file will be added.</param>
        /// <param name="styleBuilder">An <see cref="ExcelStyleIndexBuilder"/> containing the styles to be included in the styles XML file.</param>
        /// <param name="options">The <see cref="ExcelWriterOptions"/> that specify culture and other configuration settings for writing the
        /// styles.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the process. Can be <see
        /// langword="null"/>.</param>
        public static void AddStylesXml(ZipArchive archive, ExcelStyleIndexBuilder styleBuilder, ExcelWriterOptions options, ILogger? logger = null)
        {
            logger?.LogDebug("Adding styles XML with {StyleCount} styles", styleBuilder.Styles.Count);
            ZipArchiveEntry stylesEntry = archive.CreateEntry("xl/styles.xml");

            using Stream stream = stylesEntry.Open();
            using XmlWriter xml = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });

            List<FontSettings> fontSettingsList = [];
            List<string> fills = []; // Hex colors
            List<string> borders = []; // Hex colors
            Dictionary<string, int> numberFormats = []; // FormatCode -> numFmtId

            // Create unique IDs per component
            logger?.LogDebug("Validating styles and creating unique IDs for fonts, fills, and borders.");
            foreach (StyleDefinition style in styleBuilder.Styles)
            {
                logger?.LogDebug("Processing style: {StyleName}", styleBuilder.Styles.ToList().IndexOf(style).ToString() ?? "Unnamed Style");
                FontSettings safeFontSettings = ValidateFontSettings(UnsafeFontSettings.AsUnsafe(style.FontName, style.FontColor, style.FontSize, style.Bold));
                ColorSettings safeColorSettings = ValidateColorSettings(UnsafeColorSettings.AsUnsafe(style.FillColor, style.BorderColor));

                fontSettingsList.Add(safeFontSettings);
                fills.Add(safeColorSettings.FillColor);
                borders.Add(safeColorSettings.BorderColor);

                if (!string.IsNullOrWhiteSpace(style.NumberFormat) && !numberFormats.ContainsKey(style.NumberFormat!))
                    numberFormats[style.NumberFormat!] = 165 + numberFormats.Count; // Custom numFmtId must be >= 164
            }

            // Start writing the XML document
            logger?.LogDebug("Starting to write styles XML document.");
            xml.WriteStartDocument();
            xml.WriteStartElement("styleSheet", ExcelDefaults.Namespaces.SpreadsheetML.NamespaceName);

            // === NumberFormats ===
            logger?.LogDebug("Writing number formats, count: {Count}", numberFormats.Count);
            xml.WriteStartElement("numFmts");
            xml.WriteAttributeString("count", numberFormats.Count.ToString());
            foreach (KeyValuePair<string, int> kvp in numberFormats)
            {
                logger?.LogDebug("Adding number format: {FormatCode} with ID {NumFmtId}", kvp.Key, kvp.Value);
                xml.WriteStartElement("numFmt");
                xml.WriteAttributeString("numFmtId", kvp.Value.ToString());
                xml.WriteAttributeString("formatCode", kvp.Key);
                xml.WriteEndElement(); // </numFmt>
            }
            logger?.LogDebug("Finished writing number formats.");
            xml.WriteEndElement(); // </numFmts>

            // === Fonts ===
            logger?.LogDebug("Writing fonts, count: {Count}", fontSettingsList.Count);
            List<FontSettings> distinctFontSettings = [.. fontSettingsList.Distinct()];
            xml.WriteStartElement("fonts");
            xml.WriteAttributeString("count", distinctFontSettings.Count.ToString());
            foreach (FontSettings setting in distinctFontSettings)
            {
                logger?.LogDebug("Adding font: {FontName}, Color: {FontColor}, Size: {FontSize}, Bold: {Bold}",
                    setting.FontName ?? "Default", setting.FontColor ?? "None", setting.FontSize, setting.Bold);

                xml.WriteStartElement("font");
                if (setting.Bold)
                {
                    xml.WriteStartElement("b");
                    xml.WriteEndElement();
                }

                if (!string.IsNullOrWhiteSpace(setting.FontColor))
                {
                    xml.WriteStartElement("color");
                    xml.WriteAttributeString("rgb", NormalizeColor(setting.FontColor));
                    xml.WriteEndElement();
                }

                if (setting.FontSize > 0)
                {
                    //xml.WriteElementString("sz", setting.FontSize.ToString(options.Culture));
                    xml.WriteStartElement("sz");
                    xml.WriteAttributeString("val", setting.FontSize.ToString(options.Culture));
                    xml.WriteEndElement(); // </sz>
                }

                if (!string.IsNullOrWhiteSpace(setting.FontName))
                {
                    xml.WriteStartElement("name");
                    xml.WriteAttributeString("val", setting.FontName);
                    xml.WriteEndElement();
                    //xml.WriteElementString("name", setting.FontName);
                }

                xml.WriteEndElement(); // </font>
            }
            logger?.LogDebug("Finished writing fonts.");
            xml.WriteEndElement(); // </fonts>

            // === Fills ===
            logger?.LogDebug("Writing fills, count: {Count}", fills.Distinct().Count());
            List<string> distinctFills = [.. fills.Distinct()];
            xml.WriteStartElement("fills");
            xml.WriteAttributeString("count", (distinctFills.Count + 2).ToString()); // +2 default
            WriteDefaultFills(xml);
            foreach (string fillColor in distinctFills.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                logger?.LogDebug("Adding fill color: {FillColor}", fillColor);
                xml.WriteStartElement("fill");
                xml.WriteStartElement("patternFill");
                xml.WriteAttributeString("patternType", "solid");
                xml.WriteStartElement("fgColor");
                xml.WriteAttributeString("rgb", NormalizeColor(fillColor));
                xml.WriteEndElement(); // </fgColor>
                xml.WriteElementString("bgColor", "indexed", "64");
                xml.WriteEndElement(); // </patternFill>
                xml.WriteEndElement(); // </fill>
            }
            logger?.LogDebug("Finished writing fills.");
            xml.WriteEndElement(); // </fills>

            // === Borders ===
            logger?.LogDebug("Writing borders, count: {Count}", borders.Distinct().Count());
            List<string> distinctBorders = [.. borders.Distinct()];
            xml.WriteStartElement("borders");
            xml.WriteAttributeString("count", (distinctBorders.Count + 1).ToString()); // +1 default

            // BorderId = 0: nessun bordo (default)
            logger?.LogDebug("Adding default border (no border)");
            xml.WriteStartElement("border");
            xml.WriteElementString("left", "");
            xml.WriteElementString("right", "");
            xml.WriteElementString("top", "");
            xml.WriteElementString("bottom", "");
            xml.WriteElementString("diagonal", "");
            xml.WriteEndElement(); // </border>

            foreach (string borderColor in distinctBorders)
            {
                logger?.LogDebug("Adding border color: {BorderColor}", borderColor);
                xml.WriteStartElement("border");
                foreach (string side in new[] { "left", "right", "top", "bottom" })
                {
                    xml.WriteStartElement(side);
                    if (!string.IsNullOrWhiteSpace(borderColor))
                    {
                        xml.WriteAttributeString("style", "thin");
                        xml.WriteStartElement("color");
                        xml.WriteAttributeString("rgb", NormalizeColor(borderColor));
                        xml.WriteEndElement(); // </color>
                    }
                    xml.WriteEndElement(); // </side>
                }
                xml.WriteElementString("diagonal", "");
                xml.WriteEndElement(); // </border>
            }
            logger?.LogDebug("Finished writing borders.");
            xml.WriteEndElement(); // </borders>

            // === cellStyleXfs (default) ===
            logger?.LogDebug("Writing cellStyleXfs (default style)");
            xml.WriteStartElement("cellStyleXfs");
            xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("xf");
            xml.WriteAttributeString("numFmtId", "0");
            xml.WriteAttributeString("fontId", "0");
            xml.WriteAttributeString("fillId", "0");
            xml.WriteAttributeString("borderId", "0");
            xml.WriteEndElement(); // </xf>
            xml.WriteEndElement(); // </cellStyleXfs>

            // === cellXfs (effettivo mapping di StyleDefinition) ===
            xml.WriteStartElement("cellXfs");
            xml.WriteAttributeString("count", (styleBuilder.Styles.Count + 1).ToString());

            // first add an empty style (default)
            logger?.LogDebug("Adding default cellXfs (empty style)");
            xml.WriteStartElement("xf");
            xml.WriteAttributeString("numFmtId", "0");
            xml.WriteAttributeString("fontId", "0");
            xml.WriteAttributeString("fillId", "0");
            xml.WriteAttributeString("borderId", "0");
            xml.WriteAttributeString("xfId", "0");
            xml.WriteAttributeString("applyFont", "0");
            xml.WriteAttributeString("applyFill", "0");
            xml.WriteAttributeString("applyBorder", "0");
            xml.WriteEndElement(); // </xf>

            foreach (StyleDefinition style in styleBuilder.Styles)
            {
                logger?.LogDebug("Adding style: {StyleName}", styleBuilder.Styles.ToList().IndexOf(style).ToString() ?? "Unnamed Style");
                xml.WriteStartElement("xf");

                FontSettings safeFontSettings = ValidateFontSettings(UnsafeFontSettings.AsUnsafe(style.FontName, style.FontColor, style.FontSize, style.Bold));
                int fontId = distinctFontSettings.IndexOf(safeFontSettings);

                // first two fillsID are reserved for default fills (no fill and white fill)
                int fillId = string.IsNullOrWhiteSpace(style.FillColor) ? 0 : distinctFills.IndexOf(style.FillColor!) + 2; // +2 for default fills
                //int borderId = string.IsNullOrWhiteSpace(style.BorderColor) ? 0 : distinctBorders.IndexOf(style.BorderColor!) + 1;
                int borderId = 0;
                if (!string.IsNullOrWhiteSpace(style.BorderColor))
                {
                    int index = distinctBorders.IndexOf(style.BorderColor!);
                    if (index >= 0)
                        borderId = index + 1; // +1 per saltare il default
                }

                xml.WriteAttributeString("fontId", fontId.ToString());
                xml.WriteAttributeString("fillId", fillId.ToString());
                xml.WriteAttributeString("borderId", borderId.ToString());

                if (style.NumberFormat != null && numberFormats.TryGetValue(style.NumberFormat, out int fmtId))
                {
                    xml.WriteAttributeString("numFmtId", fmtId.ToString());
                    xml.WriteAttributeString("applyNumberFormat", "1");
                }
                else
                {
                    xml.WriteAttributeString("numFmtId", "0");
                }

                xml.WriteAttributeString("xfId", "0");
                xml.WriteAttributeString("applyFont", "1");
                xml.WriteAttributeString("applyFill", fillId > 1 ? "1" : "0");
                xml.WriteAttributeString("applyBorder", borderId > 0 ? "1" : "0");
                xml.WriteEndElement(); // </xf>
            }
            logger?.LogDebug("Finished writing cellXfs styles.");
            xml.WriteEndElement(); // </cellXfs>

            // === cellStyles (default) ===
            logger?.LogDebug("Writing cellStyles (default style)");
            xml.WriteStartElement("cellStyles");
            xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("cellStyle");
            xml.WriteAttributeString("name", "Normal");
            xml.WriteAttributeString("xfId", "0");
            xml.WriteAttributeString("builtinId", "0");
            xml.WriteEndElement();
            xml.WriteEndElement(); // </cellStyles>

            logger?.LogDebug("Finished writing cellStyles.");

            xml.WriteEndElement(); // </styleSheet>
            xml.WriteEndDocument();
            logger?.LogDebug("Styles XML written successfully.");
        }

        /// <summary>
        /// Generates and adds the styles XML file to the specified Excel workbook archive.
        /// </summary>
        /// <remarks>This method creates a `xl/styles.xml` entry in the provided Excel workbook archive
        /// and writes the styles definitions based on the provided <paramref name="styleBuilder"/>. The styles include
        /// fonts, fills, borders, and number formats, ensuring unique identifiers for each component. Default styles
        /// are also included to comply with the OpenXML specification.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> representing the Excel workbook to which the styles XML will be added.</param>
        /// <param name="styleBuilder">An <see cref="ExcelStyleIndexBuilder"/> containing the styles to be included in the styles XML.</param>
        /// <param name="options">A collection of <see cref="ExcelWorksheetOptions"/> that define additional formatting options for the
        /// styles.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the generation process.</param>
        public static void AddStylesXml(ZipArchive archive, ExcelStyleIndexBuilder styleBuilder, HashSet<ExcelWorksheetOptions> options, ILogger? logger = null)
        {
            logger?.LogDebug("Adding styles XML with {StyleCount} styles", styleBuilder.Styles.Count);
            ZipArchiveEntry stylesEntry = archive.CreateEntry("xl/styles.xml");

            using Stream stream = stylesEntry.Open();
            using XmlWriter xml = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });

            List<FontSettings> fontSettingsList = [];
            List<string> fills = []; // Hex colors
            List<string> borders = []; // Hex colors
            Dictionary<string, int> numberFormats = []; // FormatCode -> numFmtId

            // Create unique IDs per component
            logger?.LogDebug("Validating styles and creating unique IDs for fonts, fills, and borders.");
            foreach (StyleDefinition style in styleBuilder.Styles)
            {
                logger?.LogDebug("Processing style: {StyleName}", styleBuilder.Styles.ToList().IndexOf(style).ToString() ?? "Unnamed Style");
                FontSettings safeFontSettings = ValidateFontSettings(UnsafeFontSettings.AsUnsafe(style.FontName, style.FontColor, style.FontSize, style.Bold));
                ColorSettings safeColorSettings = ValidateColorSettings(UnsafeColorSettings.AsUnsafe(style.FillColor, style.BorderColor));

                fontSettingsList.Add(safeFontSettings);
                fills.Add(safeColorSettings.FillColor);
                borders.Add(safeColorSettings.BorderColor);

                if (!string.IsNullOrWhiteSpace(style.NumberFormat) && !numberFormats.ContainsKey(style.NumberFormat!))
                    numberFormats[style.NumberFormat!] = 165 + numberFormats.Count; // Custom numFmtId must be >= 164
            }

            // Start writing the XML document
            logger?.LogDebug("Starting to write styles XML document.");
            xml.WriteStartDocument();
            xml.WriteStartElement("styleSheet", ExcelDefaults.Namespaces.SpreadsheetML.NamespaceName);

            // === NumberFormats ===
            logger?.LogDebug("Writing number formats, count: {Count}", numberFormats.Count);
            xml.WriteStartElement("numFmts");
            xml.WriteAttributeString("count", numberFormats.Count.ToString());
            foreach (KeyValuePair<string, int> kvp in numberFormats)
            {
                logger?.LogDebug("Adding number format: {FormatCode} with ID {NumFmtId}", kvp.Key, kvp.Value);
                xml.WriteStartElement("numFmt");
                xml.WriteAttributeString("numFmtId", kvp.Value.ToString());
                xml.WriteAttributeString("formatCode", kvp.Key);
                xml.WriteEndElement(); // </numFmt>
            }
            logger?.LogDebug("Finished writing number formats.");
            xml.WriteEndElement(); // </numFmts>

            // === Fonts ===
            logger?.LogDebug("Writing fonts, count: {Count}", fontSettingsList.Count);
            List<FontSettings> distinctFontSettings = [.. fontSettingsList.Distinct()];
            xml.WriteStartElement("fonts");
            xml.WriteAttributeString("count", distinctFontSettings.Count.ToString());
            int i = 0;
            foreach (FontSettings setting in distinctFontSettings)
            {
                logger?.LogDebug("Adding font: {FontName}, Color: {FontColor}, Size: {FontSize}, Bold: {Bold}",
                    setting.FontName ?? "Default", setting.FontColor ?? "None", setting.FontSize, setting.Bold);

                //ExcelWorksheetOptions option = options.ElementAtOrDefault(i) ?? new ExcelWorksheetOptions();

                xml.WriteStartElement("font");
                if (setting.Bold)
                {
                    xml.WriteStartElement("b");
                    xml.WriteEndElement();
                }

                if (!string.IsNullOrWhiteSpace(setting.FontColor))
                {
                    xml.WriteStartElement("color");
                    xml.WriteAttributeString("rgb", NormalizeColor(setting.FontColor));
                    xml.WriteEndElement();
                }

                if (setting.FontSize > 0)
                {
                    //xml.WriteElementString("sz", setting.FontSize.ToString(options.Culture));
                    xml.WriteStartElement("sz");
                    xml.WriteAttributeString("val", setting.FontSize.ToString(CultureInfo.InvariantCulture));
                    xml.WriteEndElement(); // </sz>
                }

                if (!string.IsNullOrWhiteSpace(setting.FontName))
                {
                    xml.WriteStartElement("name");
                    xml.WriteAttributeString("val", setting.FontName);
                    xml.WriteEndElement();
                    //xml.WriteElementString("name", setting.FontName);
                }

                xml.WriteEndElement(); // </font>

                i++;
            }
            logger?.LogDebug("Finished writing fonts.");
            xml.WriteEndElement(); // </fonts>

            // === Fills ===
            logger?.LogDebug("Writing fills, count: {Count}", fills.Distinct().Count());
            List<string> distinctFills = [.. fills.Distinct()];
            xml.WriteStartElement("fills");
            xml.WriteAttributeString("count", (distinctFills.Count + 2).ToString()); // +2 default
            WriteDefaultFills(xml);
            foreach (string fillColor in distinctFills.Where(c => !string.IsNullOrWhiteSpace(c)))
            {
                logger?.LogDebug("Adding fill color: {FillColor}", fillColor);
                xml.WriteStartElement("fill");
                xml.WriteStartElement("patternFill");
                xml.WriteAttributeString("patternType", "solid");
                xml.WriteStartElement("fgColor");
                xml.WriteAttributeString("rgb", NormalizeColor(fillColor));
                xml.WriteEndElement(); // </fgColor>
                xml.WriteElementString("bgColor", "indexed", "64");
                xml.WriteEndElement(); // </patternFill>
                xml.WriteEndElement(); // </fill>
            }
            logger?.LogDebug("Finished writing fills.");
            xml.WriteEndElement(); // </fills>

            // === Borders ===
            logger?.LogDebug("Writing borders, count: {Count}", borders.Distinct().Count());
            List<string> distinctBorders = [.. borders.Distinct()];
            xml.WriteStartElement("borders");
            xml.WriteAttributeString("count", (distinctBorders.Count + 1).ToString()); // +1 default

            // BorderId = 0: nessun bordo (default)
            logger?.LogDebug("Adding default border (no border)");
            xml.WriteStartElement("border");
            xml.WriteElementString("left", "");
            xml.WriteElementString("right", "");
            xml.WriteElementString("top", "");
            xml.WriteElementString("bottom", "");
            xml.WriteElementString("diagonal", "");
            xml.WriteEndElement(); // </border>

            foreach (string borderColor in distinctBorders)
            {
                logger?.LogDebug("Adding border color: {BorderColor}", borderColor);
                xml.WriteStartElement("border");
                foreach (string side in new[] { "left", "right", "top", "bottom" })
                {
                    xml.WriteStartElement(side);
                    if (!string.IsNullOrWhiteSpace(borderColor))
                    {
                        xml.WriteAttributeString("style", "thin");
                        xml.WriteStartElement("color");
                        xml.WriteAttributeString("rgb", NormalizeColor(borderColor));
                        xml.WriteEndElement(); // </color>
                    }
                    xml.WriteEndElement(); // </side>
                }
                xml.WriteElementString("diagonal", "");
                xml.WriteEndElement(); // </border>
            }
            logger?.LogDebug("Finished writing borders.");
            xml.WriteEndElement(); // </borders>

            // === cellStyleXfs (default) ===
            logger?.LogDebug("Writing cellStyleXfs (default style)");
            xml.WriteStartElement("cellStyleXfs");
            xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("xf");
            xml.WriteAttributeString("numFmtId", "0");
            xml.WriteAttributeString("fontId", "0");
            xml.WriteAttributeString("fillId", "0");
            xml.WriteAttributeString("borderId", "0");
            xml.WriteEndElement(); // </xf>
            xml.WriteEndElement(); // </cellStyleXfs>

            // === cellXfs (effettivo mapping di StyleDefinition) ===
            xml.WriteStartElement("cellXfs");
            xml.WriteAttributeString("count", (styleBuilder.Styles.Count + 1).ToString());

            // first add an empty style (default)
            logger?.LogDebug("Adding default cellXfs (empty style)");
            xml.WriteStartElement("xf");
            xml.WriteAttributeString("numFmtId", "0");
            xml.WriteAttributeString("fontId", "0");
            xml.WriteAttributeString("fillId", "0");
            xml.WriteAttributeString("borderId", "0");
            xml.WriteAttributeString("xfId", "0");
            xml.WriteAttributeString("applyFont", "0");
            xml.WriteAttributeString("applyFill", "0");
            xml.WriteAttributeString("applyBorder", "0");
            xml.WriteEndElement(); // </xf>

            foreach (StyleDefinition style in styleBuilder.Styles)
            {
                logger?.LogDebug("Adding style: {StyleName}", styleBuilder.Styles.ToList().IndexOf(style).ToString() ?? "Unnamed Style");
                xml.WriteStartElement("xf");

                FontSettings safeFontSettings = ValidateFontSettings(UnsafeFontSettings.AsUnsafe(style.FontName, style.FontColor, style.FontSize, style.Bold));
                int fontId = distinctFontSettings.IndexOf(safeFontSettings);

                // first two fillsID are reserved for default fills (no fill and white fill)
                int fillId = string.IsNullOrWhiteSpace(style.FillColor) ? 0 : distinctFills.IndexOf(style.FillColor!) + 2; // +2 for default fills
                //int borderId = string.IsNullOrWhiteSpace(style.BorderColor) ? 0 : distinctBorders.IndexOf(style.BorderColor!) + 1;
                int borderId = 0;
                if (!string.IsNullOrWhiteSpace(style.BorderColor))
                {
                    int index = distinctBorders.IndexOf(style.BorderColor!);
                    if (index >= 0)
                        borderId = index + 1; // +1 per saltare il default
                }

                xml.WriteAttributeString("fontId", fontId.ToString());
                xml.WriteAttributeString("fillId", fillId.ToString());
                xml.WriteAttributeString("borderId", borderId.ToString());

                if (style.NumberFormat != null && numberFormats.TryGetValue(style.NumberFormat, out int fmtId))
                {
                    xml.WriteAttributeString("numFmtId", fmtId.ToString());
                    xml.WriteAttributeString("applyNumberFormat", "1");
                }
                else
                {
                    xml.WriteAttributeString("numFmtId", "0");
                }

                xml.WriteAttributeString("xfId", "0");
                xml.WriteAttributeString("applyFont", "1");
                xml.WriteAttributeString("applyFill", fillId > 1 ? "1" : "0");
                xml.WriteAttributeString("applyBorder", borderId > 0 ? "1" : "0");
                xml.WriteEndElement(); // </xf>
            }
            logger?.LogDebug("Finished writing cellXfs styles.");
            xml.WriteEndElement(); // </cellXfs>

            // === cellStyles (default) ===
            logger?.LogDebug("Writing cellStyles (default style)");
            xml.WriteStartElement("cellStyles");
            xml.WriteAttributeString("count", "1");
            xml.WriteStartElement("cellStyle");
            xml.WriteAttributeString("name", "Normal");
            xml.WriteAttributeString("xfId", "0");
            xml.WriteAttributeString("builtinId", "0");
            xml.WriteEndElement();
            xml.WriteEndElement(); // </cellStyles>

            logger?.LogDebug("Finished writing cellStyles.");
            xml.WriteEndElement(); // </styleSheet>
            xml.WriteEndDocument();
            logger?.LogDebug("Styles XML written successfully.");
        }

        /// <summary>
        /// Adds a [Content_Types].xml file to the specified ZIP archive, defining content types for the package.
        /// </summary>
        /// <remarks>The [Content_Types].xml file is a required part of Open Packaging Conventions (OPC)
        /// packages, such as those used in Office Open XML documents. This method defines default content types for
        /// `.rels` and `.xml` files, as well as overrides for specific parts of an Excel workbook, including the
        /// workbook, worksheets, styles, and shared strings.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the [Content_Types].xml file will be added.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information. If <see langword="null"/>, no
        /// logging will occur.</param>
        public static void AddContentTypesXml(ZipArchive archive, ILogger? logger = null)
        {
            logger?.LogDebug("Adding [Content_Types].xml to the archive.");
            ZipArchiveEntry entry = archive.CreateEntry("[Content_Types].xml");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("Types", "http://schemas.openxmlformats.org/package/2006/content-types");

            // Default types
            logger?.LogDebug("Adding default content types for .rels and .xml files.");
            xml.WriteStartElement("Default");
            xml.WriteAttributeString("Extension", "rels");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.relationships+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Default");
            xml.WriteAttributeString("Extension", "xml");
            xml.WriteAttributeString("ContentType", "application/xml");
            xml.WriteEndElement();

            // Override parts
            logger?.LogDebug("Adding overrides for main parts of the Excel file.");
            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/workbook.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", $"/{ExcelDefaults.Worksheets.Sheet1PathName}");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/styles.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/sharedStrings.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml");
            xml.WriteEndElement();

            xml.WriteEndElement(); // </Types>
            xml.WriteEndDocument();
            logger?.LogDebug("[Content_Types].xml added successfully.");
        }

        /// <summary>
        /// Adds a [Content_Types].xml file to the specified ZIP archive, defining content types for the Excel file
        /// structure.
        /// </summary>
        /// <remarks>This method generates a [Content_Types].xml file that specifies default content types
        /// for .rels and .xml files,  as well as overrides for specific parts of the Excel file, such as the workbook,
        /// worksheets, styles, and shared strings. The method ensures that the content types are correctly defined for
        /// compatibility with the Open XML format.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the [Content_Types].xml file will be added.</param>
        /// <param name="worksheets">A collection of worksheets to include in the content type overrides. Each worksheet corresponds to a part in
        /// the Excel file.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation. If null, no
        /// logging will occur.</param>
        public static void AddContentTypesXml(ZipArchive archive, IEnumerable<IExcelWorksheet> worksheets, ILogger? logger = null)
        {
            logger?.LogDebug("Adding [Content_Types].xml to the archive with {WorksheetCount} worksheets.", worksheets.Count());
            ZipArchiveEntry entry = archive.CreateEntry("[Content_Types].xml");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("Types", "http://schemas.openxmlformats.org/package/2006/content-types");

            // Default types
            logger?.LogDebug("Adding default content types for .rels and .xml files.");
            xml.WriteStartElement("Default");
            xml.WriteAttributeString("Extension", "rels");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-package.relationships+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Default");
            xml.WriteAttributeString("Extension", "xml");
            xml.WriteAttributeString("ContentType", "application/xml");
            xml.WriteEndElement();

            // Override parts
            logger?.LogDebug("Adding overrides for main parts of the Excel file.");
            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/workbook.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
            xml.WriteEndElement();

            int index = 1;
            foreach (IExcelWorksheet _ in worksheets)
            {
                logger?.LogDebug("Adding override for worksheet sheet{Index}.xml", index);
                xml.WriteStartElement("Override");
                xml.WriteAttributeString("PartName", $"/xl/worksheets/sheet{index}.xml");
                xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
                xml.WriteEndElement();

                index++;
            }

            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/styles.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Override");
            xml.WriteAttributeString("PartName", "/xl/sharedStrings.xml");
            xml.WriteAttributeString("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml");
            xml.WriteEndElement();

            xml.WriteEndElement(); // </Types>
            xml.WriteEndDocument();
            logger?.LogDebug("[Content_Types].xml added successfully.");
        }

        /// <summary>
        /// Adds a `_rels/.rels` file to the specified ZIP archive, defining relationships for an Office Open XML
        /// package.
        /// </summary>
        /// <remarks>The `_rels/.rels` file is a required part of an Office Open XML package and defines
        /// the relationship to the main document (e.g., `xl/workbook.xml` for an Excel workbook). This method creates
        /// the file with a predefined relationship pointing to the main document.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the `_rels/.rels` file will be added. Cannot be null.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information. If null, no logging will occur.</param>
        public static void AddRels(ZipArchive archive, ILogger? logger = null)
        {
            logger?.LogDebug("Adding _rels/.rels to the archive.");
            ZipArchiveEntry entry = archive.CreateEntry("_rels/.rels");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", "rId1");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
            xml.WriteAttributeString("Target", "xl/workbook.xml");
            xml.WriteEndElement();
            xml.WriteEndElement();
            xml.WriteEndDocument();
            logger?.LogDebug("_rels/.rels added successfully.");
        }

        /// <summary>
        /// Adds the workbook relationships file to the specified ZIP archive.
        /// </summary>
        /// <remarks>This method creates a file at the path <c>xl/_rels/workbook.xml.rels</c> within the
        /// provided ZIP archive. The file defines relationships for the workbook, including references to the
        /// worksheet, styles, and shared strings files.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the workbook relationships file will be added. Cannot be <c>null</c>.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information. If <c>null</c>, no logging will
        /// occur.</param>
        public static void AddWorkbookRels(ZipArchive archive, ILogger? logger = null)
        {
            logger?.LogDebug("Adding xl/_rels/workbook.xml.rels to the archive.");
            ZipArchiveEntry entry = archive.CreateEntry("xl/_rels/workbook.xml.rels");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            logger?.LogDebug("Writing workbook relationships XML document.");
            xml.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");

            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", "rId1");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
            xml.WriteAttributeString("Target", "worksheets/sheet1.xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", "rId2");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
            xml.WriteAttributeString("Target", "styles.xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", "rId3");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings");
            xml.WriteAttributeString("Target", "sharedStrings.xml");
            xml.WriteEndElement();

            xml.WriteEndElement(); // </Relationships>
            xml.WriteEndDocument();
            logger?.LogDebug("xl/_rels/workbook.xml.rels added successfully.");
        }

        /// <summary>
        /// Adds the workbook relationships file (xl/_rels/workbook.xml.rels) to the specified archive.
        /// </summary>
        /// <remarks>This method generates the workbook relationships file required for an Excel workbook,
        /// including relationships for each worksheet, the styles file, and the shared strings file. The relationships
        /// are written in compliance with the Open XML format.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the workbook relationships file will be added.</param>
        /// <param name="sheets">A collection of worksheets to include in the relationships file. Each worksheet will have a corresponding
        /// relationship entry.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation. If null, no
        /// logging will occur.</param>
        public static void AddWorkbookRels(ZipArchive archive, IEnumerable<IExcelWorksheet> sheets, ILogger? logger = null)
        {
            logger?.LogDebug("Adding xl/_rels/workbook.xml.rels to the archive with {SheetCount} sheets.", sheets.Count());
            ZipArchiveEntry entry = archive.CreateEntry("xl/_rels/workbook.xml.rels");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            logger?.LogDebug("Writing workbook relationships XML document.");
            xml.WriteStartElement("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");

            int index = 1;
            foreach (IExcelWorksheet sheet in sheets)
            {
                logger?.LogDebug("Adding relationship for worksheet sheet{Index}.xml", index);
                xml.WriteStartElement("Relationship");
                xml.WriteAttributeString("Id", $"rId{index}");
                xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
                xml.WriteAttributeString("Target", $"worksheets/sheet{index}.xml");
                xml.WriteEndElement();
                index++;
            }

            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", $"rId{index}");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles");
            xml.WriteAttributeString("Target", "styles.xml");
            xml.WriteEndElement();

            xml.WriteStartElement("Relationship");
            xml.WriteAttributeString("Id", $"rId{index + 1}");
            xml.WriteAttributeString("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings");
            xml.WriteAttributeString("Target", "sharedStrings.xml");
            xml.WriteEndElement();

            xml.WriteEndElement(); // </Relationships>
            xml.WriteEndDocument();
            logger?.LogDebug("xl/_rels/workbook.xml.rels added successfully.");
        }

        /// <summary>
        /// Adds a workbook XML file to the specified ZIP archive, representing the structure of an Excel workbook.
        /// </summary>
        /// <remarks>This method creates a new entry in the ZIP archive at the path "xl/workbook.xml" and
        /// writes the XML content  for an Excel workbook. The XML includes a single sheet with the specified or default
        /// name and a relationship ID of "rId1".</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the workbook XML file will be added. Cannot be <see langword="null"/>.</param>
        /// <param name="sheetName">An optional <see cref="SheetName"/> representing the name of the first sheet in the workbook.  If <see
        /// langword="null"/>, the default name "Sheet1" will be used.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation.  If <see
        /// langword="null"/>, no logging will occur.</param>
        public static void AddWorkbookXml(ZipArchive archive, SheetName? sheetName = null, ILogger? logger = null)
        {
            logger?.LogDebug("Adding xl/workbook.xml to the archive with sheet name: {SheetName}", sheetName?.ToString() ?? "Sheet1");
            ZipArchiveEntry entry = archive.CreateEntry("xl/workbook.xml");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

            // questo lancia exception per il name
            //xml.WriteAttributeString("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")
            logger?.LogDebug("Writing workbook XML document with namespace for relationships.");
            xml.WriteAttributeString("xmlns", "r", null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            logger?.LogDebug("Writing sheets element with sheet name: {SheetName}", sheetName?.ToString() ?? "Sheet1");
            xml.WriteStartElement("sheets");
            xml.WriteStartElement("sheet");
            xml.WriteAttributeString("name", sheetName.ToString() ?? "Sheet1");
            xml.WriteAttributeString("sheetId", "1");
            //xml.WriteAttributeString("r:id", "rId1")
            logger?.LogDebug("Writing relationship ID for the sheet: rId1");
            xml.WriteAttributeString("r", "id", null, "rId1");

            xml.WriteEndElement(); // </sheet>
            xml.WriteEndElement(); // </sheets>

            xml.WriteEndElement(); // </workbook>
            xml.WriteEndDocument();
            logger?.LogDebug("xl/workbook.xml added successfully.");
        }

        /// <summary>
        /// Adds a workbook definition XML file to the specified ZIP archive, representing the structure of an Excel
        /// workbook.
        /// </summary>
        /// <remarks>This method creates an entry named "xl/workbook.xml" in the provided ZIP archive. The
        /// XML file defines the workbook structure, including the names and IDs of the sheets. Each sheet is assigned a
        /// unique ID starting from 1, and its name is taken from the <see cref="IExcelWorksheet.Name"/> property. If a
        /// sheet name is null, a default name in the format "Sheet{index}" will be used, where {index} is the sheet's
        /// position in the collection.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the workbook XML file will be added. Must not be null.</param>
        /// <param name="sheets">A collection of <see cref="IExcelWorksheet"/> objects representing the sheets in the workbook. Each sheet
        /// must have a unique name.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation. If null, no
        /// logging will occur.</param>
        public static void AddWorkbookXml(ZipArchive archive, IEnumerable<IExcelWorksheet> sheets, ILogger? logger = null)
        {
            logger?.LogDebug("Adding xl/workbook.xml to the archive with {SheetCount} sheets.", sheets.Count());
            ZipArchiveEntry entry = archive.CreateEntry("xl/workbook.xml");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            logger?.LogDebug("Writing workbook XML document with namespace for relationships.");
            xml.WriteAttributeString("xmlns", "r", null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

            logger?.LogDebug("Writing sheets element with {SheetCount} sheets.", sheets.Count());
            xml.WriteStartElement("sheets");

            int index = 1;
            foreach (SheetName sheetName in sheets.Select(s => s.Name))
            {
                logger?.LogDebug("Adding sheet: {SheetName} with ID {SheetId}", sheetName.ToString() ?? $"Sheet{index}", index);
                xml.WriteStartElement("sheet");
                xml.WriteAttributeString("name", sheetName.ToString() ?? $"Sheet{index}");
                xml.WriteAttributeString("sheetId", index.ToString());
                xml.WriteAttributeString("r", "id", null, $"rId{index}");
                xml.WriteEndElement(); // </sheet>
                index++;
            }

            xml.WriteEndElement(); // </sheets>
            xml.WriteEndElement(); // </workbook>
            xml.WriteEndDocument();
            logger?.LogDebug("xl/workbook.xml added successfully.");
        }

        /// <summary>
        /// Adds a shared strings XML file to the specified ZIP archive.
        /// </summary>
        /// <remarks>The method creates an entry named <c>xl/sharedStrings.xml</c> in the provided ZIP
        /// archive. The XML file includes all shared strings from the <paramref name="sharedStrings"/> object, along
        /// with their total count and unique count as attributes in the root element.</remarks>
        /// <param name="archive">The <see cref="ZipArchive"/> to which the shared strings XML file will be added.</param>
        /// <param name="sharedStrings">A <see cref="SharedStringBuilder"/> containing the shared strings to include in the XML file. The shared
        /// strings are written in the order they are retrieved from the builder.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging debug information during the operation. If <see
        /// langword="null"/>, no logging will occur.</param>
        public static void AddSharedStringsXml(ZipArchive archive, SharedStringBuilder sharedStrings, ILogger? logger = null)
        {
            logger?.LogDebug("Adding xl/sharedStrings.xml to the archive with {StringCount} strings.", sharedStrings.UniqueCount);
            ZipArchiveEntry entry = archive.CreateEntry("xl/sharedStrings.xml");
            using StreamWriter writer = new(entry.Open());
            using XmlWriter xml = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });

            xml.WriteStartDocument();
            xml.WriteStartElement("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            xml.WriteAttributeString("count", sharedStrings.Count.ToString());
            xml.WriteAttributeString("uniqueCount", sharedStrings.UniqueCount.ToString());

            foreach (string s in sharedStrings.GetOrderedStrings())
            {
                logger?.LogDebug("Adding shared string: {StringValue}", s);
                xml.WriteStartElement("si");
                xml.WriteElementString("t", s);
                xml.WriteEndElement();
            }

            xml.WriteEndElement(); // </sst>
            xml.WriteEndDocument();
            logger?.LogDebug("xl/sharedStrings.xml added successfully.");
        }

        #region private methods
        private static FontSettings ValidateFontSettings(UnsafeFontSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.FontName) && string.IsNullOrWhiteSpace(settings.FontColor))
                throw new InvalidFontSettingException("At least one of FontName or FontColor must be specified.");

            if (string.IsNullOrWhiteSpace(settings.FontName))
                throw new InvalidFontSettingException("FontName cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(settings.FontColor))
                throw new InvalidFontSettingException("FontName cannot be null or empty.");

            if (settings.FontSize <= 0)
                throw new InvalidFontSettingException("FontSize must be greater than zero.");

            return new FontSettings(
                settings.FontName!,
                settings.FontColor!,
                settings.FontSize,
                settings.Bold);
        }

        private static ColorSettings ValidateColorSettings(UnsafeColorSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.FillColor) && string.IsNullOrWhiteSpace(settings.BorderColor))
                throw new InvalidColorSettingException("At least one of FillColor or BorderColor must be specified.");

            if (string.IsNullOrWhiteSpace(settings.FillColor))
                throw new InvalidColorSettingException("FillColor cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(settings.BorderColor))
                throw new InvalidColorSettingException("BorderColor cannot be null or empty.");

            return new ColorSettings(
                settings.FillColor!,
                settings.BorderColor!);
        }

        private static string GetCellReference(int rowIndex, int colIndex)
        {
            // 65 is the ASCII code for 'A'
            // 26 is the number of letters in the English alphabet
            // dividend is 1-based index, so we add 1 to colIndex

            int dividend = colIndex + 1;
            StringBuilder columnName = new();
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName.Insert(0, Convert.ToChar(65 + modulo));
                dividend = (dividend - modulo) / 26;
            }
            return columnName.ToString() + rowIndex;
        }

        private static XElement CreateCellXml(object? value, PropertyName propName, SharedStringBuilder? sharedStrings, ExcelWriterOptions excelWriterOptions)
        {
            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;

            if (value == null)
                return new XElement(ns + "c");

            Type type = value.GetType();
            XElement cell = new(ns + "c");

            // STRING
            if (value is string str)
            {
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(str);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", str)));
                }

                return cell;
            }

            // BOOL
            if (value is bool b)
            {
                bool alreadyHandled = false;
                if (excelWriterOptions.MultipleTrueFalseWords?.Count > 0 && excelWriterOptions.MultipleTrueFalseWords.TryGetWords(propName, out TrueFalseWords words))
                {
                    string boolStr = b ? words.TrueWord : words.FalseWord;
                    if (sharedStrings != null)
                    {
                        int index = sharedStrings.TryGetOrAddIndex(boolStr);
                        cell.SetAttributeValue("t", "s");
                        cell.Add(new XElement(ns + "v", index));
                    }
                    else
                    {
                        cell.SetAttributeValue("t", "inlineStr");
                        cell.Add(new XElement(ns + "is", new XElement(ns + "t", boolStr)));
                    }

                    alreadyHandled = true;
                }

                if (!alreadyHandled)
                {
                    if (excelWriterOptions.HasTrueFalseWords && excelWriterOptions.TrueFalseWords.HasValue)
                    {
                        TrueFalseWords a = excelWriterOptions.TrueFalseWords.Value;
                        string boolStr = b ? a.TrueWord : a.FalseWord;

                        if (sharedStrings != null)
                        {
                            int index = sharedStrings.TryGetOrAddIndex(boolStr);
                            cell.SetAttributeValue("t", "s");
                            cell.Add(new XElement(ns + "v", index));
                        }
                        else
                        {
                            cell.SetAttributeValue("t", "inlineStr");
                            cell.Add(new XElement(ns + "is", new XElement(ns + "t", boolStr)));
                        }
                    }
                    else
                    {
                        // Default boolean representation as numeric (1 for true, 0 for false)
                        cell.SetAttributeValue("t", "b");
                        cell.Add(new XElement(ns + "v", b ? "1" : "0"));
                    }
                }

                return cell;
            }

            // DATETIME
            if (value is DateTime dt)
            {
                double oaDate = dt.ToOADate();
                cell.SetAttributeValue("t", "n");
                cell.Add(new XElement(ns + "v", oaDate.ToString(excelWriterOptions.Culture)));
                return cell;
            }

            // NUMERIC
            if (value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal)
            {
                string num = Convert.ToString(value, excelWriterOptions.Culture) ?? "0";
                cell.SetAttributeValue("t", "n");
                cell.Add(new XElement(ns + "v", num));
                return cell;
            }

            // GUID (as string)
            if (value is Guid guid)
            {
                string strVal = guid.ToString();
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(strVal);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", strVal)));
                }

                return cell;
            }

            // TIMESPAN (formatted)
            if (value is TimeSpan ts)
            {
                string duration = ts.ToString("c", excelWriterOptions.Culture); // formato ISO (hh:mm:ss)
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(duration);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", duration)));
                }

                return cell;
            }

            // ENUM (name string or numeric?)
            if (type.IsEnum)
            {
                string enumStr = Enum.GetName(type, value) ?? value.ToString()!;
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(enumStr);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", enumStr)));
                }

                return cell;
            }

            // DEFAULT: fallback
            string fallback = value.ToString() ?? string.Empty;
            if (sharedStrings != null)
            {
                int index = sharedStrings.TryGetOrAddIndex(fallback);
                cell.SetAttributeValue("t", "s");
                cell.Add(new XElement(ns + "v", index));
            }
            else
            {
                cell.SetAttributeValue("t", "inlineStr");
                cell.Add(new XElement(ns + "is", new XElement(ns + "t", fallback)));
            }

            return cell;
        }

        private static XElement CreateCellXml(object? value, string propertyName, SharedStringBuilder? sharedStrings, ExcelWorksheetOptions excelWriterOptions)
        {
            XNamespace ns = ExcelDefaults.Namespaces.SpreadsheetML;

            if (value == null)
                return new XElement(ns + "c");

            Type type = value.GetType();
            XElement cell = new(ns + "c");

            // STRING
            if (value is string str)
            {
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(str);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", str)));
                }

                return cell;
            }

            // BOOL
            if (value is bool b)
            {
                bool alreadyHandled = false;

                if (excelWriterOptions.MultipleTrueFalseWords?.Count > 0 && excelWriterOptions.MultipleTrueFalseWords.TryGetWords(propertyName, out TrueFalseWords words))
                {
                    string boolStr = b ? words.TrueWord : words.FalseWord;
                    if (sharedStrings != null)
                    {
                        int index = sharedStrings.TryGetOrAddIndex(boolStr);
                        cell.SetAttributeValue("t", "s");
                        cell.Add(new XElement(ns + "v", index));
                    }
                    else
                    {
                        cell.SetAttributeValue("t", "inlineStr");
                        cell.Add(new XElement(ns + "is", new XElement(ns + "t", boolStr)));
                    }

                    alreadyHandled = true;
                }

                if (!alreadyHandled)
                {
                    if (excelWriterOptions.HasTrueFalseWords && excelWriterOptions.TrueFalseWords.HasValue)
                    {
                        TrueFalseWords a = excelWriterOptions.TrueFalseWords.Value;
                        string boolStr = b ? a.TrueWord : a.FalseWord;

                        if (sharedStrings != null)
                        {
                            int index = sharedStrings.TryGetOrAddIndex(boolStr);
                            cell.SetAttributeValue("t", "s");
                            cell.Add(new XElement(ns + "v", index));
                        }
                        else
                        {
                            cell.SetAttributeValue("t", "inlineStr");
                            cell.Add(new XElement(ns + "is", new XElement(ns + "t", boolStr)));
                        }
                    }
                    else
                    {
                        // Default boolean representation as numeric (1 for true, 0 for false)
                        cell.SetAttributeValue("t", "b");
                        cell.Add(new XElement(ns + "v", b ? "1" : "0"));
                    }
                }

                return cell;
            }

            // DATETIME
            if (value is DateTime dt)
            {
                double oaDate = dt.ToOADate();
                cell.SetAttributeValue("t", "n");
                cell.Add(new XElement(ns + "v", oaDate.ToString(excelWriterOptions.Culture)));
                return cell;
            }

            // NUMERIC
            if (value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal)
            {
                string num = Convert.ToString(value, excelWriterOptions.Culture) ?? "0";
                cell.SetAttributeValue("t", "n");
                cell.Add(new XElement(ns + "v", num));
                return cell;
            }

            // GUID (as string)
            if (value is Guid guid)
            {
                string strVal = guid.ToString();
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(strVal);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", strVal)));
                }

                return cell;
            }

            // TIMESPAN (formatted)
            if (value is TimeSpan ts)
            {
                string duration = ts.ToString("c", excelWriterOptions.Culture); // formato ISO (hh:mm:ss)
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(duration);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", duration)));
                }

                return cell;
            }

            // ENUM (name string or numeric?)
            if (type.IsEnum)
            {
                string enumStr = Enum.GetName(type, value) ?? value.ToString()!;
                if (sharedStrings != null)
                {
                    int index = sharedStrings.TryGetOrAddIndex(enumStr);
                    cell.SetAttributeValue("t", "s");
                    cell.Add(new XElement(ns + "v", index));
                }
                else
                {
                    cell.SetAttributeValue("t", "inlineStr");
                    cell.Add(new XElement(ns + "is", new XElement(ns + "t", enumStr)));
                }

                return cell;
            }

            // DEFAULT: fallback
            string fallback = value.ToString() ?? string.Empty;
            if (sharedStrings != null)
            {
                int index = sharedStrings.TryGetOrAddIndex(fallback);
                cell.SetAttributeValue("t", "s");
                cell.Add(new XElement(ns + "v", index));
            }
            else
            {
                cell.SetAttributeValue("t", "inlineStr");
                cell.Add(new XElement(ns + "is", new XElement(ns + "t", fallback)));
            }

            return cell;
        }

        private static int? TryResolveStyle(ExcelWriterOptions options, ExcelStyleIndexBuilder builder, int row, int col, PropertyInfo prop, object? value)
        {
            ExcelCellStyle? style = options.StyleSelector?.SelectStyle(row, col, prop, value)
                ?? ExcelStyleSelector.FromOptions(row, options.StyleOptions);

            if (style == null)
                return null;

            if (value is DateTime)
            {
                style.CurrentCellNumberFormat = style.DateFormat
                    ?? style.IntegerNumberFormat
                    ?? options.StyleOptions?.DateFormat
                    ?? "yyyy-mm-dd";
            }
            else if (value is double or float or decimal)
            {
                style.CurrentCellNumberFormat = style.DecimalNumberFormat
                    ?? style.IntegerNumberFormat
                    ?? options.StyleOptions?.DecimalNumberFormat
                    ?? options.StyleOptions?.IntegerNumberFormat
                    ?? ExcelDefaults.NumberFormats.Decimals.Default; // default decimal format
            }
            else if (value is byte or sbyte or short or ushort or int or uint or long or ulong)
            {
                style.CurrentCellNumberFormat = style.IntegerNumberFormat
                    ?? options.StyleOptions?.IntegerNumberFormat
                    ?? ExcelDefaults.NumberFormats.Integers.Default; // default integer format
            }

            return style is not null ? builder.GetOrAddIndex(StyleDefinition.FromCellStyle(style)) : null;
        }

        private static int? TryResolveStyle(ExcelWorksheetOptions options, ExcelStyleIndexBuilder builder, int row, int col, PropertyInfo prop, object? value)
        {
            ExcelCellStyle? style = options.StyleSelector?.SelectStyle(row, col, prop, value)
                ?? ExcelStyleSelector.FromOptions(row, options.StyleOptions);

            if (style == null)
                return null;

            if (value is DateTime)
            {
                style.CurrentCellNumberFormat = style.DateFormat
                    ?? style.IntegerNumberFormat
                    ?? options.StyleOptions?.DateFormat
                    ?? "yyyy-mm-dd";
            }
            else if (value is double or float or decimal)
            {
                style.CurrentCellNumberFormat = style.DecimalNumberFormat
                    ?? style.IntegerNumberFormat
                    ?? options.StyleOptions?.DecimalNumberFormat
                    ?? options.StyleOptions?.IntegerNumberFormat
                    ?? ExcelDefaults.NumberFormats.Decimals.Default; // default decimal format
            }
            else if (value is byte or sbyte or short or ushort or int or uint or long or ulong)
            {
                style.CurrentCellNumberFormat = style.IntegerNumberFormat
                    ?? options.StyleOptions?.IntegerNumberFormat
                    ?? ExcelDefaults.NumberFormats.Integers.Default; // default integer format
            }

            return style is not null ? builder.GetOrAddIndex(StyleDefinition.FromCellStyle(style)) : null;
        }

        private static string NormalizeColor(string? hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) return "FF000000"; // default black

            string cleaned = hex!.Replace("#", "").Trim();
            if (cleaned.Length == 6) return $"FF{cleaned}";
            if (cleaned.Length == 8) return cleaned;
            return "FF000000"; // fallback
        }

        private static void WriteDefaultFills(XmlWriter xml)
        {
            // Required default fills: 'none' and 'gray125'
            xml.WriteStartElement("fill");
            xml.WriteStartElement("patternFill");
            xml.WriteAttributeString("patternType", "none");
            xml.WriteEndElement();
            xml.WriteEndElement();

            xml.WriteStartElement("fill");
            xml.WriteStartElement("patternFill");
            xml.WriteAttributeString("patternType", "gray125");
            xml.WriteEndElement();
            xml.WriteEndElement();
        }
        #endregion
    }
}
