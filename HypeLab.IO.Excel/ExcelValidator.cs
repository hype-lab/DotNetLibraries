using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Helpers;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HypeLab.IO.Excel
{
    /// <summary>
    /// Provides methods for validating the integrity and structure of Excel sheet data.
    /// </summary>
    /// <remarks>The <see cref="ExcelValidator"/> class includes functionality to validate sheet data against
    /// specified rules,  such as ensuring the presence of headers, detecting duplicate headers, and verifying row
    /// conformity to header structures. It also supports validation of required properties in parsed objects.</remarks>
    public static class ExcelValidator
    {
        /// <summary>
        /// Validates the integrity and structure of the provided sheet data based on the specified options.
        /// </summary>
        /// <remarks>If <paramref name="options"/> specifies that the sheet has a header row, the method
        /// validates the presence of a header, checks for duplicate headers, and ensures that all rows conform to the
        /// header structure.</remarks>
        /// <param name="sheet">The sheet data to validate. Cannot be <see langword="null"/>.</param>
        /// <param name="options">The options that define validation rules, such as whether the sheet has a header row. Cannot be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sheet"/> or <paramref name="options"/> is <see langword="null"/>.</exception>
        public static void ValidateSheetData(ExcelSheetData sheet, ExcelReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));

            if (options.HasHeaderRow)
            {
                ValidateHeaderExists(sheet, options);
                ValidateDuplicateHeaders(sheet);
            }

            ValidateRowsAgainstHeader(sheet, options);
        }

        /// <summary>
        /// Validates that all required properties in the specified list of instances are not null or empty.
        /// </summary>
        /// <remarks>A property is considered required if it is marked with the <see
        /// cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>  or if it is defined as required using C#
        /// 11's `required` keyword. If a required property is null or empty, an error is added to the  <paramref
        /// name="errors"/> list, and a log entry is created if a <paramref name="logger"/> is provided.</remarks>
        /// <typeparam name="T">The type of the instances to validate. Each property of type <typeparamref name="T"/> is checked for
        /// required attributes.</typeparam>
        /// <param name="instances">The list of instances to validate. Each instance is checked for properties marked as required.</param>
        /// <param name="errors">A list to which validation errors will be added. Each error includes the property name, a descriptive
        /// message, and the row index of the invalid instance.</param>
        /// <param name="logger">An optional logger used to log validation errors. If <see langword="null"/>, no logging will occur.</param>
        public static void ValidateRequiredFields<T>(List<T> instances, List<ExcelParseError> errors, ILogger? logger = null)
        {
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (InstanceRowIndex<T> instanceRowIndex in instances.Select((inst, idx) => new InstanceRowIndex<T>(inst, idx)))
            {
                foreach (PropertyInfo prop in props)
                {
                    bool hasAttribute = prop.GetCustomAttribute<RequiredAttribute>() != null;
                    bool isCSharpRequired = prop.IsRequired();

                    if (!hasAttribute && !isCSharpRequired)
                        continue;

                    object? value = prop.GetValue(instanceRowIndex.Instance);
                    bool isNullOrEmpty = ValidatorHelper.IsNullOrEmpty(value);

                    if (isNullOrEmpty)
                    {
                        string msg = $"Property '{prop.Name}' is required but has null or empty value at row {instanceRowIndex.RowIndex}.";
                        errors.Add(new ExcelParseError(prop.Name, msg, instanceRowIndex.RowIndex));
                        logger?.LogError("{Msg}", msg);
                    }
                }
            }
        }

        private static void ValidateHeaderExists(ExcelSheetData sheet, ExcelReaderOptions options)
        {
            if (sheet.Headers.Count == 0)
                throw new InvalidOperationException($"Expected a header row at index {options.HeaderRowIndex}, but none was found.");
        }

        private static void ValidateDuplicateHeaders(ExcelSheetData sheet)
        {
            List<string> duplicates = [.. sheet.Headers
                .GroupBy(h => h, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)];

            if (duplicates.Any())
                throw new InvalidOperationException($"Duplicate headers detected: {string.Join(", ", duplicates)}");
        }

        private static void ValidateRowsAgainstHeader(ExcelSheetData sheet, ExcelReaderOptions options)
        {
            if (!options.HasHeaderRow || sheet.Headers.Count == 0)
                return;

            for (int i = 0; i < sheet.Rows.Count; i++)
            {
                List<string> row = sheet.Rows[i];
                int headerCount = sheet.Headers.Count;

                if (row.Count > headerCount)
                    sheet.RowWarnings.Add(new RowWarning(i, $"Row has more columns ({row.Count}) than the header ({headerCount}). Extra columns will be ignored."));
                else if (row.Count < headerCount)
                    sheet.RowWarnings.Add(new RowWarning(i, $"Row has fewer columns ({row.Count}) than the header ({headerCount}). Missing columns will be empty."));
            }
        }

        //public static void ValidateFields<T>(List<T> instances, List<ExcelParseError> errors, ILogger? logger = null)
        //{
        //    PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach ((T instance, int rowIndex) in instances.Select((inst, idx) => (inst, idx)))
        //    {
        //        foreach (PropertyInfo prop in props)
        //        {
        //            bool isRequired = prop.CustomAttributes.Any(a => a.AttributeType.Name == "RequiredAttribute");
        //            if (!isRequired) continue;

        //            object value = prop.GetValue(instance);
        //            if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
        //            {
        //                string msg = $"Missing required value for property '{prop.Name}' on row {rowIndex}";
        //                errors.Add(new ExcelParseError(prop.Name, msg, rowIndex));
        //                logger?.LogWarning(msg);
        //            }
        //        }
        //    }
        //}
    }
}
