namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Represents the name of a worksheet in a spreadsheet, ensuring it adheres to specific constraints.
    /// </summary>
    /// <remarks>A <see cref="SheetName"/> is a value type that enforces rules for valid worksheet names, such
    /// as length restrictions and invalid character filtering. It is immutable and provides comparison and equality
    /// operations for use in collections or sorting scenarios.</remarks>
    public readonly struct SheetName : IEquatable<SheetName>, IComparable<SheetName>
    {
        private static readonly char[] _invalidChars = [':', '\\', '/', '?', '*', '[', ']'];
        private readonly string _value;

        /// <summary>
        /// Represents the maximum allowable length for a specific value or input.
        /// </summary>
        /// <remarks>This constant can be used to validate or enforce length constraints in scenarios
        /// where the maximum length is restricted to 31 characters.</remarks>
        public const int MaxLength = 31;

        /// <summary>
        /// Initializes a new instance of the <see cref="SheetName"/> class with the specified name.
        /// </summary>
        /// <remarks>The sheet name must adhere to specific constraints to ensure validity. The maximum
        /// length  and invalid characters are defined by the implementation. If the provided name violates  any of
        /// these constraints, an exception is thrown.</remarks>
        /// <param name="name">The name of the sheet. Must be a non-empty string, cannot exceed the maximum allowed length,  and must not
        /// contain invalid characters.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null, empty, consists only of whitespace, exceeds the  maximum allowed
        /// length, or contains invalid characters.</exception>
        public SheetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > MaxLength)
                throw new ArgumentException("Sheet name cannot be null or empty.");

            if (name.Length > MaxLength)
                throw new ArgumentException($"Sheet name cannot exceed {MaxLength} characters.");

            if (name.IndexOfAny(_invalidChars) >= 0)
                throw new ArgumentException($"Sheet name contains invalid characters like {string.Join(", ", _invalidChars)}.");

            _value = name;
        }

        /// <summary>
        /// Returns the string representation of the current object.
        /// </summary>
        /// <returns>The string value represented by this instance.</returns>
        public override string ToString()
            => _value;

        /// <summary>
        /// Creates a sanitized <see cref="SheetName"/> instance by removing invalid characters and truncating the input
        /// string to the maximum allowed length.
        /// </summary>
        /// <param name="rawName">The raw name to sanitize. If the value is <see langword="null"/>, empty, or consists only of whitespace, a
        /// default name of "Sheet1" will be used.</param>
        /// <returns>A <see cref="SheetName"/> instance containing the sanitized name. The name will exclude invalid characters
        /// and will be truncated if it exceeds the maximum allowed length.</returns>
        public static SheetName CreateSanitized(string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName))
                rawName = "Sheet1";

            string clean = new([.. rawName.Where(c => !_invalidChars.Contains(c))]);

            if (clean.Length > MaxLength)
                clean = clean.Substring(0, MaxLength);

            return new SheetName(clean);
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the underlying value of the object.  It is suitable for
        /// use in hashing algorithms and data structures such as hash tables.</remarks>
        /// <returns>An integer that represents the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="SheetName"/> and is equal to the current
        /// instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is SheetName other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current <see cref="SheetName"/> is equal to another <see cref="SheetName"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="SheetName"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current <see cref="SheetName"/> and the specified <see cref="SheetName"/>
        /// have the same value, ignoring case; otherwise, <see langword="false"/>.</returns>
        public bool Equals(SheetName other)
        {
            return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares the current <see cref="SheetName"/> instance with another <see cref="SheetName"/> instance and
        /// returns an integer that indicates their relative order.
        /// </summary>
        /// <param name="other">The <see cref="SheetName"/> instance to compare to the current instance.</param>
        /// <returns>A value that indicates the relative order of the instances: <list type="bullet"> <item><description>Less
        /// than zero if the current instance precedes <paramref name="other"/> in the sort order.</description></item>
        /// <item><description>Zero if the current instance is equal to <paramref name="other"/>.</description></item>
        /// <item><description>Greater than zero if the current instance follows <paramref name="other"/> in the sort
        /// order.</description></item> </list></returns>
        public int CompareTo(SheetName other)
        {
            return string.CompareOrdinal(_value, other._value);
        }

        /// <summary>
        /// Converts a <see cref="SheetName"/> instance to its string representation.
        /// </summary>
        /// <param name="sheetName">The <see cref="SheetName"/> instance to convert.</param>
        public static implicit operator string(SheetName sheetName)
        {
            return sheetName._value;
        }

        /// <summary>
        /// Defines an implicit conversion from a <see cref="string"/> to a <see cref="SheetName"/>.
        /// </summary>
        /// <param name="name">The string value to convert to a <see cref="SheetName"/>.</param>
        public static implicit operator SheetName(string name)
        {
            return new SheetName(name);
        }

        /// <summary>
        /// Determines whether two <see cref="SheetName"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="SheetName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="SheetName"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(SheetName left, SheetName right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="SheetName"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="SheetName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="SheetName"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(SheetName left, SheetName right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether one <see cref="SheetName"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="SheetName.CompareTo(SheetName)"/> method to perform
        /// the comparison.</remarks>
        /// <param name="left">The first <see cref="SheetName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(SheetName left, SheetName right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="SheetName"/> instance is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="SheetName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(SheetName left, SheetName right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the first <see cref="SheetName"/> instance is less than or equal to the second instance.
        /// </summary>
        /// <param name="left">The first <see cref="SheetName"/> instance to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the first instance is less than or equal to the second instance; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <=(SheetName left, SheetName right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="SheetName"/> is greater than or equal to the right <see
        /// cref="SheetName"/>.
        /// </summary>
        /// <param name="left">The first <see cref="SheetName"/> to compare.</param>
        /// <param name="right">The second <see cref="SheetName"/> to compare.</param>
        /// <returns><see langword="true"/> if the left <see cref="SheetName"/> is greater than or equal to the right <see
        /// cref="SheetName"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(SheetName left, SheetName right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
