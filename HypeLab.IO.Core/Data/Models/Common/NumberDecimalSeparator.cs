namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a decimal separator used in numeric formatting, supporting either a dot (".") or a comma (",").
    /// </summary>
    /// <remarks>This struct provides a type-safe way to specify and work with decimal separators in numeric
    /// operations or formatting. It ensures that only valid separators (dot or comma) are used, and offers predefined
    /// instances for both.</remarks>
    public readonly struct NumberDecimalSeparator : IEquatable<NumberDecimalSeparator>
    {
        private readonly string _dotSeparator = ".";
        private readonly string _commaSeparator = ",";

        /// <summary>
        /// Gets the string used to separate elements in a collection or output.
        /// </summary>
        public string Separator { get; }

        /// <summary>
        /// Gets a value indicating whether the current separator is a dot ('.').
        /// </summary>
        public bool IsDot => Separator == _dotSeparator;
        /// <summary>
        /// Gets a value indicating whether the current separator is a comma.
        /// </summary>
        public bool IsComma => Separator == _commaSeparator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberDecimalSeparator"/> class with the specified decimal
        /// separator.
        /// </summary>
        /// <param name="separator">The decimal separator to use. Must be either <c>"."</c> or <c>","</c>.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="separator"/> is <see langword="null"/>, consists only of whitespace,  or is not
        /// <c>"."</c> or <c>","</c>.</exception>
        public NumberDecimalSeparator(string separator)
        {
            if (string.IsNullOrWhiteSpace(separator) || (separator != _dotSeparator && separator != _commaSeparator))
                throw new ArgumentException("Invalid decimal separator. Use '.' or ','", nameof(separator));

            Separator = separator;
        }

        /// <summary>
        /// Gets a <see cref="NumberDecimalSeparator"/> instance representing a dot (".") as the decimal separator.
        /// </summary>
        public static NumberDecimalSeparator Dot => new(".");
        /// <summary>
        /// Gets a <see cref="NumberDecimalSeparator"/> instance representing a comma (",") as the decimal separator.
        /// </summary>
        public static NumberDecimalSeparator Comma => new(",");

        /// <summary>
        /// Implicitly converts a string to a <see cref="NumberDecimalSeparator"/> instance.
        /// </summary>
        /// <param name="separator">The string representation of the decimal separator.</param>
        public static implicit operator NumberDecimalSeparator(string separator)
        {
            return new NumberDecimalSeparator(separator);
        }

        /// <summary>
        /// Converts a <see cref="NumberDecimalSeparator"/> instance to its string representation.
        /// </summary>
        /// <param name="separator">The <see cref="NumberDecimalSeparator"/> instance to convert.</param>
        public static implicit operator string(NumberDecimalSeparator separator)
        {
            return separator.Separator;
        }

        /// <summary>
        /// Returns a string representation of the current value of the separator.
        /// </summary>
        /// <returns>A string that represents the current object, formatted as "Current separator: '{Separator}'".</returns>
        public override string ToString()
        {
            return $"Current separator: '{Separator}'";
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is derived from the <see cref="Separator"/> property.  This ensures
        /// that objects with the same <see cref="Separator"/> value produce the same hash code.</remarks>
        /// <returns>An integer representing the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Separator?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. This can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is of type <c>NumberDecimalSeparator</c> and is equal to the
        /// current instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is NumberDecimalSeparator other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current instance is equal to another instance of <see
        /// cref="NumberDecimalSeparator"/>.
        /// </summary>
        /// <param name="other">The <see cref="NumberDecimalSeparator"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance and the <paramref name="other"/> instance have the same
        /// separator value; otherwise, <see langword="false"/>.</returns>
        public bool Equals(NumberDecimalSeparator other)
        {
            return Separator == other.Separator;
        }

        /// <summary>
        /// Determines whether two <see cref="NumberDecimalSeparator"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="NumberDecimalSeparator"/> instance to compare.</param>
        /// <param name="right">The second <see cref="NumberDecimalSeparator"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(NumberDecimalSeparator left, NumberDecimalSeparator right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="NumberDecimalSeparator"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="NumberDecimalSeparator"/> instance to compare.</param>
        /// <param name="right">The second <see cref="NumberDecimalSeparator"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="NumberDecimalSeparator"/> instances are not equal;  otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator !=(NumberDecimalSeparator left, NumberDecimalSeparator right)
        {
            return !(left == right);
        }
    }
}
