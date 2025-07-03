using HypeLab.IO.Core.Data.Models.Common;
using HypeLab.IO.Core.Data.Models.Excel;
using HypeLab.IO.Core.Helpers.Const;
using HypeLab.IO.Core.Helpers.Excel;
using System.Globalization;

namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    /// <summary>
    /// Represents configuration options for customizing the behavior and formatting of an Excel worksheet.
    /// </summary>
    /// <remarks>This class provides various properties to control the generation and formatting of Excel
    /// worksheets, including options for boolean value representation, shared string usage, cell styles, worksheet
    /// naming, and culture-specific formatting. It also implements comparison and equality operations to facilitate
    /// sorting and comparison of instances.</remarks>
    public sealed class ExcelWorksheetOptions : IWriterOptions, IEquatable<ExcelWorksheetOptions>, IComparable<ExcelWorksheetOptions>
    {
        /// <summary>
        /// Gets or sets the words used to represent boolean values.
        /// </summary>
        public TrueFalseWords? TrueFalseWords { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TrueFalseWords"/> object contains valid non-empty values for
        /// both the <see cref="TrueFalseWords.TrueWord"/> and <see cref="TrueFalseWords.FalseWord"/> properties.
        /// </summary>
        public bool HasTrueFalseWords => TrueFalseWords != null && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.TrueWord) && !string.IsNullOrWhiteSpace(TrueFalseWords.Value.FalseWord);

        /// <summary>
        /// Gets or sets the collection of multiple-choice words where each word is associated with a true or false
        /// value.
        /// </summary>
        public MultipleTrueFalseWords? MultipleTrueFalseWords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shared strings should be used when generating the document. (default: <see langword="true"/>)
        /// </summary>
        public bool UseSharedStrings { get; set; } = true;

        /// <summary>
        /// Gets or sets the style selector used to determine the styles applied to Excel cells.
        /// </summary>
        public IExcelStyleSelector? StyleSelector { get; set; }

        /// <summary>
        /// Gets or sets the style options for formatting Excel cells.
        /// </summary>
        public ExcelStyleOptions StyleOptions { get; set; } = ExcelStyleOptions.Empty;

        /// <summary>
        /// Gets or sets the name of the worksheet.
        /// </summary>
        public SheetName SheetName { get; set; } = ExcelDefaults.Worksheets.Sheet1;

        /// <summary>
        /// Gets or sets the culture information used for formatting and parsing operations.
        /// </summary>
        /// <remarks>This property determines the culture-specific formatting and parsing behavior, such
        /// as date, time, and number formats.</remarks>
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is computed based on the values of the object's properties, including
        /// <see cref="TrueFalseWords"/>, <see cref="UseSharedStrings"/>, <see cref="StyleSelector"/>,  <see
        /// cref="StyleOptions"/>, <see cref="SheetName"/>, and <see cref="Culture"/>.  This ensures that objects with
        /// the same property values produce the same hash code.</remarks>
        /// <returns>An integer hash code that represents the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (TrueFalseWords?.GetHashCode() ?? 0);
                hash = (hash * 23) + UseSharedStrings.GetHashCode();
                hash = (hash * 23) + (StyleSelector?.GetHashCode() ?? 0);
                hash = (hash * 23) + StyleOptions.GetHashCode();
                hash = (hash * 23) + SheetName.GetHashCode();
                hash = (hash * 23) + Culture.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is an <c>ExcelWorksheetOptions</c> instance  and is equal to
        /// the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ExcelWorksheetOptions other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current instance is equal to another <see cref="ExcelWorksheetOptions"/> instance.
        /// </summary>
        /// <remarks>Two <see cref="ExcelWorksheetOptions"/> instances are considered equal if all their
        /// properties, including <see cref="TrueFalseWords"/>, <see cref="UseSharedStrings"/>, <see
        /// cref="StyleSelector"/>, <see cref="StyleOptions"/>, <see cref="SheetName"/>, and <see cref="Culture"/>, are
        /// equal.</remarks>
        /// <param name="other">The <see cref="ExcelWorksheetOptions"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="other"/> instance is equal to the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(ExcelWorksheetOptions other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(TrueFalseWords, other.TrueFalseWords) &&
                   UseSharedStrings == other.UseSharedStrings &&
                   Equals(StyleSelector, other.StyleSelector) &&
                   StyleOptions.Equals(other.StyleOptions) &&
                   SheetName.Equals(other.SheetName) &&
                   Culture.Equals(other.Culture);
        }

        /// <summary>
        /// Compares the current instance with another <see cref="ExcelWorksheetOptions"/> object and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position in the sort
        /// order.
        /// </summary>
        /// <remarks>The comparison is performed in the following order of precedence: <list
        /// type="number"> <item><description><see cref="SheetName"/> is compared using its default comparison
        /// logic.</description></item> <item><description><see cref="CultureInfo.Name"/> is compared using an ordinal
        /// string comparison.</description></item> <item><description><see cref="UseSharedStrings"/> is compared using
        /// its default comparison logic.</description></item> <item><description><see cref="StyleOptions"/> is compared
        /// using its default comparison logic.</description></item> <item><description><see cref="TrueFalseWords"/> is
        /// compared using its default comparison logic, or treated as equal if both are <see
        /// langword="null"/>.</description></item> </list></remarks>
        /// <param name="other">The <see cref="ExcelWorksheetOptions"/> instance to compare with the current instance.  This parameter can
        /// be <see langword="null"/>.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>A positive value if the current instance is greater than <paramref name="other"/> or if
        /// <paramref name="other"/> is <see langword="null"/>.</description></item> <item><description>A negative value
        /// if the current instance is less than <paramref name="other"/>.</description></item> <item><description>Zero
        /// if the current instance is equal to <paramref name="other"/>.</description></item> </list></returns>
        public int CompareTo(ExcelWorksheetOptions other)
        {
            if (other is null) return 1; // null is considered less than any instance
            int result = SheetName.CompareTo(other.SheetName);
            if (result != 0) return result;
            result = string.CompareOrdinal(Culture.Name, other.Culture.Name);
            if (result != 0) return result;
            result = UseSharedStrings.CompareTo(other.UseSharedStrings);
            if (result != 0) return result;
            result = StyleOptions.CompareTo(other.StyleOptions);
            if (result != 0) return result;

            if (TrueFalseWords.HasValue && other.TrueFalseWords.HasValue)
                return TrueFalseWords.Value.CompareTo(other.TrueFalseWords.Value);
            else if (TrueFalseWords.HasValue)
                return 1; // Current instance has a value, other does not
            else if (other.TrueFalseWords.HasValue)
                return -1; // Other instance has a value, current does not

            return 0; // Both are null
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelWorksheetOptions"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelWorksheetOptions"/> instances are equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelWorksheetOptions"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelWorksheetOptions"/> instances are not equal;  otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator !=(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelWorksheetOptions"/> instance is less than another.
        /// </summary>
        /// <remarks>If <paramref name="left"/> is <see langword="null"/>, the result is <see
        /// langword="true"/> unless <paramref name="right"/> is also <see langword="null"/>.</remarks>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            if (left is null) return right is not null; // null is less than any instance
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelWorksheetOptions"/> instance is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>. If <paramref name="left"/> is <see langword="null"/>, the result is always <see
        /// langword="false"/>.</returns>
        public static bool operator >(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            if (left is null) return false; // null is not greater than any instance
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelWorksheetOptions"/> operand is less than or equal to the right operand.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> operand is less than or equal to the <paramref name="right"/>
        /// operand;  otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            return left == right || left < right;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelWorksheetOptions"/> operand is greater than or equal to the
        /// right operand.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheetOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> operand is greater than or equal to the <paramref
        /// name="right"/> operand;  otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(ExcelWorksheetOptions left, ExcelWorksheetOptions right)
        {
            return left == right || left > right;
        }
    }
}
