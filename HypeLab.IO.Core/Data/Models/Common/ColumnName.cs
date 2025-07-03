//namespace HypeLab.IO.Core.Data.Models.Common
//{
//    /// <summary>
//    /// Represents the name of a column in a database or data structure.
//    /// </summary>
//    /// <remarks>The <see cref="ColumnName"/> struct provides a strongly-typed representation of a column
//    /// name,  ensuring case-insensitive equality comparisons and convenient implicit conversions to and from strings.
//    /// Instances of <see cref="ColumnName"/> are immutable.</remarks>
//    public readonly struct ColumnName : IEquatable<ColumnName>, IComparable<ColumnName>
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ColumnName"/> class with the specified column name.
//        /// </summary>
//        /// <param name="name">The name of the column. Cannot be <see langword="null"/>.</param>
//        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null"/>.</exception>
//        public ColumnName(string name)
//        {
//            Name = name ?? throw new ArgumentNullException(nameof(name));
//        }

//        /// <summary>
//        /// Gets the name associated with the current instance.
//        /// </summary>
//        public string Name { get; }

//        /// <summary>
//        /// Implicitly converts a <see cref="ColumnName"/> instance to its string representation.
//        /// </summary>
//        /// <param name="columnName">The <see cref="ColumnName"/> instance to convert.</param>
//        public static implicit operator string(ColumnName columnName)
//        {
//            return columnName.Name;
//        }

//        /// <summary>
//        /// Defines an implicit conversion from a <see cref="string"/> to a <see cref="ColumnName"/>.
//        /// </summary>
//        /// <param name="name">The name of the column as a string.</param>
//        public static implicit operator ColumnName(string name)
//        {
//            return new ColumnName(name);
//        }

//        /// <summary>
//        /// Determines whether the current <see cref="ColumnName"/> instance is equal to another specified <see
//        /// cref="ColumnName"/> instance.
//        /// </summary>
//        /// <param name="other">The <see cref="ColumnName"/> instance to compare with the current instance.</param>
//        /// <returns><see langword="true"/> if the <paramref name="other"/> instance has the same name as the current instance, 
//        /// using a case-insensitive comparison; otherwise, <see langword="false"/>.</returns>
//        public bool Equals(ColumnName other)
//        {
//            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
//        }

//        /// <summary>
//        /// Determines whether the specified object is equal to the current instance.
//        /// </summary>
//        /// <param name="obj">The object to compare with the current instance.</param>
//        /// <returns><see langword="true"/> if the specified object is a <see cref="ColumnName"/> and is equal to the current
//        /// instance;  otherwise, <see langword="false"/>. </returns>
//        public override bool Equals(object obj)
//        {
//            return obj is ColumnName other && Equals(other);
//        }

//        /// <summary>
//        /// Returns a hash code for the current instance based on the value of the <see cref="Name"/> property.
//        /// </summary>
//        /// <remarks>The hash code is computed using a case-insensitive comparison of the <see
//        /// cref="Name"/> property. This ensures consistent hash codes for strings that differ only in casing.</remarks>
//        /// <returns>An integer representing the hash code for the current instance.</returns>
//        public override int GetHashCode()
//        {
//            return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
//        }

//        /// <summary>
//        /// Compares the current <see cref="ColumnName"/> instance to another <see cref="ColumnName"/> instance and
//        /// returns an integer that indicates their relative order.
//        /// </summary>
//        /// <remarks>The comparison is performed using a case-insensitive ordinal string
//        /// comparison.</remarks>
//        /// <param name="other">The <see cref="ColumnName"/> instance to compare to the current instance.  This parameter can be <see
//        /// langword="null"/>.</param>
//        /// <returns>A signed integer that indicates the relative order of the instances: <list type="bullet">
//        /// <item><description>Returns 1 if <paramref name="other"/> is <see langword="null"/>.</description></item>
//        /// <item><description>Returns 0 if the <see cref="Name"/> properties of both instances are equal, ignoring
//        /// case.</description></item> <item><description>Returns a negative value if the current instance precedes
//        /// <paramref name="other"/> in the sort order.</description></item> <item><description>Returns a positive value
//        /// if the current instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
//        public int CompareTo(ColumnName other)
//        {
//            if (other == default) return 1; // Null is considered less than any instance
//            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
//        }

//        /// <summary>
//        /// Determines whether two <see cref="ColumnName"/> instances are equal.
//        /// </summary>
//        /// <param name="left">The first <see cref="ColumnName"/> instance to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> instance to compare.</param>
//        /// <returns><see langword="true"/> if the two <see cref="ColumnName"/> instances are equal; otherwise, <see
//        /// langword="false"/>.</returns>
//        public static bool operator ==(ColumnName left, ColumnName right)
//        {
//            return left.Equals(right);
//        }

//        /// <summary>
//        /// Determines whether two <see cref="ColumnName"/> instances are not equal.
//        /// </summary>
//        /// <param name="left">The first <see cref="ColumnName"/> instance to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> instance to compare.</param>
//        /// <returns><see langword="true"/> if the two <see cref="ColumnName"/> instances are not equal;  otherwise, <see
//        /// langword="false"/>.</returns>
//        public static bool operator !=(ColumnName left, ColumnName right)
//        {
//            return !left.Equals(right);
//        }

//        /// <summary>
//        /// Determines whether one <see cref="ColumnName"/> instance is less than another.
//        /// </summary>
//        /// <remarks>This operator uses the <see cref="CompareTo"/> method to perform the
//        /// comparison.</remarks>
//        /// <param name="left">The first <see cref="ColumnName"/> instance to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> instance to compare.</param>
//        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
//        /// langword="false"/>.</returns>
//        public static bool operator <(ColumnName left, ColumnName right)
//        {
//            return left.CompareTo(right) < 0;
//        }

//        /// <summary>
//        /// Determines whether the specified <see cref="ColumnName"/> instance is greater than another.
//        /// </summary>
//        /// <param name="left">The first <see cref="ColumnName"/> instance to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> instance to compare.</param>
//        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
//        /// langword="false"/>.</returns>
//        public static bool operator >(ColumnName left, ColumnName right)
//        {
//            return left.CompareTo(right) > 0;
//        }

//        /// <summary>
//        /// Determines whether the left <see cref="ColumnName"/> is less than or equal to the right <see
//        /// cref="ColumnName"/>.
//        /// </summary>
//        /// <param name="left">The first <see cref="ColumnName"/> to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> to compare.</param>
//        /// <returns><see langword="true"/> if the left <see cref="ColumnName"/> is less than or equal to the right <see
//        /// cref="ColumnName"/>; otherwise, <see langword="false"/>.</returns>
//        public static bool operator <=(ColumnName left, ColumnName right)
//        {
//            return left.CompareTo(right) <= 0;
//        }

//        /// <summary>
//        /// Determines whether the left <see cref="ColumnName"/> is greater than or equal to the right <see
//        /// cref="ColumnName"/>.
//        /// </summary>
//        /// <param name="left">The first <see cref="ColumnName"/> to compare.</param>
//        /// <param name="right">The second <see cref="ColumnName"/> to compare.</param>
//        /// <returns><see langword="true"/> if the left <see cref="ColumnName"/> is greater than or equal to the right <see
//        /// cref="ColumnName"/>; otherwise, <see langword="false"/>.</returns>
//        public static bool operator >=(ColumnName left, ColumnName right)
//        {
//            return left.CompareTo(right) >= 0;
//        }
//    }
//}
