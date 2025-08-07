using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers.Const;
using HypeLab.IO.Core.Helpers.Excel;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides functionality for reading and extracting data from Excel files in OpenXML format.
    /// </summary>
    /// <remarks>The <see cref="ExcelReader"/> class is designed to facilitate the extraction of data from
    /// Excel files,  including support for shared strings, header rows, and optional validation. It is particularly
    /// useful  for scenarios where structured data needs to be retrieved from specific sheets within an Excel
    /// file.</remarks>
    public static class ExcelReader
    {
        /// <summary>
        /// Extracts data from a specified sheet in an Excel file and returns it as a <see cref="ExcelSheetData"/> object.
        /// </summary>
        /// <remarks>This method processes the Excel file as a ZIP archive and extracts data from the
        /// specified sheet.  If no sheet name is provided in the <paramref name="options"/>, the default sheet is used.
        /// Validation can be enabled via the <paramref name="options"/> to ensure the extracted data meets specific
        /// criteria.</remarks>
        /// <param name="path">The file path to the Excel file to be processed. Must not be null or empty.</param>
        /// <param name="options">Optional configuration for reading the Excel file, such as the sheet name to extract, validation settings,
        /// and error handling preferences. If not provided, default options will be used.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging errors and diagnostic information during the
        /// extraction process. If null, no logging will occur.</param>
        /// <returns>A <see cref="ExcelSheetData"/> object containing the extracted data from the specified sheet.  If the sheet name
        /// is not found and fallback behavior is enabled, data from the default sheet will be returned.</returns>
        /// <exception cref="ExcelReaderException">Thrown if the specified sheet name is not found and <see
        /// cref="ExcelReaderOptions.ThrowExceptionIfSheetNameNotFound"/>  is set to <see langword="true"/>, or if an
        /// error occurs during the extraction process.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the specified Excel file or the resolved sheet file cannot be found.</exception>
        public static ExcelSheetData ExtractSheetData(string path, ExcelReaderOptions? options = null, ILogger? logger = null)
        {
            try
            {
                options ??= new ExcelReaderOptions();

                ExcelSheetData result = new();

                using (ZipArchive archive = ZipFile.OpenRead(path))
                {
                    List<string> sharedStrings = ExcelReaderHelper.LoadSharedStrings(archive);
                    string? sheetPath = ExcelReaderHelper.ResolveSheetFilePath(archive, options.SheetName, logger);
                    if (sheetPath is null)
                    {
                        if (options.ThrowExceptionIfSheetNameNotFound)
                            throw new ExcelReaderException($"Sheet '{options.SheetName}' not found in workbook.");
                        else
                            sheetPath = ExcelDefaults.Worksheets.Sheet1PathName; // fallback
                    }

                    ZipArchiveEntry sheetEntry = archive.GetEntry(sheetPath)
                        ?? throw new FileNotFoundException($"Sheet file '{sheetPath}' not found in the Excel file.", path);

                    ExcelReaderHelper.WriteSheetData(sheetEntry, result, options, sharedStrings, logger);
                }

                if (options.EnableValidation)
                    ExcelValidator.ValidateSheetData(result, options, logger);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error extracting sheet data from Excel file: {Path}", path);
                throw new ExcelReaderException($"Failed to extract sheet data from '{path}'. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Extracts data from an Excel sheet provided as a byte array and returns it as a <see cref="ExcelSheetData"/>
        /// object.
        /// </summary>
        /// <remarks>This method processes the Excel file in memory and extracts data from the specified
        /// sheet. If no sheet name is provided in the <paramref name="options"/>, the default sheet ("Sheet1") will be
        /// used as a fallback. Validation can be enabled via the <paramref name="options"/> to ensure the extracted
        /// data meets specific criteria.</remarks>
        /// <param name="fileBytes">The byte array representing the Excel file. This parameter cannot be <see langword="null"/> or empty.</param>
        /// <param name="options">Optional configuration for reading the Excel file, such as specifying the sheet name or enabling validation.
        /// If <see langword="null"/>, default options will be used.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging errors or diagnostic information during the
        /// extraction process. If <see langword="null"/>, no logging will occur.</param>
        /// <returns>A <see cref="ExcelSheetData"/> object containing the extracted data from the specified Excel sheet.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="fileBytes"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ExcelReaderException">Thrown if the specified sheet name is not found and <see
        /// cref="ExcelReaderOptions.ThrowExceptionIfSheetNameNotFound"/> is set to <see langword="true"/>, or if an
        /// error occurs during the extraction process.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the resolved sheet file path is not found in the Excel file.</exception>
        public static ExcelSheetData ExtractSheetData(byte[] fileBytes, ExcelReaderOptions? options = null, ILogger? logger = null)
        {
            try
            {
                if (fileBytes is null || fileBytes.Length == 0)
                    throw new ArgumentException("The provided file bytes cannot be null or empty.", nameof(fileBytes));

                options ??= new ExcelReaderOptions();
                ExcelSheetData result = new();

                using (MemoryStream memoryStream = new(fileBytes))
                using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Read, leaveOpen: false))
                {
                    List<string> sharedStrings = ExcelReaderHelper.LoadSharedStrings(archive);
                    string? sheetPath = ExcelReaderHelper.ResolveSheetFilePath(archive, options.SheetName, logger);

                    if (sheetPath is null)
                    {
                        if (options.ThrowExceptionIfSheetNameNotFound)
                            throw new ExcelReaderException($"Sheet '{options.SheetName}' not found in workbook.");
                        else
                            sheetPath = ExcelDefaults.Worksheets.Sheet1PathName; // fallback
                    }

                    ZipArchiveEntry sheetEntry = archive.GetEntry(sheetPath)
                        ?? throw new FileNotFoundException($"Sheet file '{sheetPath}' not found in the Excel file.");

                    ExcelReaderHelper.WriteSheetData(sheetEntry, result, options, sharedStrings, logger);
                }

                if (options.EnableValidation)
                    ExcelValidator.ValidateSheetData(result, options, logger);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error extracting sheet data from Excel file bytes.");
                throw new ExcelReaderException("Failed to extract sheet data from provided file bytes. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Extracts data from an Excel sheet provided as a stream and returns it as an <see cref="ExcelSheetData"/>
        /// object.
        /// </summary>
        /// <remarks>This method reads the Excel file as a ZIP archive and processes the specified sheet
        /// based on the provided options. If no sheet name is specified in <paramref name="options"/>, a default sheet
        /// will be used. Validation can be enabled through the <paramref name="options"/> parameter to ensure the
        /// extracted data meets specific criteria.</remarks>
        /// <param name="stream">A readable <see cref="Stream"/> containing the Excel file data. The stream must not be null or unreadable.</param>
        /// <param name="options">Optional configuration settings for reading the Excel file, such as the target sheet name and validation
        /// options. If not provided, default options will be used.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging errors or diagnostic information during the
        /// extraction process. If null, no logging will occur.</param>
        /// <returns>An <see cref="ExcelSheetData"/> object containing the extracted data from the specified Excel sheet.</returns>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="stream"/> is null or unreadable.</exception>
        /// <exception cref="ExcelReaderException">Thrown if the specified sheet name is not found and <see
        /// cref="ExcelReaderOptions.ThrowExceptionIfSheetNameNotFound"/> is set to <see langword="true"/>, or if an
        /// error occurs during the extraction process.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the resolved sheet file path is not found in the Excel file.</exception>
        public static ExcelSheetData ExtractSheetData(Stream stream, ExcelReaderOptions? options = null, ILogger? logger = null)
        {
            try
            {
                if (stream?.CanRead != true)
                    throw new ArgumentException("The provided stream cannot be null or unreadable.", nameof(stream));

                options ??= new ExcelReaderOptions();
                ExcelSheetData result = new();

                using (ZipArchive archive = new(stream, ZipArchiveMode.Read, leaveOpen: false))
                {
                    List<string> sharedStrings = ExcelReaderHelper.LoadSharedStrings(archive);
                    string? sheetPath = ExcelReaderHelper.ResolveSheetFilePath(archive, options.SheetName, logger);

                    if (sheetPath is null)
                    {
                        if (options.ThrowExceptionIfSheetNameNotFound)
                            throw new ExcelReaderException($"Sheet '{options.SheetName}' not found in workbook.");
                        else
                            sheetPath = ExcelDefaults.Worksheets.Sheet1PathName; // fallback
                    }

                    ZipArchiveEntry sheetEntry = archive.GetEntry(sheetPath)
                        ?? throw new FileNotFoundException($"Sheet file '{sheetPath}' not found in the Excel file.");

                    ExcelReaderHelper.WriteSheetData(sheetEntry, result, options, sharedStrings, logger);
                }

                if (options.EnableValidation)
                    ExcelValidator.ValidateSheetData(result, options, logger);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error extracting sheet data from Excel file bytes.");
                throw new ExcelReaderException("Failed to extract sheet data from provided file bytes. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Downloads an Excel file from the specified URI and extracts its sheet data.
        /// </summary>
        /// <remarks>This method downloads an Excel file from the specified URI and processes it to
        /// extract sheet data. Ensure that the provided URI is valid and points to an accessible Excel file. If logging
        /// is enabled via the <paramref name="logger"/> parameter, any errors encountered during the download or
        /// extraction process will be logged.</remarks>
        /// <param name="onlineSheetFile">The absolute URI of the online Excel file to download. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="options">Optional settings for reading the Excel file, such as specifying the desired sheet or parsing options. Can
        /// be <see langword="null"/> to use default settings.</param>
        /// <param name="logger">An optional logger instance for capturing diagnostic or error information during the operation. Can be <see
        /// langword="null"/> if logging is not required.</param>
        /// <returns>The extracted sheet data as a <see cref="ExcelSheetData"/> object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="onlineSheetFile"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="onlineSheetFile"/> is not an absolute URI.</exception>
        /// <exception cref="ExcelFileDownloadException">Thrown if the file could not be downloaded, the downloaded file is empty, or an error occurs during the
        /// download process.</exception>
        /// <exception cref="ExcelReaderException">Thrown if an error occurs while extracting sheet data from the downloaded file.</exception>
        public static async Task<ExcelSheetData> DownloadAndExtractSheetDataAsync(Uri onlineSheetFile, ExcelReaderOptions? options = null, ILogger? logger = null)
        {
            if (onlineSheetFile is null)
                throw new ArgumentNullException(nameof(onlineSheetFile), "The online sheet file URI cannot be null.");
            if (!onlineSheetFile.IsAbsoluteUri)
                throw new ArgumentException("The provided URI must be absolute.", nameof(onlineSheetFile));

            // download the file to an array of bytes
            byte[] fileBytes;
            try
            {
                using HttpClient httpClient = new();
                fileBytes = await httpClient.GetByteArrayAsync(onlineSheetFile).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error downloading online sheet file: {Uri}", onlineSheetFile);
                throw new ExcelFileDownloadException($"Failed to download online sheet file from '{onlineSheetFile}'. See inner exception for details.", ex);
            }

            if (fileBytes.Length == 0)
                throw new ExcelFileDownloadException($"The downloaded file from '{onlineSheetFile}' is empty.");

            // extract the sheet data from the downloaded file bytes
            try
            {
                return ExtractSheetData(fileBytes, options, logger);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error extracting sheet data from downloaded file: {Uri}", onlineSheetFile);
                throw new ExcelReaderException($"Failed to extract sheet data from downloaded file '{onlineSheetFile}'. See inner exception for details.", ex);
            }
        }
    }
}
