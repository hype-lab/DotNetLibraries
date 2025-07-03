using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers;
using HypeLab.IO.Core.Helpers.Excel;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides functionality for generating Excel files from data collections.
    /// </summary>
    /// <remarks>The <see cref="ExcelWriter"/> class offers methods to create Excel files in the OpenXML
    /// format. It supports writing data to a file or returning the generated content as a byte array. The class is
    /// designed to handle various customization options, such as styles and shared strings, through the <see
    /// cref="ExcelWriterOptions"/> parameter. Logging is also supported to track the writing process and handle
    /// errors.</remarks>
    public static class ExcelWriter
    {
        /// <summary>
        /// Writes the specified data to an Excel file at the given file path.
        /// </summary>
        /// <remarks>This method creates a new Excel file in the OpenXML format and writes the provided
        /// data to it. The method supports customization through the <paramref name="options"/> parameter, including
        /// the ability to define styles and use shared strings for optimization. If logging is enabled via the
        /// <paramref name="logger"/> parameter, detailed information about the process will be logged.</remarks>
        /// <typeparam name="T">The type of the data to write. Must be a reference type.</typeparam>
        /// <param name="data">The collection of data items to be written to the Excel file. Each item represents a row in the worksheet.</param>
        /// <param name="filePath">The full path of the file to create. If the file already exists, it will be overwritten.</param>
        /// <param name="options">Optional configuration for customizing the Excel file, such as sheet name, style options, and shared string
        /// usage. If not provided, default options will be used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during the file writing process. If not
        /// provided, no logging will occur.</param>
        /// <exception cref="ExcelWriterException">Thrown if an error occurs while writing the Excel file.</exception>
        public static void WriteFile<T>(IEnumerable<T> data, string filePath, ExcelWriterOptions? options = null, ILogger? logger = null)
            where T : class
        {
            try
            {
                options ??= new ExcelWriterOptions();

                using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
                using ZipArchive archive = new(fs, ZipArchiveMode.Create);

                if (options.StyleOptions is null && options.StyleSelector == null)
                    options.StyleOptions = ExcelStyleOptions.Empty;

                SharedStringBuilder sharedStringBuilder = new();
                ExcelStyleIndexBuilder styleIndexBuilder = new();
                // cartelle e file minimi obbligatori
                ExcelWriterHelper.AddContentTypesXml(archive, logger);
                ExcelWriterHelper.AddRels(archive, logger);
                ExcelWriterHelper.AddWorkbookRels(archive, logger);
                ExcelWriterHelper.AddWorkbookXml(archive, options.SheetName, logger);

                // sheet
                ExcelWriterHelper.AddWorksheetXml(archive, data, options, sharedStringBuilder, styleIndexBuilder, logger);

                // styles.xml
                ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, options, logger);

                // se usiamo shared strings
                if (options.UseSharedStrings && sharedStringBuilder.UniqueCount > 0)
                    ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
            }
            catch (Exception ex)
            {
                string msg = $"Errors while writing the Excel.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Message}", msg);
                throw new ExcelWriterException(msg, ex);
            }
        }

        /// <summary>
        /// Writes data to an Excel file asynchronously, creating a new file at the specified path.
        /// </summary>
        /// <remarks>This method creates an Excel file in the OpenXML format and writes the provided data
        /// to a single worksheet. The file is created with default or user-specified options, including styles and
        /// shared string usage.  If <paramref name="options"/> specifies the use of shared strings, a shared string
        /// table will be included in the file. The method processes data in batches, as specified by <paramref
        /// name="chunksBatchSize"/>, to optimize memory usage.  Exceptions thrown during the operation are logged if a
        /// <paramref name="logger"/> is provided.</remarks>
        /// <typeparam name="T">The type of the data objects to be written. Must be a reference type.</typeparam>
        /// <param name="data">The collection of data objects to write to the Excel file. Each object represents a row in the worksheet.</param>
        /// <param name="filePath">The full path of the file to create. The file will be overwritten if it already exists.</param>
        /// <param name="options">Optional configuration for Excel writing, such as sheet name, style options, and shared string usage. If not
        /// provided, default options will be used.</param>
        /// <param name="chunksBatchSize">The number of rows to process in each batch. Must be greater than zero. Larger values may improve
        /// performance for large datasets.</param>
        /// <param name="logger">Optional logger for capturing diagnostic or error information during the operation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If canceled, the operation will terminate prematurely.</param>
        /// <returns>A task representing the asynchronous operation. The task completes when the Excel file has been successfully
        /// written.</returns>
        /// <exception cref="ExcelWriterException">Thrown if an error occurs while writing the Excel file.</exception>
        public static async Task WriteFileAsync<T>(IEnumerable<T> data, string filePath, ExcelWriterOptions? options = null, int chunksBatchSize = 50, ILogger? logger = null, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                options ??= new ExcelWriterOptions();

                using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
                using ZipArchive archive = new(fs, ZipArchiveMode.Create);

                if (options.StyleOptions is null && options.StyleSelector == null)
                    options.StyleOptions = ExcelStyleOptions.Empty;

                SharedStringBuilder sharedStringBuilder = new();
                ExcelStyleIndexBuilder styleIndexBuilder = new();
                ExcelWriterHelper.AddContentTypesXml(archive, logger);
                ExcelWriterHelper.AddRels(archive, logger);
                ExcelWriterHelper.AddWorkbookRels(archive, logger);
                ExcelWriterHelper.AddWorkbookXml(archive, options.SheetName, logger);

                await ExcelWriterHelper.AddWorksheetXmlAsync(archive, data, chunksBatchSize, options, sharedStringBuilder, styleIndexBuilder, logger, cancellationToken).ConfigureAwait(false);

                // styles.xml
                ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, options, logger);

                // se usiamo shared strings
                if (options.UseSharedStrings && sharedStringBuilder.UniqueCount > 0)
                    ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
            }
            catch (Exception ex)
            {
                string msg = $"Errors while writing the Excel asynchronously.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Message}", msg);
                throw new ExcelWriterException(msg, ex);
            }
        }

        /// <summary>
        /// Generates an Excel file in memory from the provided data and returns its content as a byte array.
        /// </summary>
        /// <remarks>This method creates an Excel file in OpenXML format and supports customization
        /// through the <paramref name="options"/> parameter. The method uses in-memory streams and does not write to
        /// disk. Ensure sufficient memory is available for large datasets.</remarks>
        /// <typeparam name="T">The type of the objects in the <paramref name="data"/> collection. Must be a reference type.</typeparam>
        /// <param name="data">The collection of data objects to be written to the Excel file. Each object represents a row in the
        /// worksheet.</param>
        /// <param name="options">Optional configuration for customizing the Excel file, such as sheet name, styling, and shared string usage.
        /// If not provided, default options will be used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during the writing process.</param>
        /// <returns>A byte array containing the content of the generated Excel file.</returns>
        /// <exception cref="ExcelWriterException">Thrown if an error occurs while generating the Excel file.</exception>
        public static byte[] WriteBytes<T>(IEnumerable<T> data, ExcelWriterOptions? options = null, ILogger? logger = null)
            where T : class
        {
            try
            {
                options ??= new ExcelWriterOptions();
                using (MemoryStream ms = new())
                {
                    using (ZipArchive archive = new(ms, ZipArchiveMode.Create))
                    {
                        if (options.StyleOptions is null && options.StyleSelector == null)
                            options.StyleOptions = ExcelStyleOptions.Empty;

                        SharedStringBuilder sharedStringBuilder = new();
                        ExcelStyleIndexBuilder styleIndexBuilder = new();
                        // cartelle e file minimi obbligatori
                        ExcelWriterHelper.AddContentTypesXml(archive, logger);
                        ExcelWriterHelper.AddRels(archive, logger);
                        ExcelWriterHelper.AddWorkbookRels(archive, logger);
                        ExcelWriterHelper.AddWorkbookXml(archive, options.SheetName, logger);
                        // sheet
                        ExcelWriterHelper.AddWorksheetXml(archive, data, options, sharedStringBuilder, styleIndexBuilder, logger);
                        // styles.xml
                        ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, options, logger);
                        // se usiamo shared strings
                        if (options.UseSharedStrings && sharedStringBuilder.UniqueCount > 0)
                            ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
                    }

                    // after `ZipArchive` is closed, we can safely return the memory stream as a byte array
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                string msg = $"Errors while writing the Excel bytes.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Message}", msg);
                throw new ExcelWriterException(msg, ex);
            }
        }

        /// <summary>
        /// Asynchronously generates an Excel file in memory from the provided data and returns its contents as a byte
        /// array.
        /// </summary>
        /// <remarks>This method creates an Excel file in the OpenXML format and writes it to an in-memory
        /// stream. The file includes a single worksheet populated with the provided data. The method supports
        /// customization through <paramref name="options"/>, including the use of shared strings and custom
        /// styles.</remarks>
        /// <typeparam name="T">The type of the data objects to be written to the Excel file. Must be a reference type.</typeparam>
        /// <param name="data">The collection of data objects to be written to the Excel file. Each object represents a row in the
        /// worksheet.</param>
        /// <param name="options">Optional configuration for customizing the Excel file generation, such as sheet name, style options, and
        /// shared string usage. If not provided, default options will be used.</param>
        /// <param name="chunksBatchSize">The maximum number of rows to process in a single batch. Defaults to 50.</param>
        /// <param name="logger">An optional logger instance for capturing diagnostic or error information during the operation.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If cancellation is requested, the operation will terminate
        /// early.</param>
        /// <returns>A byte array containing the contents of the generated Excel file.</returns>
        /// <exception cref="ExcelWriterException">Thrown if an error occurs during the Excel file generation process.</exception>
        public static async Task<byte[]> WriteBytesAsync<T>(IEnumerable<T> data, ExcelWriterOptions? options = null, int chunksBatchSize = 50, ILogger? logger = null, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                options ??= new ExcelWriterOptions();
                using MemoryStream ms = new();
                using (ZipArchive archive = new(ms, ZipArchiveMode.Create))
                {
                    if (options.StyleOptions is null && options.StyleSelector == null)
                        options.StyleOptions = ExcelStyleOptions.Empty;

                    SharedStringBuilder sharedStringBuilder = new();
                    ExcelStyleIndexBuilder styleIndexBuilder = new();

                    ExcelWriterHelper.AddContentTypesXml(archive, logger);
                    ExcelWriterHelper.AddRels(archive, logger);
                    ExcelWriterHelper.AddWorkbookRels(archive, logger);
                    ExcelWriterHelper.AddWorkbookXml(archive, options.SheetName, logger);

                    await ExcelWriterHelper.AddWorksheetXmlAsync(archive, data, chunksBatchSize, options, sharedStringBuilder, styleIndexBuilder, logger, cancellationToken).ConfigureAwait(false);

                    // styles.xml
                    ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, options, logger);
                    // se usiamo shared strings
                    if (options.UseSharedStrings && sharedStringBuilder.UniqueCount > 0)
                        ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
                }
                // after `ZipArchive` is closed, we can safely return the memory stream as a byte array
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                string msg = $"Errors while writing the Excel bytes asynchronously.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Message}", msg);
                throw new ExcelWriterException(msg, ex);
            }
        }
    }
}
