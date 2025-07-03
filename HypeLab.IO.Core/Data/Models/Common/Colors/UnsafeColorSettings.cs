namespace HypeLab.IO.Core.Data.Models.Common.Colors
{
    /// <summary>
    /// Represents a set of color settings, including fill and border colors, that may be null or unsafe for certain
    /// operations.
    /// </summary>
    /// <remarks>This struct is designed to provide a lightweight representation of color settings where null
    /// values are allowed. It supports equality comparison, sorting, and conversion from other color settings types.
    /// Use this type when working with scenarios where color values may be optional or unvalidated.</remarks>
    public readonly struct UnsafeColorSettings : IEquatable<UnsafeColorSettings>, IComparable<UnsafeColorSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsafeColorSettings"/> class with the specified fill and font
        /// colors.
        /// </summary>
        /// <param name="fillColor">The fill color to be used. Can be null to indicate no fill color.</param>
        /// <param name="fontColor">The font color to be used. Can be null to indicate no font color.</param>
        public UnsafeColorSettings(string? fillColor, string? fontColor)
        {
            FillColor = fillColor;
            BorderColor = fontColor;
        }

        /// <summary>
        /// Gets the fill color used for rendering the object.
        /// </summary>
        public string? FillColor { get; }
        /// <summary>
        /// Gets the color of the border as a string representation.
        /// </summary>
        public string? BorderColor { get; }

        /// <summary>
        /// Converts the specified fill and font colors into an <see cref="UnsafeColorSettings"/> instance.
        /// </summary>
        /// <param name="fillColor">The fill color to use. Can be null to indicate no fill color.</param>
        /// <param name="fontColor">The font color to use. Can be null to indicate no font color.</param>
        /// <returns>An <see cref="UnsafeColorSettings"/> instance initialized with the specified colors.</returns>
        public static UnsafeColorSettings AsUnsafe(string? fillColor, string? fontColor)
        {
            return new UnsafeColorSettings(fillColor, fontColor);
        }

        /// <summary>
        /// Converts a <see cref="ColorSettings"/> instance to an <see cref="UnsafeColorSettings"/> instance.
        /// </summary>
        /// <param name="settings">The <see cref="ColorSettings"/> instance to convert. Cannot be null.</param>
        /// <returns>An <see cref="UnsafeColorSettings"/> instance initialized with the fill color and border color from the
        /// specified <see cref="ColorSettings"/> instance.</returns>
        public static UnsafeColorSettings AsUnsafe(ColorSettings settings)
        {
            return new UnsafeColorSettings(settings.FillColor, settings.BorderColor);
        }

        /// <summary>
        /// Returns a hash code for the current object based on its <see cref="FillColor"/> and <see
        /// cref="BorderColor"/> properties.
        /// </summary>
        /// <remarks>The hash code is computed using a combination of the hash codes of the <see
        /// cref="FillColor"/> and <see cref="BorderColor"/> properties. If either property is <see langword="null"/>, a
        /// default value of 0 is used in its place.</remarks>
        /// <returns>An integer hash code that represents the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + (FillColor?.GetHashCode() ?? 0);
                hash = (hash * 23) + (BorderColor?.GetHashCode() ?? 0);
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is of type <c>UnsafeColorSettings</c> and is equal to the
        /// current instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is UnsafeColorSettings other)
            {
                return Equals(other);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current <see cref="UnsafeColorSettings"/> instance is equal to another instance of
        /// the same type.
        /// </summary>
        /// <param name="other">The <see cref="UnsafeColorSettings"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the <paramref name="other"/> instance; otherwise,
        /// <see langword="false"/>.</returns>
        public bool Equals(UnsafeColorSettings other)
        {
            return FillColor == other.FillColor && BorderColor == other.BorderColor;
        }

        /// <summary>
        /// Compares the current instance with another <see cref="UnsafeColorSettings"/> object and returns an integer
        /// that indicates whether the current instance precedes, follows, or occurs in the same position in the sort
        /// order.
        /// </summary>
        /// <remarks>The comparison is performed first on the <see cref="FillColor"/> property using an
        /// ordinal string comparison.  If the <see cref="FillColor"/> values are equal, the comparison falls back to
        /// the <see cref="BorderColor"/> property,  also using an ordinal string comparison.</remarks>
        /// <param name="other">The <see cref="UnsafeColorSettings"/> object to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Less than zero if the current instance precedes <paramref name="other"/> in the sort
        /// order.</description></item> <item><description>Zero if the current instance occurs in the same position as
        /// <paramref name="other"/> in the sort order.</description></item> <item><description>Greater than zero if the
        /// current instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(UnsafeColorSettings other)
        {
            int fillColorComparison = string.CompareOrdinal(FillColor, other.FillColor);
            if (fillColorComparison != 0)
            {
                return fillColorComparison;
            }
            return string.CompareOrdinal(BorderColor, other.BorderColor);
        }

        /// <summary>
        /// Determines whether two <see cref="UnsafeColorSettings"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="UnsafeColorSettings"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="UnsafeColorSettings"/> instances are not equal;  otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator !=(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="UnsafeColorSettings"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="UnsafeColorSettings.CompareTo"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="UnsafeColorSettings"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is based on the implementation of the <see
        /// cref="IComparable{T}.CompareTo"/> method for the <see cref="UnsafeColorSettings"/> type.</remarks>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="UnsafeColorSettings"/> instance is less than or equal to the right
        /// <see cref="UnsafeColorSettings"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is less than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="UnsafeColorSettings"/> instance is greater than or equal to the right
        /// <see cref="UnsafeColorSettings"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="UnsafeColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(UnsafeColorSettings left, UnsafeColorSettings right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
