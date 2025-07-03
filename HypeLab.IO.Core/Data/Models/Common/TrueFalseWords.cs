namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a pair of words used to signify boolean values.
    /// </summary>
    /// <remarks>This class encapsulates two string values, one representing "true" and the other representing
    /// "false." It is useful for scenarios where custom textual representations of boolean values are
    /// required.</remarks>
    public readonly struct TrueFalseWords : IEquatable<TrueFalseWords>, IComparable<TrueFalseWords>
    {
        /// <summary>
        /// Gets the word or phrase that represents a true value in the current context.
        /// </summary>
        public string TrueWord { get; }

        /// <summary>
        /// Gets the word or phrase that represents a false value in the current context.
        /// </summary>
        public string FalseWord { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrueFalseWords"/> class with specified words representing <see
        /// langword="true"/> and <see langword="false"/> values.
        /// </summary>
        /// <param name="trueWord">The word to represent a <see langword="true"/> value. Cannot be null, empty, or whitespace.</param>
        /// <param name="falseWord">The word to represent a <see langword="false"/> value. Cannot be null, empty, or whitespace.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="trueWord"/> is null, empty, or consists only of whitespace. Thrown if <paramref
        /// name="falseWord"/> is null, empty, or consists only of whitespace.</exception>
        public TrueFalseWords(string trueWord, string falseWord)
        {
            if (string.IsNullOrWhiteSpace(trueWord))
                throw new ArgumentException("True word cannot be null or empty.", nameof(trueWord));
            if (string.IsNullOrWhiteSpace(falseWord))
                throw new ArgumentException("False word cannot be null or empty.", nameof(falseWord));

            TrueWord = trueWord;
            FalseWord = falseWord;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TrueFalseWords"/> class with the specified true and false words.
        /// </summary>
        /// <param name="trueWord">The word representing a <see langword="true"/> value.</param>
        /// <param name="falseWord">The word representing a <see langword="false"/> value.</param>
        /// <returns>A new <see cref="TrueFalseWords"/> instance initialized with the specified true and false words.</returns>
        public static TrueFalseWords Create(string trueWord, string falseWord)
        {
            return new TrueFalseWords(trueWord, falseWord);
        }

        /// <summary>
        /// Defines an implicit conversion from a tuple containing true and false words to a <see
        /// cref="TrueFalseWords"/> instance.
        /// </summary>
        /// <param name="words">A tuple containing the true word and false word. The first item represents the word for <see
        /// langword="true"/>,  and the second item represents the word for <see langword="false"/>.</param>
        public static implicit operator TrueFalseWords((string trueWord, string falseWord) words)
        {
            return new TrueFalseWords(words.trueWord, words.falseWord);
        }

        /// <summary>
        /// Implicitly converts a <see cref="TrueFalseWords"/> instance to a tuple containing its true and false word
        /// representations.
        /// </summary>
        /// <param name="words">The <see cref="TrueFalseWords"/> instance to convert.</param>
        public static implicit operator (string trueWord, string falseWord)(TrueFalseWords words)
        {
            return (words.TrueWord, words.FalseWord);
        }

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is computed based on the values of the <see cref="TrueWord"/> and <see
        /// cref="FalseWord"/> properties. This method ensures a consistent hash code for objects with the same property
        /// values.</remarks>
        /// <returns>An integer representing the hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + TrueWord.GetHashCode();
                hash = (hash * 23) + FalseWord.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is of the same type and has the same value as the current
        /// instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is TrueFalseWords other && Equals(other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="TrueFalseWords"/> instance is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="TrueFalseWords"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified instance has the same <see cref="TrueWord"/> and <see cref="FalseWord"/>
        /// values  as the current instance, ignoring case; otherwise, <see langword="false"/>.</returns>
        public bool Equals(TrueFalseWords other)
        {
            return TrueWord.Equals(other.TrueWord, StringComparison.OrdinalIgnoreCase) &&
                   FalseWord.Equals(other.FalseWord, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares the current instance with another <see cref="TrueFalseWords"/> object and returns an integer  that
        /// indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed first on the <see cref="TrueWord"/> property and, if they
        /// are equal,  on the <see cref="FalseWord"/> property. Comparisons are case-insensitive.</remarks>
        /// <param name="other">The <see cref="TrueFalseWords"/> object to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>Less than zero if this instance precedes <paramref name="other"/> in the sort
        /// order.</description></item> <item><description>Zero if this instance occurs in the same position in the sort
        /// order as <paramref name="other"/>.</description></item> <item><description>Greater than zero if this
        /// instance follows <paramref name="other"/> in the sort order.</description></item> </list></returns>
        public int CompareTo(TrueFalseWords other)
        {
            int trueComparison = string.Compare(TrueWord, other.TrueWord, StringComparison.OrdinalIgnoreCase);
            if (trueComparison != 0) return trueComparison;
            return string.Compare(FalseWord, other.FalseWord, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a string representation of the object, including the values of the <see cref="TrueWord"/> and <see
        /// cref="FalseWord"/> properties.
        /// </summary>
        /// <returns>A string in the format "True: {TrueWord}, False: {FalseWord}", where <see cref="TrueWord"/> and <see
        /// cref="FalseWord"/> represent the respective property values.</returns>
        public override string ToString()
        {
            return $"True: {TrueWord}, False: {FalseWord}";
        }

        /// <summary>
        /// Determines whether two <see cref="TrueFalseWords"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="TrueFalseWords"/> instances are equal; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(TrueFalseWords left, TrueFalseWords right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="TrueFalseWords"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(TrueFalseWords left, TrueFalseWords right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Determines whether one <see cref="TrueFalseWords"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the default comparer for the <see cref="TrueFalseWords"/> type to
        /// perform the comparison.</remarks>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(TrueFalseWords left, TrueFalseWords right)
        {
            return Comparer<TrueFalseWords>.Default.Compare(left, right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="TrueFalseWords"/> instance is greater than another.
        /// </summary>
        /// <remarks>The comparison is performed using the default comparer for the <see
        /// cref="TrueFalseWords"/> type. Ensure that the <see cref="TrueFalseWords"/> type implements a meaningful
        /// comparison logic  to produce consistent results.</remarks>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(TrueFalseWords left, TrueFalseWords right)
        {
            return Comparer<TrueFalseWords>.Default.Compare(left, right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="TrueFalseWords"/> instance is less than or equal to the right <see
        /// cref="TrueFalseWords"/> instance.
        /// </summary>
        /// <remarks>This operator uses the default comparer for the <see cref="TrueFalseWords"/> type to
        /// perform the comparison.</remarks>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is less than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(TrueFalseWords left, TrueFalseWords right)
        {
            return Comparer<TrueFalseWords>.Default.Compare(left, right) <= 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="TrueFalseWords"/> instance is greater than or equal to the right <see
        /// cref="TrueFalseWords"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <param name="right">The second <see cref="TrueFalseWords"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than or equal to the <paramref
        /// name="right"/> instance; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(TrueFalseWords left, TrueFalseWords right)
        {
            return Comparer<TrueFalseWords>.Default.Compare(left, right) >= 0;
        }
    }
}
