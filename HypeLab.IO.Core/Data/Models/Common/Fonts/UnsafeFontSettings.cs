namespace HypeLab.IO.Core.Data.Models.Common.Fonts
{
    /// <summary>
    /// Represents a set of font settings that can be used for rendering text, including font name, color, size, and
    /// bold styling.
    /// </summary>
    /// <remarks>This struct is designed to provide a lightweight, immutable representation of font
    /// properties.  It supports equality comparison, sorting, and conversion from other font-related types.</remarks>
    public readonly struct UnsafeFontSettings : IEquatable<UnsafeFontSettings>, IComparable<UnsafeFontSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeFontSettings"/> class with the specified font properties.
        /// </summary>
        /// <remarks>This constructor allows you to define font settings that may not be validated for
        /// safety or correctness.  Ensure that the provided values are appropriate for the intended use case.</remarks>
        /// <param name="fontName">The name of the font. Can be <see langword="null"/> to indicate no specific font name.</param>
        /// <param name="fontColor">The color of the font as a string. Can be <see langword="null"/> to indicate no specific color.</param>
        /// <param name="fontSize">The size of the font in points. Must be a positive value.</param>
        /// <param name="bold"><see langword="true"/> to apply bold styling to the font; otherwise, <see langword="false"/>.</param>
        public UnsafeFontSettings(string? fontName, string? fontColor, double fontSize, bool bold)
        {
            FontName = fontName;
            FontColor = fontColor;
            FontSize = fontSize;
            Bold = bold;
        }

        /// <summary>
        /// Gets the name of the font used for rendering text.
        /// </summary>
        public string? FontName { get; }
        /// <summary>
        /// Gets the font color as a string representation, or <see langword="null"/> if no color is set.
        /// </summary>
        public string? FontColor { get; }
        /// <summary>
        /// Gets the font size used for rendering text.
        /// </summary>
        public double FontSize { get; }
        /// <summary>
        /// Gets a value indicating whether the text is displayed in bold.
        /// </summary>
        public bool Bold { get; }

        /// <summary>
        /// Creates an instance of <see cref="UnsafeFontSettings"/> with the specified font properties.
        /// </summary>
        /// <param name="fontName">The name of the font. Can be <see langword="null"/> to indicate no specific font name.</param>
        /// <param name="fontColor">The color of the font as a string. Can be <see langword="null"/> to indicate no specific color.</param>
        /// <param name="fontSize">The size of the font in points. Must be a positive value.</param>
        /// <param name="bold"><see langword="true"/> to apply bold styling to the font; otherwise, <see langword="false"/>.</param>
        /// <returns>An <see cref="UnsafeFontSettings"/> object initialized with the specified font properties.</returns>
        public static UnsafeFontSettings AsUnsafe(string? fontName, string? fontColor, double fontSize, bool bold)
        {
            return new UnsafeFontSettings(fontName, fontColor, fontSize, bold);
        }

        /// <summary>
        /// Converts a <see cref="FontSettings"/> instance to an <see cref="UnsafeFontSettings"/> instance.
        /// </summary>
        /// <param name="settings">The <see cref="FontSettings"/> instance to convert. Must not be <c>null</c>.</param>
        /// <returns>An <see cref="UnsafeFontSettings"/> instance containing the same font properties as the specified <paramref
        /// name="settings"/>.</returns>
        public static UnsafeFontSettings AsUnsafe(FontSettings settings)
        {
            return new UnsafeFontSettings(settings.FontName, settings.FontColor, settings.FontSize, settings.Bold);
        }

        /// <summary>
        /// Generates a hash code for the current object based on its font properties.
        /// </summary>
        /// <remarks>The hash code is computed using the values of the <c>FontName</c>, <c>FontColor</c>,
        /// <c>FontSize</c>, and <c>Bold</c> properties. This method ensures that objects with the same property values
        /// produce the same hash code, which is useful for hash-based collections such as dictionaries or hash
        /// sets.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + FontName?.GetHashCode() ?? 0;
                hash = (hash * 23) + FontColor?.GetHashCode() ?? 0;
                hash = (hash * 23) + FontSize.GetHashCode();
                hash = (hash * 23) + Bold.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is of type <c>UnsafeFontSettings</c> and is equal to the
        /// current instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is UnsafeFontSettings other)
            {
                return Equals(other);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current <see cref="UnsafeFontSettings"/> instance is equal to another instance of the
        /// same type.
        /// </summary>
        /// <param name="other">The <see cref="UnsafeFontSettings"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the <paramref name="other"/> instance; otherwise,
        /// <see langword="false"/>.</returns>
        public bool Equals(UnsafeFontSettings other)
        {
            return FontName == other.FontName &&
                   FontColor == other.FontColor &&
                   FontSize.Equals(other.FontSize) &&
                   Bold == other.Bold;
        }

        /// <summary>
        /// Compares the current instance with another <see cref="UnsafeFontSettings"/> object and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position in the sort
        /// order.
        /// </summary>
        /// <remarks>The comparison is performed in the following order of precedence: <list
        /// type="number"> <item><description><see cref="FontName"/> is compared using an ordinal string
        /// comparison.</description></item> <item><description><see cref="FontColor"/> is compared using an ordinal
        /// string comparison.</description></item> <item><description><see cref="FontSize"/> is compared using its
        /// natural numeric ordering.</description></item> <item><description><see cref="Bold"/> is compared using its
        /// natural boolean ordering.</description></item> </list></remarks>
        /// <param name="other">The <see cref="UnsafeFontSettings"/> object to compare with the current instance.</param>
        /// <returns>A signed integer that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Less than zero if the current instance precedes <paramref name="other"/> in the sort
        /// order.</description></item> <item><description>Zero if the current instance occurs in the same position as
        /// <paramref name="other"/> in the sort order.</description></item> <item><description>Greater than zero if the
        /// current instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(UnsafeFontSettings other)
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
        /// Determines whether two <see cref="UnsafeFontSettings"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="UnsafeFontSettings"/> instances are equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="UnsafeFontSettings"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="UnsafeFontSettings"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="UnsafeFontSettings"/> instance is less than another.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="UnsafeFontSettings"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is based on the implementation of the <see cref="CompareTo"/> method
        /// for <see cref="UnsafeFontSettings"/>. Ensure that <see cref="UnsafeFontSettings"/> instances being compared
        /// are valid and properly initialized.</remarks>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether one <see cref="UnsafeFontSettings"/> instance is less than or equal to another.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="UnsafeFontSettings"/> instance is greater than or equal to the right
        /// <see cref="UnsafeFontSettings"/> instance.
        /// </summary>
        /// <remarks>This operator relies on the <see
        /// cref="UnsafeFontSettings.CompareTo(UnsafeFontSettings)"/> method to perform the comparison. Ensure that both
        /// instances are properly initialized before using this operator.</remarks>
        /// <param name="left">The first <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeFontSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(UnsafeFontSettings left, UnsafeFontSettings right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
