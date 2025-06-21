using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents a key-value pair for an HTTP request header.
    /// </summary>
    public struct RequestHeaderKeyValue : IEquatable<RequestHeaderKeyValue>
    {
        /// <summary>
        /// Gets or sets the key represented by this instance.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value represented by this instance.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeaderKeyValue"/> class with the specified key and
        /// value.>
        /// </summary>
        /// <param name="key">The key of the request header. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="value">The value of the request header. Cannot be <see langword="null"/> or empty.</param>
        public RequestHeaderKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Generates a hash code for the current object based on its key and value.
        /// </summary>
        /// <remarks>This method combines the hash codes of the <see cref="Key"/> and <see cref="Value"/>
        /// properties  to produce a unique hash code for the object. It is suitable for use in hash-based collections 
        /// such as dictionaries or hash sets.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public readonly override int GetHashCode()
        {
#pragma warning disable S125 // Sections of code should not be commented out
            //unchecked
            //{
            //    int hash = 17;
            //    hash = hash * 23 + Key.GetHashCode();
            //    hash = hash * 23 + Value.GetHashCode();
            //    return hash;
            //}
#pragma warning restore S125 // Sections of code should not be commented out
            return HashCode.Combine(Key, Value);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Must be of type <see cref="RequestHeaderKeyValue"/> to be
        /// considered equal.</param>
        /// <returns><see langword="true"/> if the specified object is a <see cref="RequestHeaderKeyValue"/> and is equal to the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not RequestHeaderKeyValue requestHeaderKeyValue)
                return false;

            return Equals(requestHeaderKeyValue);
        }

        /// <summary>
        /// Determines whether the current <see cref="RequestHeaderKeyValue"/> instance is equal to the specified
        /// instance.
        /// </summary>
        /// <param name="other">The <see cref="RequestHeaderKeyValue"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified instance; otherwise, <see
        /// langword="false"/>. Equality is determined by comparing both the <see cref="Key"/> and <see cref="Value"/>
        /// properties.</returns>
        public readonly bool Equals(RequestHeaderKeyValue other)
        {
            return Key == other.Key && Value == other.Value;
        }

        /// <summary>
        /// Determines whether two <see cref="RequestHeaderKeyValue"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="RequestHeaderKeyValue"/> instance to compare.</param>
        /// <param name="right">The second <see cref="RequestHeaderKeyValue"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(RequestHeaderKeyValue left, RequestHeaderKeyValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="RequestHeaderKeyValue"/> instances are not equal.
        /// </summary>
        /// <remarks>This operator performs a logical negation of the equality operator (<c>==</c>) for
        /// <see cref="RequestHeaderKeyValue"/>.</remarks>
        /// <param name="left">The first <see cref="RequestHeaderKeyValue"/> instance to compare.</param>
        /// <param name="right">The second <see cref="RequestHeaderKeyValue"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(RequestHeaderKeyValue left, RequestHeaderKeyValue right)
        {
            return !(left == right);
        }
    }
}
