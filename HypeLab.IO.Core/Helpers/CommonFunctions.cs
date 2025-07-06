using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Options;
using HypeLab.IO.Core.Data.Options.Impl.Excel;
using HypeLab.IO.Core.Helpers.Const;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace HypeLab.IO.Core.Helpers
{
    /// <summary>
    /// Provides a collection of utility methods for working with reflection, type conversion, and property metadata.
    /// </summary>
    /// <remarks>This static class includes methods for determining property characteristics, such as whether
    /// a property is  <see langword="init-only"/> or marked as required, as well as methods for safely converting
    /// string values to  various target types. These utilities are designed to simplify common reflection and parsing
    /// tasks in .NET applications.</remarks>
    public static class CommonFunctions
    {
        /// <summary>
        /// Determines whether the specified property is marked as <see langword="init-only"/>.
        /// </summary>
        /// <param name="prop">The <see cref="PropertyInfo"/> representing the property to check.</param>
        /// <returns><see langword="true"/> if the property is marked as <see langword="init-only"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool IsInitOnly(this PropertyInfo prop)
        {
            MethodInfo? setMethod = prop.SetMethod;
            if (setMethod == null) return false;

            return IsInit(setMethod);
        }

        /// <summary>
        /// Determines whether the specified <see cref="MethodInfo"/> represents a property setter  that is marked as
        /// <see langword="init"/>-only.
        /// </summary>
        /// <param name="setMethod">The <see cref="MethodInfo"/> of the property setter to evaluate.</param>
        /// <returns><see langword="true"/> if the specified setter is marked as <see langword="init"/>-only;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool IsInitOnly(this MethodInfo setMethod)
        {
            return IsInit(setMethod);
        }

        /// <summary>
        /// Determines whether the specified property is marked as required.
        /// </summary>
        /// <remarks>This method checks for two conditions to determine if a property is required: <list
        /// type="bullet"> <item><description>The presence of the <see cref="RequiredAttribute"/> on the
        /// property.</description></item> <item><description>The use of the C# <c>required</c> keyword, which is
        /// identified through custom modifiers.</description></item> </list> A property without a setter cannot be
        /// marked as required and will always return <see langword="false"/>.</remarks>
        /// <param name="prop">The <see cref="PropertyInfo"/> representing the property to check.</param>
        /// <returns><see langword="true"/> if the property is marked as required, either by the presence of a  <see
        /// cref="RequiredAttribute"/> or the C# <c>required</c> keyword; otherwise, <see langword="false"/>.</returns>
        public static bool IsRequired(this PropertyInfo prop)
        {
            MethodInfo? setMethod = prop.SetMethod;
            if (setMethod == null) return false; // non ha un setter, quindi non può essere required

            // intercetta [Required]
            bool hasRequiredAttribute = prop.IsDefined(typeof(RequiredAttribute), inherit: true);

            //var tr = setMethod.ReturnParameter.GetRequiredCustomModifiers().ToList();

            // intercetta `required` (C# keyword)
            bool hasRequiredKeyword = setMethod.ReturnParameter.GetRequiredCustomModifiers()
                .Any(mod => mod.FullName == CoreDefaults.RequiredMemberAttributeName);

            return hasRequiredAttribute || hasRequiredKeyword;
        }

        /// <summary>
        /// Creates a dictionary that maps properties to their corresponding values.
        /// </summary>
        /// <param name="props">An array of <see cref="PropertyInfo"/> objects representing the properties to be used as keys in the
        /// dictionary. Cannot contain null elements.</param>
        /// <param name="values">An array of objects representing the values to be associated with the properties. Must have the same length
        /// as <paramref name="props"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> where each key is a <see cref="PropertyInfo"/> from <paramref
        /// name="props"/>  and its corresponding value is the object from <paramref name="values"/> at the same index.</returns>
        public static Dictionary<PropertyInfo, object?> ConvertToDict(PropertyInfo[] props, object?[] values)
        {
            Dictionary<PropertyInfo, object?> dict = new(props.Length);
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i] != null)
                    dict[props[i]] = values[i];
            }
            return dict;
        }

        /// <summary>
        /// Attempts to convert the specified input string to a value of the specified target type.
        /// </summary>
        /// <remarks>This method supports conversion to common types such as <see cref="string"/>, <see
        /// cref="decimal"/>, <see cref="bool"/>, <see cref="Guid"/>, <see cref="DateTime"/>, and enums. It also handles
        /// nullable types and applies custom parsing rules defined in <paramref name="options"/>. If the input string
        /// is null or whitespace, the method returns <see langword="true"/> and sets <paramref name="result"/> to
        /// either <see langword="null"/> or an empty string, depending on the target type.</remarks>
        /// <param name="input">The input string to be converted. Can be null or empty.</param>
        /// <param name="targetType">The type to which the input string should be converted. Cannot be null.</param>
        /// <param name="propertyName">The name of the property associated with the input, used for custom parsing rules.</param>
        /// <param name="options">The parsing options that define custom behaviors, such as decimal separators, true/false word mappings, and
        /// date formats. Cannot be null.</param>
        /// <param name="result">When the method returns, contains the converted value if the conversion was successful; otherwise, null.</param>
        /// <returns><see langword="true"/> if the conversion was successful; otherwise, <see langword="false"/>.</returns>
        public static bool TryConvertCellValue(string? input, Type targetType, string propertyName, IParserOptions options, out object? result)
        {
            result = null;

            // Nullable<T>, magari è già stato fatto dal caller, ma non si sa mai
            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            bool isTargetTypeString = targetType == typeof(string);
            bool isTargetTypeDecimal = targetType == typeof(decimal);

            if (string.IsNullOrWhiteSpace(input))
            {
                result = isTargetTypeString ? string.Empty : null;
                return true; // valore nullo gestito altrove (es. per Nullable)
            }

            if (isTargetTypeString)
            {
                result = input;
                return true;
            }

            try
            {
                if (targetType.IsEnum)
                {
                    object enumValue = Enum.Parse(targetType, input, ignoreCase: true);

                    if (enumValue != null)
                    {
                        result = enumValue;
                        return true;
                    }
                }

                if (targetType == typeof(Guid) && Guid.TryParse(input, out Guid guid))
                {
                    result = guid;
                    return true;
                }

                //if (isTargetTypeDecimal && !string.IsNullOrWhiteSpace(input))
                //{
                //    input = CleanDecimalInput(input, options);

                //    if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
                //    {
                //        result = decimalValue;
                //        return true;
                //    }
                //}

                if (isTargetTypeDecimal && !string.IsNullOrWhiteSpace(input))
                {
                    input = input!.Trim();
                    // if targetType is decimal, clean any non-numeric characters related to currency
                    foreach (string currency in Currencies.All)
                    {
                        input = input.Replace(currency, string.Empty);
                    }

                    // convert to decimal using the specified decimal separator
                    if (!string.IsNullOrWhiteSpace(options.DecimalSepartor) && options.DecimalSepartor.IsComma)
                    {
                        input = input.Replace(NumberDecimalSeparator.Dot, string.Empty);
                        input = input.Replace(options.DecimalSepartor, CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                    }
                    else if (!string.IsNullOrWhiteSpace(options.DecimalSepartor) && options.DecimalSepartor.IsDot)
                    {
                        input = input.Replace(NumberDecimalSeparator.Comma, string.Empty);
                    }

                    if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
                    {
                        result = decimalValue;
                        return true;
                    }
                }

                if (targetType == typeof(bool))
                {
                    string inputTrimmed = input?.Trim() ?? string.Empty;
                    if (options.MultipleTrueFalseWords?.Count > 0 && options.MultipleTrueFalseWords.TryGetWords(propertyName, out TrueFalseWords words))
                    {
                        // gestione con parole multiple
                        if (inputTrimmed.Equals(words.TrueWord, StringComparison.OrdinalIgnoreCase))
                        {
                            result = true;
                            return true;
                        }

                        if (inputTrimmed.Equals(words.FalseWord, StringComparison.OrdinalIgnoreCase))
                        {
                            result = false;
                            return true;
                        }

                        // se non superate le parole multiple esplicitamente settate, per me è un errore
                        Debug.WriteLine($"Input '{inputTrimmed}' does not match any custom true/false words: {words.TrueWord}, {words.FalseWord}");
                        result = null;
                        return false;
                    }

                    if (options.TrueFalseWords.HasValue && options.HasTrueFalseWords)
                    {
                        // gestione con parole personalizzate
                        if (inputTrimmed.Equals(options.TrueFalseWords.Value.TrueWord, StringComparison.OrdinalIgnoreCase))
                        {
                            result = true;
                            return true;
                        }

                        if (inputTrimmed.Equals(options.TrueFalseWords.Value.FalseWord, StringComparison.OrdinalIgnoreCase))
                        {
                            result = false;
                            return true;
                        }

                        // se non superate le parole personalizzate esplicitamente settate, per me è un errore
                        Debug.WriteLine($"Input '{inputTrimmed}' does not match any custom true/false words: {options.TrueFalseWords.Value.TrueWord}, {options.TrueFalseWords.Value.FalseWord}");
                        result = null;
                        return false;
                    }
                    else if (options.TrueFalseWords.HasValue && !options.HasTrueFalseWords)
                    {
                        Debug.WriteLine("TrueFalseWords is set but does not contain valid true/false words...");
                        result = null;
                        return false; // non posso convertire senza parole valide
                    }

                    if (bool.TryParse(inputTrimmed, out bool b))
                    {
                        result = b;
                        return true;
                    }

                    // gestione extra: "1", "0"
                    if (inputTrimmed == "1") { result = true; return true; }
                    if (inputTrimmed == "0") { result = false; return true; }
                    if (inputTrimmed.Equals("yes", StringComparison.OrdinalIgnoreCase)) { result = true; return true; }
                    if (inputTrimmed.Equals("no", StringComparison.OrdinalIgnoreCase)) { result = false; return true; }
                }

                if (targetType == typeof(DateTime))
                {
                    // check for OLE Automation Date format first
                    if (IsOADateTime(input, out DateTime oaDateTime))
                    {
                        result = oaDateTime;
                        return true;
                    }

                    if (options.DateTimeFormats.Length > 0 && DateTime.TryParseExact(input, options.DateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        result = date;
                        return true;
                    }
                    else if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                    {
                        result = dateTime;
                        return true;
                    }
                }

                // fallback standard
                result = Convert.ChangeType(input, targetType);
                return true;
            }
            catch (Exception ex)
            {
                // Safe: ignora errori di conversione
                Debug.WriteLine($"Error converting '{input}' to {targetType.Name}: {ex.GetFullMessage()}");

                return false;
            }
        }

        //private static string CleanDecimalInput(string? input, IParserOptions options)
        //{
        //    if (input == null || input.Length == 0)
        //        return string.Empty;

        //    input = input.Trim();

        //    bool isComma = options.DecimalSepartor.IsComma;
        //    bool isDot = options.DecimalSepartor.IsDot;

        //    if (!isComma && !isDot)
        //        return input; // fallback se mal configurato

        //    // 1. Trova posizione dell'ultimo separatore
        //    int lastComma = input.LastIndexOf(',');
        //    int lastDot = input.LastIndexOf('.');

        //    int decimalSeparatorIndex = Math.Max(lastComma, lastDot);

        //    var sb = new StringBuilder(input.Length);

        //    for (int i = 0; i < input.Length; i++)
        //    {
        //        char c = input[i];

        //        if (char.IsDigit(c))
        //        {
        //            sb.Append(c);
        //        }
        //        else if ((c == ',' || c == '.') && i == decimalSeparatorIndex) //  otherwise i'ts a thousand separator or invalid character, skip   
        //        {
        //            sb.Append('.');
        //        }
        //    }

        //    return sb.ToString();
        //}

        private static bool IsOADateTime(string? input, out DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                dateTime = default;
                return false; // input non valido
            }

            // OLE Automation Date è un formato di data utilizzato in COM e OLE Automation
            // è rappresentato come un numero decimale che indica il numero di giorni trascorsi dal 30 dicembre 1899
            // e le frazioni di giorno rappresentano l'ora del giorno

            if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                return true;

            // poi provo a convertire in OLE Automation Date
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double oaDate))
            {
                dateTime = DateTime.FromOADate(oaDate);
                return true;
            }

            dateTime = default;
            return false;
        }

        private static bool IsInit(MethodInfo setMethod)
        {
            Type[] modifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();
            return modifiers.Any(m => m.FullName == CoreDefaults.IsExternalInitNamespace);
        }
    }
}
