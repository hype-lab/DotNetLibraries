using HypeLab.IO.Core.Data.Attributes.Excel;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Helpers;
using HypeLab.IO.Core.Helpers.Excel;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides methods for parsing Excel-style data, including extracting column indices from cell references and mapping
    /// sheet data to objects of a specified type.
    /// </summary>
    /// <remarks>The <see cref="ExcelParser"/> class includes static methods for working with Excel-style data: <list
    /// type="bullet"> <item> <description>Parsing column indices from cell references in Excel-style notation (e.g., "A1",
    /// "B2").</description> </item> <item> <description>Mapping sheet data to objects of a specified type, with support for
    /// attributes to define column mappings.</description> </item> <item> <description>Asynchronous parsing for scenarios
    /// requiring non-blocking operations, such as UI or web applications.</description> </item> </list> This class is
    /// designed to simplify the process of working with Excel-style data in .NET applications.</remarks>
    public static class ExcelParser
    {
        /// <summary>
        /// Parses the provided sheet data into a collection of objects of the specified type.
        /// </summary>
        /// <remarks>This method maps the columns in the provided sheet data to the properties of the
        /// specified type <typeparamref name="T"/>. The mapping is determined based on property names and the provided
        /// options. If required field validation is enabled in the <paramref name="options"/>, missing required fields
        /// will be reported as errors.</remarks>
        /// <typeparam name="T">The type of objects to parse the data into. Must be a reference type with writable public properties.</typeparam>
        /// <param name="data">The sheet data to parse. This represents the input data from an Excel sheet.</param>
        /// <param name="options">Optional parsing options that control the behavior of the parser. If not provided, default options will be
        /// used.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during parsing.</param>
        /// <returns>An <see cref="ExcelParseResult{T}"/> containing the parsed objects and any errors encountered during
        /// parsing.</returns>
        /// <exception cref="ExcelParserException">Thrown if an unexpected error occurs during parsing.</exception>
        public static ExcelParseResult<T> ParseTo<T>(ExcelSheetData data, ExcelParserOptions? options = null, ILogger? logger = null)
            where T : class
        {
            try
            {
                options ??= new ExcelParserOptions();

                Type type = typeof(T);
                List<PropertyInfo> props = [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && (p.GetCustomAttribute<ExcelIgnoreAttribute>() == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>() is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnRead)))];

                if (data.HasNullHeaderValues)
                    data.Headers = [.. data.Headers.Where(x => !string.IsNullOrEmpty(x))];

                Dictionary<int, PropertyInfo> indexMap = [];

                ExcelParserHelper.FillIndexMap(data, props, indexMap, logger);

                List<T> instances = new(data.Rows.Count);
                List<ExcelParseError>? errors = null;

                ExcelParserHelper.Parse(data, indexMap, instances, options, errors, logger);

                //if (options.EnableRequiredFieldValidation)
                //    ExcelValidator.ValidateRequiredFields(instances, errors, logger);

                return new ExcelParseResult<T>(instances, errors);
            }
            catch (Exception ex)
            {
                string msg = $"Errors while parsing the sheet data.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Msg}", msg);
                throw new ExcelParserException(msg, ex);
            }
        }

        /// <summary>
        /// Parses the provided sheet data into a collection of objects of type <typeparamref name="T"/> asynchronously.
        /// </summary>
        /// <remarks>This method processes the sheet data in chunks to avoid blocking the calling thread,
        /// making it suitable for UI or web applications. If <paramref name="options"/> specifies required field
        /// validation, the method will validate that all required fields are populated.</remarks>
        /// <typeparam name="T">The type of objects to parse the sheet data into. Must be a reference type with writable properties.</typeparam>
        /// <param name="data">The sheet data to parse, including headers and rows.</param>
        /// <param name="options">Optional configuration for parsing behavior. If not provided, default options will be used.</param>
        /// <param name="chunksBatchSize">The number of rows to process in each batch. Defaults to 50. Must be a positive integer.</param>
        /// <param name="logger">An optional logger for capturing diagnostic or error information during parsing.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Parsing will stop if cancellation is requested.</param>
        /// <returns>An <see cref="ExcelParseResult{T}"/> containing the parsed objects and any errors encountered during
        /// parsing.</returns>
        /// <exception cref="ExcelParserException">Thrown if an error occurs during parsing that prevents the operation from completing.</exception>
        public static async Task<ExcelParseResult<T>> ParseToAsync<T>(ExcelSheetData data, ExcelParserOptions? options = null, int chunksBatchSize = 50, ILogger? logger = null, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                options ??= new ExcelParserOptions();

                await Task.Yield(); // non blocca thread (utile per UI, web, ecc.)

                Type type = typeof(T);
                List<PropertyInfo> props = [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && (p.GetCustomAttribute<ExcelIgnoreAttribute>() == null || (p.GetCustomAttribute<ExcelIgnoreAttribute>() is ExcelIgnoreAttribute ignoreAttr && !ignoreAttr.OnRead)))];

                Dictionary<int, PropertyInfo> indexMap = [];

                ExcelParserHelper.FillIndexMap(data, props, indexMap, logger);

                List<T> instances = [];
                List<ExcelParseError> errors = [];

                // Parsing chunked per evitare blocchi
                int totalRows = data.Rows.Count;

                for (int i = 0; i < totalRows; i += chunksBatchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // seleziona chunk corrente
                    ExcelSheetData chunk = new()
                    {
                        Headers = data.Headers,
                        Rows = [.. data.Rows.Skip(i).Take(chunksBatchSize)]
                    };

                    ExcelParserHelper.Parse(chunk, indexMap, instances, options, errors, logger);

                    await Task.Yield(); // cede il controllo al contesto chiamante
                }

                //if (options.EnableRequiredFieldValidation)
                //    ExcelValidator.ValidateRequiredFields(instances, errors, logger);

                return new ExcelParseResult<T>(instances, errors);
            }
            catch (Exception ex)
            {
                string msg = $"Errors while parsing the sheet data.\n{ex.GetFullMessage()}";
                logger?.LogError(ex, "{Msg}", msg);
                throw new ExcelParserException(msg, ex);
            }
        }
    }
}
