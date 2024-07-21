using System.Diagnostics.CodeAnalysis;

namespace HypeLab.MailEngine.Data.Models.Impl.Credentials.ValueTypes
{
    /// <summary>
    /// Represents a key-value pair for request headers.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public struct RequestHeaderKeyValue(string key, string value) : IEquatable<RequestHeaderKeyValue>
    {
        /// <summary>
        /// The key.
        /// </summary>
        public string Key { get; set; } = key;
        /// <summary>
        /// The value.
        /// </summary>
        public string Value { get; set; } = value;

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns></returns>
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
        /// Checks if the object is equal to another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public readonly override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not RequestHeaderKeyValue requestHeaderKeyValue)
                return false;

            return Equals(requestHeaderKeyValue);
        }

        /// <summary>
        /// Checks if the object is equal to another object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(RequestHeaderKeyValue other)
        {
            return Key == other.Key && Value == other.Value;
        }

        /// <summary>
        /// The operator for equality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RequestHeaderKeyValue left, RequestHeaderKeyValue right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// The operator for inequality.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RequestHeaderKeyValue left, RequestHeaderKeyValue right)
        {
            return !(left == right);
        }
    }
}
