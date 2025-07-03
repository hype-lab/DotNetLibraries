namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents the style settings for an Excel cell, including font, colors, and formatting options.
    /// </summary>
    /// <remarks>This class encapsulates various style properties for an Excel cell, such as font color,
    /// background color, border color, font name, font size, bold state, and number/date formatting. It provides
    /// functionality for comparing and equating styles, making it suitable for scenarios where consistent cell styling
    /// is required.</remarks>
    public sealed class ExcelCellStyle : IEquatable<ExcelCellStyle>, IComparable<ExcelCellStyle>
    {
        /// <summary>
        /// Gets or sets the font color as a hexadecimal string.
        /// </summary>
        public string? FontColorHex { get; set; } = "#000000";
        /// <summary>
        /// Gets or sets the background color in hexadecimal format.
        /// </summary>
        public string? BackgroundColorHex { get; set; } = "#FFFFFF";
        /// <summary>
        /// Gets or sets the border color as a hexadecimal string.
        /// </summary>
        public string? BorderColorHex { get; set; } = "#E0E0E0";
        /// <summary>
        /// Gets or sets the name of the font to be used.
        /// </summary>
        public string? FontName { get; set; } = "Times New Roman";
        /// <summary>
        /// Gets or sets the font size for the text content.
        /// </summary>
        public double FontSize { get; set; } = 11;
        /// <summary>
        /// Gets or sets a value indicating whether the text is displayed in bold.
        /// </summary>
        public bool Bold { get; set; } = false;
        /// <summary>
        /// Gets or sets the format string used to represent integer numbers.
        /// </summary>
        public string? IntegerNumberFormat { get; set; }
        /// <summary>
        /// Gets or sets the format string used to represent decimal numbers.
        /// </summary>
        public string? DecimalNumberFormat { get; set; }
        /// <summary>
        /// Gets or sets the number format applied to the current cell.
        /// </summary>
        public string? CurrentCellNumberFormat { get; set; }
        /// <summary>
        /// Gets or sets the date format string used for parsing and formatting dates.
        /// </summary>
        public string? DateFormat { get; set; }

        /// <summary>
        /// Generates a hash code for the current object based on its properties.
        /// </summary>
        /// <remarks>The hash code is computed using the values of the object's properties, including font
        /// color, background color, border color, font name, font size, bold state,  and various formatting options.
        /// This method ensures that objects with the same  property values produce the same hash code.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (FontColorHex?.GetHashCode() ?? 0);
                hash = (hash * 23) + (BackgroundColorHex?.GetHashCode() ?? 0);
                hash = (hash * 23) + (BorderColorHex?.GetHashCode() ?? 0);
                hash = (hash * 23) + (FontName?.GetHashCode() ?? 0);
                hash = (hash * 23) + FontSize.GetHashCode();
                hash = (hash * 23) + Bold.GetHashCode();
                hash = (hash * 23) + (IntegerNumberFormat?.GetHashCode() ?? 0);
                hash = (hash * 23) + (DecimalNumberFormat?.GetHashCode() ?? 0);
                hash = (hash * 23) + (CurrentCellNumberFormat?.GetHashCode() ?? 0);
                hash = (hash * 23) + (DateFormat?.GetHashCode() ?? 0);
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="ExcelCellStyle"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is an <see cref="ExcelCellStyle"/> and is equal to the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ExcelCellStyle other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current <see cref="ExcelCellStyle"/> instance is equal to another specified <see
        /// cref="ExcelCellStyle"/> instance.
        /// </summary>
        /// <remarks>Two <see cref="ExcelCellStyle"/> instances are considered equal if all their
        /// properties, including font color, background color, border color, font name, font size, bold state, and
        /// various formatting strings, are identical.</remarks>
        /// <param name="other">The <see cref="ExcelCellStyle"/> instance to compare with the current instance, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified <see cref="ExcelCellStyle"/> instance is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(ExcelCellStyle? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(FontColorHex, other.FontColorHex) &&
                   string.Equals(BackgroundColorHex, other.BackgroundColorHex) &&
                   string.Equals(BorderColorHex, other.BorderColorHex) &&
                   string.Equals(FontName, other.FontName) &&
                   FontSize.Equals(other.FontSize) &&
                   Bold == other.Bold &&
                   string.Equals(IntegerNumberFormat, other.IntegerNumberFormat) &&
                   string.Equals(DecimalNumberFormat, other.DecimalNumberFormat) &&
                   string.Equals(CurrentCellNumberFormat, other.CurrentCellNumberFormat) &&
                   string.Equals(DateFormat, other.DateFormat);
        }

        /// <summary>
        /// Compares the current <see cref="ExcelCellStyle"/> instance with another <see cref="ExcelCellStyle"/>
        /// instance and returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed by evaluating the properties of the <see
        /// cref="ExcelCellStyle"/> instances in a specific order. The comparison is case-sensitive and uses ordinal
        /// string comparison for string properties.</remarks>
        /// <param name="other">The <see cref="ExcelCellStyle"/> instance to compare with the current instance. Can be <see
        /// langword="null"/>.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Returns <c>1</c> if <paramref name="other"/> is <see
        /// langword="null"/>.</description></item> <item><description>Returns <c>0</c> if the current instance and
        /// <paramref name="other"/> are equal.</description></item> <item><description>Returns a negative value if the
        /// current instance is less than <paramref name="other"/>.</description></item> <item><description>Returns a
        /// positive value if the current instance is greater than <paramref name="other"/>.</description></item>
        /// </list></returns>
        public int CompareTo(ExcelCellStyle? other)
        {
            if (other is null) return 1; // null is considered less than any instance
            if (ReferenceEquals(this, other)) return 0; // same instance
            // Compare properties in a specific order, you can customize this logic
            int result = string.CompareOrdinal(FontColorHex, other.FontColorHex);
            if (result != 0) return result;
            result = string.CompareOrdinal(BackgroundColorHex, other.BackgroundColorHex);
            if (result != 0) return result;
            result = string.CompareOrdinal(BorderColorHex, other.BorderColorHex);
            if (result != 0) return result;
            result = string.CompareOrdinal(FontName, other.FontName);
            if (result != 0) return result;
            result = FontSize.CompareTo(other.FontSize);
            if (result != 0) return result;
            result = Bold.CompareTo(other.Bold);
            if (result != 0) return result;
            result = string.CompareOrdinal(IntegerNumberFormat, other.IntegerNumberFormat);
            if (result != 0) return result;
            result = string.CompareOrdinal(DecimalNumberFormat, other.DecimalNumberFormat);
            if (result != 0) return result;
            result = string.CompareOrdinal(CurrentCellNumberFormat, other.CurrentCellNumberFormat);
            if (result != 0) return result;
            return string.CompareOrdinal(DateFormat, other.DateFormat);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelCellStyle"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> instance to compare. Can be <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> instance to compare. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both instances are equal or both are <see langword="null"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelCellStyle"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> instance to compare, or <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> instance to compare, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelCellStyle"/> instances are not equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelCellStyle"/> instance is less than another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> instance to compare, or <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> instance to compare, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>. If <paramref name="left"/> is <see langword="null"/>, it is considered less than any
        /// non-<see langword="null"/> instance.</returns>
        public static bool operator <(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            if (left is null) return right is not null; // null is less than any instance
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelCellStyle"/> instance is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> instance to compare. Can be <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> instance to compare. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>. If <paramref name="right"/> is <see langword="null"/>,  the result is <see
        /// langword="true"/> if <paramref name="left"/> is not <see langword="null"/>.</returns>
        public static bool operator >(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            if (right is null) return left is not null; // any instance is greater than null
            return left?.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelCellStyle"/> is less than or equal to the right <see
        /// cref="ExcelCellStyle"/>.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> to compare. Can be <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> to compare. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the left <see cref="ExcelCellStyle"/> is less than or equal to the right <see
        /// cref="ExcelCellStyle"/>;  otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            return left == right || left < right;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelCellStyle"/> is greater than or equal to the right <see
        /// cref="ExcelCellStyle"/>.
        /// </summary>
        /// <remarks>This operator evaluates to <see langword="true"/> if the two <see
        /// cref="ExcelCellStyle"/> instances are equal  or if <paramref name="left"/> is greater than <paramref
        /// name="right"/>.</remarks>
        /// <param name="left">The first <see cref="ExcelCellStyle"/> to compare. Can be <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="ExcelCellStyle"/> to compare. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(ExcelCellStyle? left, ExcelCellStyle? right)
        {
            return left == right || left > right;
        }
    }
}
