using HypeLab.IO.Core.Data.Models.Common;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    /// <summary>
    /// Represents configuration options for a parser, allowing customization of validation, instance creation, and
    /// date/time formatting.
    /// </summary>
    /// <remarks>This class provides properties to control various aspects of parsing behavior, including
    /// enabling or disabling required field validation, specifying a custom factory for instance creation, and defining
    /// acceptable date/time formats.</remarks>
    public class ExcelParserOptions : IParserOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether required fields validation is enabled. (default: <see langword="false"/>)
        /// </summary>
        [SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "WIP. Still not used")]
        [Obsolete("WIP. Still not used")]
        public bool EnableRequiredFieldValidation { get; set; } = false;

        /// <summary>
        /// Gets or sets a custom factory function for creating instances of a type.
        /// </summary>
        /// <remarks>The dictionary parameter provides property values  for the instance, and the <see cref="ILogger"/> parameter can be used for logging during
        /// creation. Intended for devs that do perfect parries.</remarks>
        public Func<Dictionary<PropertyInfo, object?>, ILogger?, object>? CustomInstanceFactory { get; set; }

        /// <summary>
        /// Gets or sets the array of date and time format strings.
        /// </summary>
        public string[] DateTimeFormats { get; set; } = [];

        /// <summary>
        /// Gets or sets the decimal separator used for formatting numeric values.
        /// </summary>
        /// <remarks>This property determines whether a dot (.) or a comma (,) is used as the decimal
        /// separator when formatting numbers. Ensure that the selected value aligns with the expected numeric format in
        /// the target culture or system.</remarks>
        public NumberDecimalSeparator DecimalSepartor { get; set; } = NumberDecimalSeparator.Dot;

        /// <summary>
        /// Gets or sets the words used to represent boolean values.
        /// </summary>
        public TrueFalseWords? TrueFalseWords { get; set; }

        /// <summary>
        /// Gets or sets the collection of multiple-choice words where each word can be marked as true or false. If both <see cref="TrueFalseWords"/> and <see cref="MultipleTrueFalseWords"/> are set, the latter takes precedence."/>
        /// </summary>
        public MultipleTrueFalseWords? MultipleTrueFalseWords { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TrueFalseWords"/> object contains valid non-empty values for
        /// both the <see cref="TrueFalseWords.TrueWord"/> and <see cref="TrueFalseWords.FalseWord"/> properties.
        /// </summary>
        public bool HasTrueFalseWords => TrueFalseWords != null && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.TrueWord) && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.FalseWord);

        /// <summary>
        /// Whether to throw on conversion errors (default: <see langword="false"/>).
        /// </summary>
        public bool ThrowOnParseError { get; set; } = false;
    }
}
