namespace HypeLab.IO.Core.Data.Models.Common.Fonts
{
    /// <summary>
    /// Represents a set of font attributes, including font name, color, size, and boldness,  used for rendering text.
    /// </summary>
    /// <remarks>This structure is immutable and provides functionality for comparing and equating font
    /// settings.  It is designed to encapsulate font-related properties in a single, lightweight value type.</remarks>
    public readonly struct FontSettings : IEquatable<FontSettings>, IComparable<FontSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontSettings"/> class with the specified font name, color,
        /// size, and boldness.
        /// </summary>
        /// <param name="fontName">The name of the font. This value cannot be <see langword="null"/>.</param>
        /// <param name="fontColor">The color of the font, represented as a string. This value cannot be <see langword="null"/>.</param>
        /// <param name="fontSize">The size of the font, specified as a double. Must be a positive value.</param>
        /// <param name="bold">A value indicating whether the font is bold. <see langword="true"/> if the font is bold; otherwise, <see
        /// langword="false"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fontName"/> is <see langword="null"/> or <paramref name="fontColor"/> is <see
        /// langword="null"/>.</exception>
        public FontSettings(string fontName, string fontColor, double fontSize, bool bold)
        {
            FontName = fontName ?? throw new ArgumentNullException(nameof(fontName), "Font name cannot be null.");
            FontColor = fontColor ?? throw new ArgumentNullException(nameof(fontColor), "Font color cannot be null.");
            FontSize = fontSize;
            Bold = bold;
        }

        /// <summary>
        /// Gets the name of the font used for rendering text.
        /// </summary>
        public string FontName { get; }
        /// <summary>
        /// Gets the font color used for rendering text.
        /// </summary>
        public string FontColor { get; }
        /// <summary>
        /// Gets the font size used for rendering text.
        /// </summary>
        public double FontSize { get; }
        /// <summary>
        /// Gets a value indicating whether the text is displayed in bold.
        /// </summary>
        public bool Bold { get; }

        /// <summary>
        /// Returns a string representation of the font's attributes.
        /// </summary>
        /// <returns>A string containing the font name, color, size, and bold status, formatted as: "<c>FontName, FontColor, FontSize, Bold: BoldStatus</c>".</returns>
        public override string ToString()
        {
            return $"{FontName}, {FontColor}, {FontSize}, Bold: {Bold}";
        }

        /// <summary>
        /// Generates a hash code for the current object based on its font properties.
        /// </summary>
        /// <remarks>The hash code is computed using the values of the <see cref="FontName"/>, <see
        /// cref="FontColor"/>,  <see cref="FontSize"/>, and <see cref="Bold"/> properties. This ensures that objects
        /// with the same  font properties produce the same hash code.</remarks>
        /// <returns>An integer hash code that represents the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + FontName.GetHashCode();
                hash = (hash * 23) + FontColor.GetHashCode();
                hash = (hash * 23) + FontSize.GetHashCode();
                hash = (hash * 23) + Bold.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="FontSettings"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="FontSettings"/> instance and is equal to the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is FontSettings other)
            {
                return Equals(other);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current <see cref="FontSettings"/> instance is equal to another <see
        /// cref="FontSettings"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="FontSettings"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="FontSettings"/> instance is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(FontSettings other)
        {
            return FontName == other.FontName &&
                   FontColor == other.FontColor &&
                   FontSize.Equals(other.FontSize) &&
                   Bold == other.Bold;
        }

        /// <summary>
        /// Compares the current <see cref="FontSettings"/> instance with another <see cref="FontSettings"/> instance
        /// and returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed in the following order: <list type="number">
        /// <item><description>Font name, compared case-insensitively.</description></item> <item><description>Font
        /// color, compared case-insensitively.</description></item> <item><description>Font size, compared
        /// numerically.</description></item> <item><description>Bold property, where <see langword="false"/> precedes
        /// <see langword="true"/>.</description></item> </list></remarks>
        /// <param name="other">The <see cref="FontSettings"/> instance to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Less than zero if the current instance precedes <paramref name="other"/> in the sort
        /// order.</description></item> <item><description>Zero if the current instance occurs in the same position as
        /// <paramref name="other"/> in the sort order.</description></item> <item><description>Greater than zero if the
        /// current instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(FontSettings other)
        {
            int nameComparison = string.CompareOrdinal(FontName, other.FontName);
            if (nameComparison != 0) return nameComparison;
            int colorComparison = string.CompareOrdinal(FontColor, other.FontColor);
            if (colorComparison != 0) return colorComparison;
            int sizeComparison = FontSize.CompareTo(other.FontSize);
            if (sizeComparison != 0) return sizeComparison;
            return Bold.CompareTo(other.Bold);
        }

        /// <summary>
        /// Determines whether two <see cref="FontSettings"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="FontSettings"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(FontSettings left, FontSettings right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="FontSettings"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="FontSettings"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(FontSettings left, FontSettings right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="FontSettings"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="FontSettings.CompareTo(FontSettings)"/> method to
        /// perform the comparison.</remarks>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(FontSettings left, FontSettings right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="FontSettings"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is based on the implementation of the <see cref="IComparable{T}"/>
        /// interface for the <see cref="FontSettings"/> type. Ensure that both instances are properly initialized
        /// before using this operator.</remarks>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(FontSettings left, FontSettings right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="FontSettings"/> instance is less than or equal to the right <see
        /// cref="FontSettings"/> instance.
        /// </summary>
        /// <remarks>This operator uses the <see cref="FontSettings.CompareTo(FontSettings)"/> method to
        /// perform the comparison.</remarks>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is less than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(FontSettings left, FontSettings right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="FontSettings"/> instance is greater than or equal to the right <see
        /// cref="FontSettings"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="FontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="FontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(FontSettings left, FontSettings right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
