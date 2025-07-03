using HypeLab.IO.Core.Data.Models.Common;

namespace HypeLab.IO.Core.Data.Options
{
    /// <summary>
    /// Defines configuration options for parsing operations, including date and time formats and custom representations
    /// for boolean values.
    /// </summary>
    /// <remarks>This interface provides properties to configure parsing behavior, such as specifying
    /// acceptable date and time format strings and defining custom words to represent boolean values. Implementations
    /// of this interface can be used to customize parsing logic in scenarios where default parsing behavior is
    /// insufficient or needs to be tailored to specific requirements.</remarks>
    public interface IParserOptions
    {
        /// <summary>
        /// Gets or sets the array of date and time format strings.
        /// </summary>
        string[] DateTimeFormats { get; set; }

        /// <summary>
        /// Gets or sets the character used as the decimal separator in numeric values.
        /// </summary>
        /// <remarks>The value of this property determines how numeric values are formatted and parsed.
        /// Ensure that the specified character is appropriate for the culture or context in which it is used.</remarks>
        NumberDecimalSeparator DecimalSepartor { get; set; }

        /// <summary>
        /// Gets or sets the words used to represent boolean values.
        /// </summary>
        TrueFalseWords? TrueFalseWords { get; set; }

        /// <summary>
        /// Gets or sets the collection of words that can represent multiple true or false values. If both <see cref="TrueFalseWords"/> and <see cref="MultipleTrueFalseWords"/> are set, the latter takes precedence.
        /// </summary>
        MultipleTrueFalseWords? MultipleTrueFalseWords { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TrueFalseWords"/> object contains valid non-empty values for
        /// both the <see cref="TrueFalseWords.TrueWord"/> and <see cref="TrueFalseWords.FalseWord"/> properties.
        /// </summary>
        bool HasTrueFalseWords { get; }
    }
}
