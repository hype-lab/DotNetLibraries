using HypeLab.IO.Core.Data.Attributes.Excel;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Factories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides helper methods for parsing and mapping Excel data to objects and properties.
    /// </summary>
    /// <remarks>The <see cref="ExcelParserHelper"/> class includes methods for parsing Excel-style cell
    /// references, mapping column indices to properties, and converting sheet data into strongly-typed objects. These
    /// methods are designed to facilitate working with Excel data in a structured and type-safe manner.</remarks>
    public static class ExcelParserHelper
    {
        /// <summary>
        /// Parses the column index from a cell reference string.
        /// </summary>
        /// <param name="cellRef">The cell reference string, typically consisting of one or more alphabetic characters representing the column
        /// (e.g., "A", "AB"). Can be null or empty.</param>
        /// <returns>The zero-based column index derived from the cell reference. Returns -1 if <paramref name="cellRef"/> is
        /// null or empty.</returns>
        [SuppressMessage("Roslynator", "RCS1113:Use 'string.IsNullOrEmpty' method", Justification = "Trying to save any KB, any cpu clock")]
        public static int ParseColumnIndex(string? cellRef)
        {
            if (cellRef == null || cellRef.Length == 0)
                return -1;

            int columnIndex = 0;

            for (int i = 0; i < cellRef!.Length; i++)
            {
                char ch = cellRef[i];

                if (ch >= 'A' && ch <= 'Z')
                    columnIndex = (columnIndex * 26) + (ch - 'A' + 1); // +1 perché l'indice è un base 1 (A=1, B=2, ..., Z=26)
                else if (ch >= 'a' && ch <= 'z')
                    columnIndex = (columnIndex * 26) + (ch - 'a' + 1); // 26 perché ci sono 26 lettere nell'alfabeto (A-Z, anche se siamo in italia in tutto il mondo sono comunemente usate le lettere inglesi)
                else
                    break;
            }

            return columnIndex - 1; // Convert to zero-based index
        }

        /// <summary>
        /// Populates the provided index map with mappings between column indices and properties based on attributes or
        /// header names.
        /// </summary>
        /// <remarks>This method maps properties to column indices in the provided <paramref
        /// name="indexMap"/> based on the following rules: <list type="bullet"> <item> If a property has an <see
        /// cref="ExcelColumnIndexAttribute"/>, its specified index is used. The method validates that the index is
        /// non-negative,  not already in use, and within the range of available headers in <paramref name="data"/>.
        /// </item> <item> If no <see cref="ExcelColumnIndexAttribute"/> is present, the method attempts to map the
        /// property by name using the  <see cref="ExcelColumnAttribute"/> or the property name itself. </item> </list>
        /// If any validation fails, an exception is thrown, and the mapping process is halted.  The method logs errors,
        /// warnings, and debug information if a logger is provided.</remarks>
        /// <param name="data">The sheet data containing headers and their corresponding indices.</param>
        /// <param name="props">The list of properties to map to column indices.</param>
        /// <param name="indexMap">The dictionary to populate with column index-to-property mappings.</param>
        /// <param name="logger">An optional logger for logging errors, warnings, and debug information.</param>
        /// <exception cref="NegativeColumnIndexAttributeException">Thrown if a property has an <see cref="ExcelColumnIndexAttribute"/> with a negative index.</exception>
        /// <exception cref="ColumnIndexAlreadyInUseException">Thrown if a property has an <see cref="ExcelColumnIndexAttribute"/> with an index that is already mapped to
        /// another property.</exception>
        /// <exception cref="ColumnIndexAttributeOutOfRangeException">Thrown if a property has an <see cref="ExcelColumnIndexAttribute"/> with an index that exceeds the range of
        /// available headers in <paramref name="data"/>.</exception>
        public static void FillIndexMap(ExcelSheetData data, List<PropertyInfo> props, Dictionary<int, PropertyInfo> indexMap, ILogger? logger = null)
        {
            foreach (PropertyInfo prop in props)
            {
                ExcelColumnIndexAttribute indexAttr = prop.GetCustomAttribute<ExcelColumnIndexAttribute>();
                if (indexAttr != null)
                {
                    // check if the index is valid
                    if (indexAttr.Index < 0)
                    {
                        string msg = $"Invalid column index {indexAttr.Index} for property '{prop.Name}'. Index must be non-negative.";
                        logger?.LogError("{Msg}", msg);
                        Debug.WriteLine(msg);
                        throw new NegativeColumnIndexAttributeException(msg);
                    }

                    // check if the index is already in use
                    if (indexMap.ContainsKey(indexAttr.Index))
                    {
                        string msg = $"Column index {indexAttr.Index} is already mapped to property '{indexMap[indexAttr.Index].Name}'. Skipping property '{prop.Name}'.";
                        logger?.LogWarning("{Msg}", msg);
                        Debug.WriteLine(msg);
                        throw new ColumnIndexAlreadyInUseException(msg);
                    }

                    // check if the index is out of range
                    if (indexAttr.Index >= data.Headers.Length)
                    {
                        string msg = $"Column index {indexAttr.Index} for property '{prop.Name}' is out of range. Maximum index is {data.Headers.Length - 1}.";
                        logger?.LogError("{Msg}", msg);
                        Debug.WriteLine(msg);
                        throw new ColumnIndexAttributeOutOfRangeException(msg);
                    }

                    indexMap[indexAttr.Index] = prop;
                    logger?.LogDebug("Property '{Name}' mapped to column index {Index}.", prop.Name, indexAttr.Index);
                    continue; // salta la mappatura per nome se è presente l'attributo di indice
                }

                ExcelColumnAttribute nameAttr = prop.GetCustomAttribute<ExcelColumnAttribute>();
                string columnName = nameAttr?.OnRead == true && !string.IsNullOrWhiteSpace(nameAttr.Name) ? nameAttr.Name : prop.Name;

                if (data.HeaderIndexMap.TryGetValue(columnName, out int index))
                    indexMap[index] = prop;
            }

            // if any of these is not present in the indexMap, throw an exception
            StringBuilder columnsNotFoundSb = new();
            foreach (string pName in props.Where(p => p.GetCustomAttribute<ExcelColumnAttribute>() is ExcelColumnAttribute att && att.ThrowExceptionIfNotFound && !indexMap.Values.Contains(p)).Select(s => s.Name))
            {
                string msg = $"Property '{pName}' is marked as required but no column index mapping found.";
                logger?.LogError("{Msg}", msg);
                Debug.WriteLine(msg);
                columnsNotFoundSb.AppendLine(msg);
            }

            if (columnsNotFoundSb.Length > 0)
                throw new ColumnNotFoundException(columnsNotFoundSb.ToString().TrimEnd());
        }

        /// <summary>
        /// Parses data from an Excel sheet into a collection of instances of the specified type.
        /// </summary>
        /// <remarks>This method processes each row of the provided <paramref name="data"/> and attempts
        /// to map cell values to the properties of the target type <typeparamref name="T"/>. If a cell value cannot be
        /// converted to the corresponding property type, or if the cell is empty, an error is logged and added to
        /// <paramref name="errors"/> if it is not null. Instances are created using either the <see
        /// cref="ExcelParserOptions.CustomInstanceFactory"/> if specified, or a default factory method.</remarks>
        /// <typeparam name="T">The type of objects to create from the parsed data. Must be a reference type.</typeparam>
        /// <param name="data">The <see cref="ExcelSheetData"/> containing the rows and headers to parse.</param>
        /// <param name="indexMap">A mapping of column indices to <see cref="PropertyInfo"/> objects, indicating which properties of the target
        /// type correspond to which columns.</param>
        /// <param name="instances">A list to which the parsed instances of type <typeparamref name="T"/> will be added.</param>
        /// <param name="options">Options that control the parsing behavior, such as custom instance creation or value conversion settings.</param>
        /// <param name="errors">An optional list to collect parsing errors encountered during the operation. If provided, errors such as
        /// invalid conversions or empty values will be added to this list.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings and errors encountered during parsing.</param>
        /// <exception cref="NullObjectException">Thrown if an instance of type <typeparamref name="T"/> cannot be created for a row.</exception>
        public static void Parse<T>(ExcelSheetData data, Dictionary<int, PropertyInfo> indexMap, List<T> instances, ExcelParserOptions options, List<ExcelParseError>? errors, ILogger? logger = null)
            where T : class
        {
            int colCount = data.Headers.Length;

            PropertyInfo[] propByIndex = new PropertyInfo[colCount];
            foreach (KeyValuePair<int, PropertyInfo> kvp in indexMap)
            {
                if (kvp.Key < colCount)
                    propByIndex[kvp.Key] = kvp.Value;
            }

            object?[] values = new object?[colCount];
            for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
            {
                string?[] row = data.Rows[rowIndex];
                Array.Clear(values, 0, values.Length);

                for (int i = 0; i < row.Length && i < propByIndex.Length; i++)
                {
                    PropertyInfo prop = propByIndex[i];
                    if (prop == null) continue;

                    string? cellValue = row[i];
                    Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    if (string.IsNullOrWhiteSpace(cellValue))
                    {
                        values[i] = null;
                        logger?.LogWarning("Property '{Name}' set to null for row {RowIndex}: cell is empty.", prop.Name, rowIndex);
                        continue;
                    }

                    try
                    {
                        if (CommonFunctions.TryConvertCellValue(cellValue, propertyType, prop.Name, options, out object? converted))
                        {
                            values[i] = converted;
                        }
                        else
                        {
                            string msg = $"Failed to convert '{cellValue}' to {propertyType.Name} for property '{prop.Name}' at row {rowIndex}.";
                            logger?.LogWarning(msg);
                            errors ??= [];
                            errors.Add(new ExcelParseError(prop.Name, msg, rowIndex));
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = $"Exception parsing '{cellValue}' for '{prop.Name}' at row {rowIndex}: {ex.GetFullMessage()}";
                        logger?.LogError(ex, msg);
                        errors ??= [];
                        errors.Add(new ExcelParseError(prop.Name, msg, rowIndex));
                    }
                }

                T? instance = options.CustomInstanceFactory != null
                    ? (T?)options.CustomInstanceFactory.Invoke(CommonFunctions.ConvertToDict(propByIndex, values), logger)
                    : GenericObjectFactory.Create<T>(propByIndex, values, logger);

                if (instance == null)
                {
                    string msg = $"Null instance at row {rowIndex} for type {typeof(T).FullName}";
                    logger?.LogError(msg);
                    throw new NullObjectException(msg);
                }

                instances.Add(instance);
            }
        }
    }
}
