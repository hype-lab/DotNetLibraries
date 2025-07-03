using HypeLab.IO.Core.Data.Models.Excel;

namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a definition of a style, including font, color, and formatting properties,  that can be applied to
    /// text or graphical elements.
    /// </summary>
    /// <remarks>The <see cref="StyleDefinition"/> class encapsulates various style-related properties, such
    /// as font color,  fill color, border color, font name, font size, boldness, and number formatting. It provides
    /// methods for  creating instances from external style definitions (e.g., <see cref="ExcelCellStyle"/>), comparing
    /// style  definitions, and generating string representations of the style. This class is immutable once created, 
    /// ensuring consistent behavior when used in comparisons or collections.</remarks>
    public sealed class StyleDefinition : IEquatable<StyleDefinition>, IComparable<StyleDefinition>
    {
        private const double _defaultFontSize = 11.0;

        /// <summary>
        /// Gets or sets the font color used for text rendering.
        /// </summary>
        public string? FontColor { get; set; }
        /// <summary>
        /// Gets or sets the fill color of the object.
        /// </summary>
        public string? FillColor { get; set; }
        /// <summary>
        /// Gets or sets the color of the border as a string.
        /// </summary>
        public string? BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the name of the font to be used.
        /// </summary>
        public string? FontName { get; set; }
        /// <summary>
        /// Gets or sets the font size for the text content.
        /// </summary>
        public double FontSize { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the text is displayed in bold.
        /// </summary>
        public bool Bold { get; set; }
        /// <summary>
        /// Gets or sets the format string used to display numbers.
        /// </summary>
        public string? NumberFormat { get; set; }

        /// <summary>
        /// Creates a <see cref="StyleDefinition"/> instance from the specified <see cref="ExcelCellStyle"/>.
        /// </summary>
        /// <remarks>This method maps the properties of an <see cref="ExcelCellStyle"/> to a <see
        /// cref="StyleDefinition"/>.  If the <c>FontSize</c> property of <paramref name="style"/> is not set or is less
        /// than or equal to zero,  a default font size is applied.</remarks>
        /// <param name="style">The <see cref="ExcelCellStyle"/> containing the cell style properties to convert.</param>
        /// <returns>A <see cref="StyleDefinition"/> populated with the corresponding style properties from the provided
        /// <paramref name="style"/>.</returns>
        public static StyleDefinition FromCellStyle(ExcelCellStyle style)
        {
            return new StyleDefinition
            {
                FillColor = style.BackgroundColorHex,
                FontColor = style.FontColorHex,
                BorderColor = style.BorderColorHex,
                FontName = style.FontName,
                NumberFormat = style.CurrentCellNumberFormat,
                Bold = style.Bold,
                FontSize = style.FontSize > 0 ? style.FontSize : _defaultFontSize // Default to 11.0 if not set
            };
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="StyleDefinition"/> instance.
        /// </summary>
        /// <returns>A string that includes the values of the <see cref="FontColor"/>, <see cref="FillColor"/>, <see
        /// cref="BorderColor"/>, <see cref="FontName"/>, <see cref="FontSize"/>, <see cref="Bold"/>, and <see
        /// cref="NumberFormat"/> properties.</returns>
        public override string ToString()
        {
            return $"StyleDefinition(FontColor: {FontColor}, FillColor: {FillColor}, BorderColor: {BorderColor}, " +
                   $"FontName: {FontName}, FontSize: {FontSize}, Bold: {Bold}, NumberFormat: {NumberFormat})";
        }

        /// <summary>
        /// Returns a hash code for the current object based on its property values.
        /// </summary>
        /// <remarks>The hash code is computed using the values of the <see cref="FontColor"/>, <see
        /// cref="FillColor"/>,  <see cref="BorderColor"/>, <see cref="FontName"/>, <see cref="NumberFormat"/>, <see
        /// cref="Bold"/>,  and <see cref="FontSize"/> properties. This ensures that objects with the same property
        /// values  produce the same hash code.</remarks>
        /// <returns>An integer hash code that represents the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (FontColor?.GetHashCode() ?? 0);
                hash = (hash * 23) + (FillColor?.GetHashCode() ?? 0);
                hash = (hash * 23) + (BorderColor?.GetHashCode() ?? 0);
                hash = (hash * 23) + (FontName?.GetHashCode() ?? 0);
                hash = (hash * 23) + (NumberFormat?.GetHashCode() ?? 0);
                hash = (hash * 23) + Bold.GetHashCode();
                hash = (hash * 23) + FontSize.GetHashCode(); // Include FontSize in hash code
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the current <see cref="StyleDefinition"/> is equal to the specified <see
        /// cref="StyleDefinition"/>.
        /// </summary>
        /// <param name="other">The <see cref="StyleDefinition"/> to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="StyleDefinition"/> is equal to the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(StyleDefinition? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return FontColor == other.FontColor &&
                   FillColor == other.FillColor &&
                   BorderColor == other.BorderColor &&
                   FontName == other.FontName &&
                   NumberFormat == other.NumberFormat &&
                   FontName == other.FontName &&
                   Bold == other.Bold;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="StyleDefinition"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be another <see cref="StyleDefinition"/> or <see
        /// langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="StyleDefinition"/> and is equal to the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is StyleDefinition otherStyle)
                return Equals(otherStyle);

            return false;
        }

        /// <summary>
        /// Compares the current <see cref="StyleDefinition"/> instance with another <see cref="StyleDefinition"/>  and
        /// returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed by evaluating the properties of the <see
        /// cref="StyleDefinition"/> in a specific order: <see cref="FontColor"/>, <see cref="FillColor"/>, <see
        /// cref="BorderColor"/>, <see cref="FontName"/>,  <see cref="FontSize"/>, <see cref="Bold"/>, and <see
        /// cref="NumberFormat"/>.  Null values are considered less than any instance, and reference equality is treated
        /// as equal.</remarks>
        /// <param name="other">The <see cref="StyleDefinition"/> instance to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Returns a negative value if the current instance precedes <paramref name="other"/> in the
        /// sort order.</description></item> <item><description>Returns zero if the current instance is equal to
        /// <paramref name="other"/>.</description></item> <item><description>Returns a positive value if the current
        /// instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(StyleDefinition other)
        {
            if (other is null) return 1; // null is considered less than any instance
            if (ReferenceEquals(this, other)) return 0; // same instance
            // Compare properties in a specific order, you can customize this logic
            int result = string.CompareOrdinal(FontColor, other.FontColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(FillColor, other.FillColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(BorderColor, other.BorderColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(FontName, other.FontName);
            if (result != 0) return result;
            result = FontSize.CompareTo(other.FontSize);
            if (result != 0) return result;
            result = Bold.CompareTo(other.Bold);
            if (result != 0) return result;
            return string.CompareOrdinal(NumberFormat, other.NumberFormat);
        }

        /// <summary>
        /// Determines whether two <see cref="StyleDefinition"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="StyleDefinition"/> instance to compare, or <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> instance to compare, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both <paramref name="left"/> and <paramref name="right"/> are <see
        /// langword="null"/>  or if they are equal according to <see cref="StyleDefinition.Equals(StyleDefinition)"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(StyleDefinition? left, StyleDefinition? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="StyleDefinition"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="StyleDefinition"/> instance to compare, or <see langword="null"/>.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> instance to compare, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the two <see cref="StyleDefinition"/> instances are not equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(StyleDefinition? left, StyleDefinition? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="StyleDefinition"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="CompareTo"/> method to perform the
        /// comparison.</remarks>
        /// <param name="left">The first <see cref="StyleDefinition"/> instance to compare.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(StyleDefinition left, StyleDefinition right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="StyleDefinition"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is based on the implementation of the <see
        /// cref="StyleDefinition.CompareTo"/> method. Ensure that both <paramref name="left"/> and <paramref
        /// name="right"/> are valid instances before using this operator.</remarks>
        /// <param name="left">The first <see cref="StyleDefinition"/> instance to compare.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(StyleDefinition left, StyleDefinition right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="StyleDefinition"/> is less than or equal to the right <see
        /// cref="StyleDefinition"/>.
        /// </summary>
        /// <param name="left">The first <see cref="StyleDefinition"/> to compare.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> <see cref="StyleDefinition"/> is less than or equal to
        /// the <paramref name="right"/> <see cref="StyleDefinition"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(StyleDefinition left, StyleDefinition right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="StyleDefinition"/> is greater than or equal to the right <see
        /// cref="StyleDefinition"/>.
        /// </summary>
        /// <param name="left">The first <see cref="StyleDefinition"/> to compare.</param>
        /// <param name="right">The second <see cref="StyleDefinition"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(StyleDefinition left, StyleDefinition right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
