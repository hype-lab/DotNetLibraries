namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents the name of a property in a case-insensitive manner.
    /// </summary>
    /// <remarks>This struct provides functionality for comparing, hashing, and converting property names. It
    /// is case-insensitive, meaning comparisons and hash codes are based on ordinal case-insensitive string comparison.
    /// Instances of <see cref="PropertyName"/> can be implicitly converted to and from <see cref="string"/>.</remarks>
    public readonly struct PropertyName : IEquatable<PropertyName>, IComparable<PropertyName>
    {
        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyName"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the property. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
        public PropertyName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Implicitly converts a <see cref="PropertyName"/> instance to its string representation.
        /// </summary>
        /// <param name="propertyName">The <see cref="PropertyName"/> instance to convert.</param>
        public static implicit operator string(PropertyName propertyName)
        {
            return propertyName.Name;
        }

        /// <summary>
        /// Implicitly converts a string to a <see cref="PropertyName"/> instance.
        /// </summary>
        /// <param name="name">The string to convert to a <see cref="PropertyName"/>.</param>
        public static implicit operator PropertyName(string name)
        {
            return new PropertyName(name);
        }

        /// <summary>
        /// Determines whether the current <see cref="PropertyName"/> instance is equal to the specified <see
        /// cref="PropertyName"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="PropertyName"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the <see cref="Name"/> property of both instances are equal, ignoring case;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(PropertyName other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="PropertyName"/> and is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is PropertyName other && Equals(other);
        }

        /// <summary>
        /// Converts the name of the property to uppercase using the invariant culture.
        /// </summary>
        /// <returns>A new <see cref="PropertyName"/> instance with the property name converted to uppercase.</returns>
        public PropertyName ToUpperInvariant()
        {
            return new PropertyName(Name.ToUpperInvariant());
        }

        /// <summary>
        /// Converts the current property name to lowercase using the invariant culture.
        /// </summary>
        /// <returns>A new <see cref="PropertyName"/> instance with the name converted to lowercase using the invariant culture.</returns>
        public PropertyName ToLowerInvariant()
        {
            return new PropertyName(Name.ToLowerInvariant());
        }

        /// <summary>
        /// Returns a hash code for the current instance based on the value of the <see cref="Name"/> property.
        /// </summary>
        /// <remarks>The hash code is computed using a case-insensitive comparison of the <see
        /// cref="Name"/> property. This ensures consistent hash codes for strings that differ only in casing.</remarks>
        /// <returns>An integer representing the hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
        }

        /// <summary>
        /// Compares the current <see cref="PropertyName"/> instance to another <see cref="PropertyName"/> instance and
        /// returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>Comparison is performed using a case-insensitive, ordinal string comparison of the
        /// <see cref="Name"/> property.</remarks>
        /// <param name="other">The <see cref="PropertyName"/> instance to compare to the current instance.  Cannot be <see
        /// langword="null"/>.</param>
        /// <returns>A signed integer that indicates the relative order of the instances: <list type="bullet">
        /// <item><description>Returns a positive value if the current instance is greater than <paramref
        /// name="other"/>.</description></item> <item><description>Returns zero if the current instance is equal to
        /// <paramref name="other"/>.</description></item> <item><description>Returns a negative value if the current
        /// instance is less than <paramref name="other"/>.</description></item> </list></returns>
        public int CompareTo(PropertyName other)
        {
            if (other == default) return 1; // Null is considered less than any instance
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <remarks>This implementation returns the value of the <see cref="Name"/> property.</remarks>
        /// <returns>A string that represents the current object, typically the value of the <see cref="Name"/> property.</returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether two <see cref="PropertyName"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="PropertyName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="PropertyName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="PropertyName"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(PropertyName left, PropertyName right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="PropertyName"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="PropertyName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="PropertyName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="PropertyName"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(PropertyName left, PropertyName right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="PropertyName"/> instance is less than another <see
        /// cref="PropertyName"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="PropertyName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="PropertyName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(PropertyName left, PropertyName right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="PropertyName"/> instance on the left is greater than the
        /// instance on the right.
        /// </summary>
        /// <param name="left">The <see cref="PropertyName"/> instance on the left side of the comparison.</param>
        /// <param name="right">The <see cref="PropertyName"/> instance on the right side of the comparison.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than the <paramref name="right"/>
        /// instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(PropertyName left, PropertyName right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="PropertyName"/> is less than or equal to the right <see
        /// cref="PropertyName"/>.
        /// </summary>
        /// <param name="left">The first <see cref="PropertyName"/> to compare.</param>
        /// <param name="right">The second <see cref="PropertyName"/> to compare.</param>
        /// <returns><see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value of
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(PropertyName left, PropertyName right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="PropertyName"/> is greater than or equal to the right <see
        /// cref="PropertyName"/>.
        /// </summary>
        /// <param name="left">The first <see cref="PropertyName"/> to compare.</param>
        /// <param name="right">The second <see cref="PropertyName"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(PropertyName left, PropertyName right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
