using HypeLab.IO.Core.Data.Models.Excel.Worksheet;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Helpers.Excel;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.IO.Compression;
using System.Reflection;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides functionality to write an Excel workbook to a file in the OpenXML format.
    /// </summary>
    /// <remarks>This class is designed to generate Excel workbooks programmatically by writing data from
    /// multiple worksheets to a file. The generated file conforms to the OpenXML standard and can  be opened in Excel
    /// or other compatible spreadsheet applications.</remarks>
    public static class ExcelWorkbookWriter
    {
        /// <summary>
        /// Writes the specified Excel worksheets to a file in the OpenXML format.
        /// </summary>
        /// <remarks>This method creates an Excel file in the OpenXML format, including all specified
        /// worksheets, styles, and shared strings. The method ensures that the file is properly structured and adheres
        /// to the OpenXML specification.</remarks>
        /// <param name="filePath">The full path of the file to create. The file will be overwritten if it already exists.</param>
        /// <param name="sheets">A collection of worksheets to include in the Excel file. Each worksheet must implement <see
        /// cref="IExcelWorksheet"/>.</param>
        /// <param name="logger">An optional logger for capturing diagnostic information during the writing process.  If <see
        /// langword="null"/>, no logging will occur.</param>
        /// <exception cref="InvalidOperationException">Thrown if a required method for processing the worksheet data cannot be found or invoked.</exception>
        public static void WriteFile(string filePath, IEnumerable<IExcelWorksheet> sheets, ILogger? logger = null)
        {
            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            using ZipArchive archive = new(fs, ZipArchiveMode.Create);

            SharedStringBuilder sharedStringBuilder = new();
            ExcelStyleIndexBuilder styleIndexBuilder = new();

            // Content and relationships
            ExcelWriterHelper.AddContentTypesXml(archive, sheets, logger);
            ExcelWriterHelper.AddRels(archive, logger);
            ExcelWriterHelper.AddWorkbookRels(archive, sheets, logger);
            ExcelWriterHelper.AddWorkbookXml(archive, sheets, logger);

            int index = 1;
            foreach (IExcelWorksheet sheet in sheets)
            {
                ExcelWorksheetOptions options = sheet.Options ?? new ExcelWorksheetOptions();
                string sheetFileName = $"sheet{index}"; // Ensure valid XML file name

                IEnumerable data = sheet.GetData(); // GetData() returns IEnumerable, not generic
                Type modelType = sheet.ModelType;

                MethodInfo method = typeof(ExcelWriterHelper)
                    .GetMethod("AddWorksheetsXml", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(modelType)
                    ?? throw new InvalidOperationException($"Method 'AddWorksheetsXml<T>' not found for type '{modelType}'.");

                method.Invoke(null,
                [
                    archive,
                    sheetFileName,
                    data,
                    options,
                    sharedStringBuilder,
                    styleIndexBuilder,
                    logger
                ]);

                index++;
            }

            // ormai inutile
            HashSet<ExcelWorksheetOptions> optionsHashet = [.. sheets
                .Select(s => s.Options)
                .Where(o => o is not null)
                .Distinct()];

            ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, optionsHashet, logger);

            if (sharedStringBuilder.UniqueCount > 0)
                ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
        }

        /// <summary>
        /// Writes an Excel file asynchronously to the specified file path using the provided worksheets.
        /// </summary>
        /// <remarks>This method generates an Excel file (.xlsx) based on the given collection of
        /// worksheets and writes it to the specified file path. Each worksheet's data is processed in batches to
        /// optimize memory usage. The method supports cancellation and optional logging.</remarks>
        /// <param name="filePath">The full path to the file where the Excel document will be written. Cannot be null or empty.</param>
        /// <param name="sheets">A collection of worksheets to include in the Excel file. Each worksheet must implement <see
        /// cref="IExcelWorksheet"/>.</param>
        /// <param name="chunkBatchSize">The maximum number of rows to process in a single batch for each worksheet. Must be greater than zero.
        /// Defaults to 50.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging progress and diagnostic information. If null, no
        /// logging will be performed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. If cancellation is requested, the operation will terminate
        /// early. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when the Excel file has been
        /// successfully written.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a required method for processing worksheet data cannot be found or does not return a valid task.</exception>
        public static async Task WriteFileAsync(string filePath, IEnumerable<IExcelWorksheet> sheets, int chunkBatchSize = 50, ILogger? logger = null, CancellationToken cancellationToken = default)
        {
            SharedStringBuilder sharedStringBuilder = new();
            ExcelStyleIndexBuilder styleIndexBuilder = new();

            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            using ZipArchive archive = new(fs, ZipArchiveMode.Create);
            // Content and relationships
            ExcelWriterHelper.AddContentTypesXml(archive, sheets, logger);
            ExcelWriterHelper.AddRels(archive, logger);
            ExcelWriterHelper.AddWorkbookRels(archive, sheets, logger);
            ExcelWriterHelper.AddWorkbookXml(archive, sheets, logger);

            int index = 1;
            foreach (IExcelWorksheet sheet in sheets)
            {
                ExcelWorksheetOptions options = sheet.Options ?? new ExcelWorksheetOptions();
                string sheetFileName = $"sheet{index}"; // Ensure valid XML file name

                IEnumerable data = sheet.GetData(); // GetData() returns IEnumerable, not generic
                Type modelType = sheet.ModelType;

                MethodInfo method = typeof(ExcelWriterHelper)
                    .GetMethod("AddWorksheetsXmlAsync", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(modelType)
                    ?? throw new InvalidOperationException($"Method 'AddWorksheetsXmlAsync<T>' not found for type '{modelType}'.");

                object? res = method.Invoke(null,
                [
                    archive,
                    sheetFileName,
                    data,
                    chunkBatchSize,
                    options,
                    sharedStringBuilder,
                    styleIndexBuilder,
                    logger,
                    cancellationToken
                ]);

                if (res is Task task)
                {
                    // await the task to ensure completion
                    await task.ConfigureAwait(false);
                }
                else
                {
                    throw new InvalidOperationException($"Method 'AddWorksheetsXmlAsync<T>' did not return a Task for type '{modelType}'.");
                }

                index++;
            }

            HashSet<ExcelWorksheetOptions> optionsHashet = [.. sheets
                .Select(s => s.Options)
                .Where(o => o is not null)
                .Distinct()];

            ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, optionsHashet, logger);

            if (sharedStringBuilder.UniqueCount > 0)
                ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
        }

        /// <summary>
        /// Generates an Excel file in memory as a byte array from the provided worksheets.
        /// </summary>
        /// <remarks>This method creates an Excel file in memory by processing the provided worksheets and
        /// their associated data. It supports shared strings, styles, and relationships between the workbook and its
        /// components. The resulting byte array can be saved to a file or used directly in memory.</remarks>
        /// <param name="sheets">A collection of worksheets to include in the Excel file. Each worksheet must implement <see
        /// cref="IExcelWorksheet"/>.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during the file generation process. Can be
        /// <see langword="null"/>.</param>
        /// <returns>A byte array representing the generated Excel file in the OpenXML format.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a required method for processing worksheet data cannot be found or invoked.</exception>
        public static byte[] WriteBytes(IEnumerable<IExcelWorksheet> sheets, ILogger? logger = null)
        {
            using MemoryStream ms = new();
            using (ZipArchive archive = new(ms, ZipArchiveMode.Create, leaveOpen: true))
            {
                SharedStringBuilder sharedStringBuilder = new();
                ExcelStyleIndexBuilder styleIndexBuilder = new();

                // Content and relationships
                ExcelWriterHelper.AddContentTypesXml(archive, sheets, logger);
                ExcelWriterHelper.AddRels(archive, logger);
                ExcelWriterHelper.AddWorkbookRels(archive, sheets, logger);
                ExcelWriterHelper.AddWorkbookXml(archive, sheets, logger);

                int index = 1;
                foreach (IExcelWorksheet sheet in sheets)
                {
                    ExcelWorksheetOptions options = sheet.Options ?? new ExcelWorksheetOptions();
                    string sheetFileName = $"sheet{index}";

                    IEnumerable data = sheet.GetData();
                    Type modelType = sheet.ModelType;

                    MethodInfo method = typeof(ExcelWriterHelper)
                        .GetMethod("AddWorksheetsXml", BindingFlags.Public | BindingFlags.Static)
                        .MakeGenericMethod(modelType)
                        ?? throw new InvalidOperationException($"Method 'AddWorksheetXml<T>' not found for type '{modelType}'.");

                    method.Invoke(null,
                    [
                        archive,
                            sheetFileName,
                            data,
                            options,
                            sharedStringBuilder,
                            styleIndexBuilder,
                            logger
                    ]);

                    index++;
                }

                HashSet<ExcelWorksheetOptions> optionsHashet = [.. sheets
                        .Select(s => s.Options)
                        .Where(o => o is not null)
                        .Distinct()];

                ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, optionsHashet, logger);
                if (sharedStringBuilder.UniqueCount > 0)
                    ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
            }

            // after `ZipArchive` is closed, we can safely return the memory stream as a byte array
            return ms.ToArray();
        }

        /// <summary>
        /// Generates an Excel file in memory as a byte array by writing the provided worksheets asynchronously.
        /// </summary>
        /// <remarks>This method creates an Excel file in memory using the provided worksheets and returns
        /// it as a byte array. The method processes worksheet data in chunks, which can improve performance for large
        /// datasets.  The caller is responsible for ensuring that the <paramref name="sheets"/> collection is not empty
        /// and that each worksheet provides valid data and options. If no worksheets are provided, the resulting Excel
        /// file will be empty.  The method supports cancellation via the <paramref name="cancellationToken"/>
        /// parameter. If cancellation is requested, the operation will terminate as soon as possible.</remarks>
        /// <param name="sheets">A collection of worksheets to include in the Excel file. Each worksheet must implement <see
        /// cref="IExcelWorksheet"/>.</param>
        /// <param name="chunkBatchSize">The maximum number of rows to process in a single batch. Defaults to 50.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging progress and diagnostic information.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A byte array containing the generated Excel file.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a required method for processing worksheet data cannot be found or does not return a valid task.</exception>
        public async static Task<byte[]> WriteBytesAsync(IEnumerable<IExcelWorksheet> sheets, int chunkBatchSize = 50, ILogger? logger = null, CancellationToken cancellationToken = default)
        {
            SharedStringBuilder sharedStringBuilder = new();
            ExcelStyleIndexBuilder styleIndexBuilder = new();

            using MemoryStream ms = new();
            using (ZipArchive archive = new(ms, ZipArchiveMode.Create, leaveOpen: true))
            {
                // Content and relationships
                ExcelWriterHelper.AddContentTypesXml(archive, sheets, logger);
                ExcelWriterHelper.AddRels(archive, logger);
                ExcelWriterHelper.AddWorkbookRels(archive, sheets, logger);
                ExcelWriterHelper.AddWorkbookXml(archive, sheets, logger);

                int index = 1;
                foreach (IExcelWorksheet sheet in sheets)
                {
                    ExcelWorksheetOptions options = sheet.Options ?? new ExcelWorksheetOptions();
                    string sheetFileName = $"sheet{index}"; // Ensure valid XML file name

                    IEnumerable data = sheet.GetData(); // GetData() returns IEnumerable, not generic
                    Type modelType = sheet.ModelType;

                    MethodInfo method = typeof(ExcelWriterHelper)
                        .GetMethod("AddWorksheetsXmlAsync", BindingFlags.Public | BindingFlags.Static)
                        .MakeGenericMethod(modelType)
                        ?? throw new InvalidOperationException($"Method 'AddWorksheetsXmlAsync<T>' not found for type '{modelType}'.");

                    object? res = method.Invoke(null,
                    [
                        archive,
                        sheetFileName,
                        data,
                        chunkBatchSize,
                        options,
                        sharedStringBuilder,
                        styleIndexBuilder,
                        logger,
                        cancellationToken
                    ]);

                    if (res is Task task)
                    {
                        // await the task to ensure completion
                        await task.ConfigureAwait(false);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Method 'AddWorksheetsXmlAsync<T>' did not return a Task for type '{modelType}'.");
                    }

                    index++;
                }

                HashSet<ExcelWorksheetOptions> optionsHashet = [.. sheets
                        .Select(s => s.Options)
                        .Where(o => o is not null)
                        .Distinct()];

                ExcelWriterHelper.AddStylesXml(archive, styleIndexBuilder, optionsHashet, logger);

                if (sharedStringBuilder.UniqueCount > 0)
                    ExcelWriterHelper.AddSharedStringsXml(archive, sharedStringBuilder, logger);
            }

            // after `ZipArchive` is closed, we can safely return the memory stream as a byte array
            return ms.ToArray();
        }
    }
}
