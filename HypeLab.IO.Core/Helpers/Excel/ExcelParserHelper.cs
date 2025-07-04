using HypeLab.IO.Core.Data.Attributes.Excel;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Exceptions;
using HypeLab.IO.Core.Factories;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

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
        /// Parses the column index from a cell reference string in Excel-style notation.
        /// </summary>
        /// <remarks>This method interprets the column letters in the cell reference as a base-26 number,
        /// where 'A' corresponds to 0, 'B' to 1, and so on. For example, "A1" returns 0, "B2" returns 1, and "AA10"
        /// returns 26.</remarks>
        /// <param name="cellRef">The cell reference string, such as "A1" or "B2". The string must begin with one or more letters representing
        /// the column, followed by optional numeric characters.</param>
        /// <returns>The zero-based column index derived from the column letters in the cell reference. Returns -1 if <paramref
        /// name="cellRef"/> is null, empty, or consists only of whitespace.</returns>
        public static int ParseColumnIndex(string? cellRef)
        {
            if (string.IsNullOrWhiteSpace(cellRef))
                return -1;

            // extract the column letters (e.g., "B" from "B2")
            string columnRef = new([.. cellRef.TakeWhile(char.IsLetter)]);

            int columnIndex = 0;
            foreach (char ch in columnRef)
            {
                columnIndex *= 26; // 26 perché ci sono 26 lettere nell'alfabeto (A-Z, anche se siamo in italia in tutto il mondo sono comunemente usate le lettere inglesi)
                columnIndex += ch - 'A' + 1;
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
            foreach (PropertyInfo prop in props.Where(p => p.GetCustomAttribute<ExcelColumnAttribute>() is ExcelColumnAttribute att && att.ThrowExceptionIfNotFound))
            {
                if (!indexMap.Values.Contains(prop))
                {
                    string msg = $"Property '{prop.Name}' is marked as required but no column index mapping found.";
                    logger?.LogError("{Msg}", msg);
                    Debug.WriteLine(msg);
                    throw new ColumnNotFoundException(msg);
                }
            }
        }

        /// <summary>
        /// Parses rows of data from an <see cref="ExcelSheetData"/> object and maps them to instances of the specified
        /// type.
        /// </summary>
        /// <remarks>This method processes each row in the <paramref name="data"/> object, maps the values
        /// to the properties of the target type using the <paramref name="indexMap"/>, and creates instances of type
        /// <typeparamref name="T"/>. If a cell value cannot be converted to the target property type, the behavior
        /// depends on the <paramref name="options"/>: <list type="bullet"> <item>If <see
        /// cref="ExcelParserOptions.ThrowOnParseError"/> is <see langword="true"/>, an exception is thrown.</item>
        /// <item>If <see cref="ExcelParserOptions.ThrowOnParseError"/> is <see langword="false"/>, the error is logged
        /// and added to the <paramref name="errors"/> list.</item> </list> If a custom instance factory is provided in
        /// <paramref name="options"/>, it will be used to create instances of type <typeparamref name="T"/>. Otherwise,
        /// a default factory is used.</remarks>
        /// <typeparam name="T">The type of objects to create from the parsed data. Must be a reference type.</typeparam>
        /// <param name="data">The <see cref="ExcelSheetData"/> containing the rows to parse.</param>
        /// <param name="indexMap">A dictionary mapping column indices to the corresponding <see cref="PropertyInfo"/> of the target type.</param>
        /// <param name="instances">A list to which the created instances of type <typeparamref name="T"/> will be added.</param>
        /// <param name="options">Options that control the parsing behavior, such as error handling and custom instance creation.</param>
        /// <param name="errors">A list to which any parsing errors encountered will be added. This list will be populated even if exceptions
        /// are not thrown.</param>
        /// <param name="logger">An optional <see cref="ILogger"/> instance for logging warnings and errors during parsing. Can be <see
        /// langword="null"/>.</param>
        /// <exception cref="ParseErrorException">Thrown if a parsing error occurs and <see cref="ExcelParserOptions.ThrowOnParseError"/> is set to <see
        /// langword="true"/>.</exception>
        /// <exception cref="NullObjectException">Thrown if an instance of type <typeparamref name="T"/> cannot be created.</exception>
        public static void Parse<T>(ExcelSheetData data, Dictionary<int, PropertyInfo> indexMap, List<T> instances, ExcelParserOptions options, List<ExcelParseError> errors, ILogger? logger = null)
            where T : class
        {
            foreach (string[] row in data.Rows)
            {
                //T instance = new();
                Dictionary<PropertyInfo, object?> propertyValues = [];
                foreach (KeyValuePair<int, PropertyInfo> kvp in indexMap)
                {
                    int index = kvp.Key;
                    PropertyInfo prop = kvp.Value;

                    if (index >= row.Length)
                    {
                        if (options.ThrowOnParseError)
                            throw new ParseErrorException($"Index {index} is out of bounds for row with {row.Length} columns.");

                        errors.Add(new ExcelParseError(prop.Name, $"Index {index} is out of bounds for row with {row.Length} columns.", data.Rows.IndexOf(row)));
                        logger?.LogWarning("Skipping property '{Name}' for row {IndexOfRow}: index {Index} is out of bounds for row with {Count} columns.", prop.Name, data.Rows.IndexOf(row), index, row.Length);
                        continue; // Skip if index is out of bounds
                    }

                    string cellValue = row[index];

                    // controllo se la proprietà è nullable e converto il valore di cella al tipo corretto
                    Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    string propertyName = prop.Name;
                    //string propertyName = prop.GetCustomAttribute<ExcelColumnAttribute>()?.Name ?? prop.Name;

                    if (string.IsNullOrWhiteSpace(cellValue))
                    {
                        // se è Nullable, setta null esplicitamente
                        //if (propertyType != null)
                        //    prop.SetValue(instance, null);

                        if (propertyType != null)
                            propertyValues[prop] = null;

                        errors.Add(new ExcelParseError(prop.Name, $"Cell value is empty for property '{prop.Name}' at row {data.Rows.IndexOf(row)}.", data.Rows.IndexOf(row)));
                        logger?.LogWarning("Property '{Name}' set to null for row {IndexOfRow}: cell value is empty.", prop.Name, data.Rows.IndexOf(row));

                        continue; // salta il resto
                    }

                    try
                    {
                        if (CommonFunctions.TryConvertCellValue(cellValue, propertyType, propertyName, options, out object? convertedValue))
                        {
                            propertyValues[prop] = convertedValue;
                            //prop.SetValue(instance, convertedValue);
                        }
                        else
                        {
                            string msg = $"Error converting value '{cellValue}' to {prop.PropertyType.Name} for property '{prop.Name}'";
                            if (options.ThrowOnParseError)
                                throw new ParseErrorException(msg);

                            errors.Add(new ExcelParseError(prop.Name, msg, data.Rows.IndexOf(row)));
                            logger?.LogError("{Msg}", msg);
                            Debug.WriteLine(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Safe: ignora errori di conversione
                        // Skip this cell if parsing fails
                        string msg = $"Error parsing cell value '{cellValue}' for property '{prop.Name}': {ex.GetFullMessage()}";
                        if (options.ThrowOnParseError)
                            throw new ParseErrorException(msg, ex);

                        errors.Add(new ExcelParseError(prop.Name, msg, data.Rows.IndexOf(row)));
                        logger?.LogError(ex, "{Msg}", msg);
                        Debug.WriteLine(msg);
                    }
                }

                T? instance = ((T?)options.CustomInstanceFactory?.Invoke(propertyValues, logger) ?? GenericObjectFactory.Create<T>(propertyValues));
                if (instance == null)
                {
                    string msg = $"Failed to create an instance of type {typeof(T).FullName}. The instance is null.";
                    logger?.LogError("{Msg}", msg);
                    Debug.WriteLine(msg);
                    throw new NullObjectException(msg);
                }

                instances.Add(instance);
            }
        }
    }
}
