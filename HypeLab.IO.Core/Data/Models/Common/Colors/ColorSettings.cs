namespace HypeLab.IO.Core.Data.Models.Common.Colors
{
    /// <summary>
    /// Represents a set of color settings, including a fill color and a font color,  with support for equality
    /// comparison and ordering.
    /// </summary>
    /// <remarks>This struct is immutable and provides functionality for comparing and  equating instances
    /// based on their color values. It is particularly useful  for scenarios where consistent color settings need to be
    /// managed or compared.</remarks>
    public readonly struct ColorSettings : IEquatable<ColorSettings>, IComparable<ColorSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSettings"/> class with the specified fill and font colors.
        /// </summary>
        /// <param name="fillColor">The fill color to be used. Cannot be <see langword="null"/>.</param>
        /// <param name="fontColor">The font color to be used. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="fillColor"/> or <paramref name="fontColor"/> is <see langword="null"/>.</exception>
        public ColorSettings(string fillColor, string fontColor)
        {
            FillColor = fillColor ?? throw new ArgumentNullException(nameof(fillColor), "Fill color cannot be null.");
            BorderColor = fontColor ?? throw new ArgumentNullException(nameof(fontColor), "Font color cannot be null.");
        }

        /// <summary>
        /// Gets the fill color used for rendering the object.
        /// </summary>
        public string FillColor { get; }
        /// <summary>
        /// Gets the color of the border as a string representation.
        /// </summary>
        public string BorderColor { get; }

        /// <summary>
        /// Returns a hash code for the current object based on its <see cref="FillColor"/> and <see
        /// cref="BorderColor"/> properties.
        /// </summary>
        /// <returns>An integer hash code that represents the current object. The hash code is derived from the values of the
        /// <see cref="FillColor"/> and <see cref="BorderColor"/> properties.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + FillColor.GetHashCode();
                hash = (hash * 23) + BorderColor.GetHashCode();
                return hash;
            }
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="ColorSettings"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="ColorSettings"/> instance  and is equal to
        /// the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ColorSettings other)
            {
                return Equals(other);
            }
            return false;
        }
        /// <summary>
        /// Determines whether the current <see cref="ColorSettings"/> instance is equal to another instance.
        /// </summary>
        /// <param name="other">The <see cref="ColorSettings"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the <paramref name="other"/> instance; otherwise,
        /// <see langword="false"/>.</returns>
        public bool Equals(ColorSettings other)
        {
            return FillColor == other.FillColor && BorderColor == other.BorderColor;
        }

        /// <summary>
        /// Returns a string representation of the object, including the fill color and border color.
        /// </summary>
        /// <returns>A string that represents the current object, formatted as  "FillColor: {FillColor}, FontColor:
        /// {BorderColor}".</returns>
        public override string ToString()
        {
            return $"FillColor: {FillColor}, FontColor: {BorderColor}";
        }

        /// <summary>
        /// Compares the current <see cref="ColorSettings"/> instance with another <see cref="ColorSettings"/> instance
        /// and returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed first on the <see cref="FillColor"/> property. If the
        /// <see cref="FillColor"/> values are equal, the comparison falls back to the <see cref="BorderColor"/>
        /// property.</remarks>
        /// <param name="other">The <see cref="ColorSettings"/> instance to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the instances: <list type="bullet"> <item><description>A
        /// negative value if the current instance precedes <paramref name="other"/> in the sort
        /// order.</description></item> <item><description>Zero if the current instance is equal to <paramref
        /// name="other"/> in the sort order.</description></item> <item><description>A positive value if the current
        /// instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(ColorSettings other)
        {
            int fillColorComparison = string.CompareOrdinal(FillColor, other.FillColor);
            if (fillColorComparison != 0)
            {
                return fillColorComparison;
            }
            return string.CompareOrdinal(BorderColor, other.BorderColor);
        }

        /// <summary>
        /// Determines whether two <see cref="ColorSettings"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ColorSettings"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(ColorSettings left, ColorSettings right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="ColorSettings"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ColorSettings"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(ColorSettings left, ColorSettings right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="ColorSettings"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="IComparable{T}.CompareTo"/> method to compare the
        /// two instances. Ensure that both <paramref name="left"/> and <paramref name="right"/> are valid and
        /// comparable.</remarks>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(ColorSettings left, ColorSettings right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ColorSettings"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is based on the implementation of the <see
        /// cref="IComparable{T}.CompareTo"/> method for the <see cref="ColorSettings"/> type. Ensure that <see
        /// cref="ColorSettings"/> instances being compared are properly initialized to avoid unexpected
        /// behavior.</remarks>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(ColorSettings left, ColorSettings right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ColorSettings"/> instance is less than or equal to the right <see
        /// cref="ColorSettings"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is less than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(ColorSettings left, ColorSettings right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ColorSettings"/> instance is greater than or equal to the right <see
        /// cref="ColorSettings"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="ColorSettings"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ColorSettings"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(ColorSettings left, ColorSettings right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
