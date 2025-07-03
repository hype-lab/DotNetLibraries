using System.Collections;
using System.Diagnostics;

namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a collection of true/false word mappings associated with column names.
    /// </summary>
    /// <remarks>This class provides functionality to manage mappings between column names and their
    /// corresponding true/false word definitions. It supports adding, retrieving, checking, and removing mappings, as
    /// well as clearing all mappings. The mappings are stored internally as a dictionary.</remarks>
    [DebuggerVisualizer("Name = MultipleTrueFalseWords")]
    public class MultipleTrueFalseWords : IReadOnlyCollection<KeyValuePair<PropertyName, TrueFalseWords>>
    {
        private readonly Dictionary<PropertyName, TrueFalseWords> _wrds;

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public int Count => _wrds.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleTrueFalseWords"/> class with the specified property
        /// name and associated true/false words.
        /// </summary>
        /// <param name="propertyName">The name of the property to associate with the true/false words. Cannot be null or empty.</param>
        /// <param name="words">The <see cref="TrueFalseWords"/> object containing the true/false word mappings. Cannot be <see
        /// langword="default"/>.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="propertyName"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="words"/> is <see langword="default"/>.</exception>
        public MultipleTrueFalseWords(PropertyName propertyName, TrueFalseWords words)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(propertyName));

            if (words == default)
                throw new ArgumentNullException(nameof(words));

            _wrds = [];
            _wrds.Add(propertyName, words);
        }

        /// <summary>
        /// Adds a set of true/false words associated with the specified property name.
        /// </summary>
        /// <remarks>This method updates the internal collection by associating the specified  property
        /// name with the provided true/false words. If the property name  already exists in the collection, its
        /// associated value will be replaced.</remarks>
        /// <param name="propertyName">The name of the property to associate with the true/false words.  Cannot be <see langword="null"/> or empty.</param>
        /// <param name="words">The set of true/false words to associate with the property name.  Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is <see langword="null"/> or empty,  or if <paramref
        /// name="words"/> is <see langword="null"/>.</exception>
        public void AddWords(PropertyName propertyName, TrueFalseWords words)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (words == default)
                throw new ArgumentNullException(nameof(words));

            _wrds[propertyName] = words;
        }

        /// <summary>
        /// Attempts to retrieve the <see cref="TrueFalseWords"/> associated with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to retrieve the associated <see cref="TrueFalseWords"/>. Must not be <see
        /// langword="null"/> or empty.</param>
        /// <param name="words">When this method returns, contains the <see cref="TrueFalseWords"/> associated with the specified property
        /// name, if the retrieval was successful; otherwise, contains the default value for the type.</param>
        /// <returns><see langword="true"/> if the <see cref="TrueFalseWords"/> were successfully retrieved; otherwise, <see
        /// langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is <see langword="null"/> or empty.</exception>
        public bool TryGetWords(PropertyName propertyName, out TrueFalseWords words)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _wrds.TryGetValue(propertyName, out words);
        }

        /// <summary>
        /// Determines whether the specified property exists in the collection.
        /// </summary>
        /// <param name="propertyName">The name of the property to check. Cannot be <see langword="null"/> or empty.</param>
        /// <returns><see langword="true"/> if the property exists in the collection; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is <see langword="null"/> or an empty string.</exception>
        public bool ContainsProperty(PropertyName propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            return _wrds.ContainsKey(propertyName);
        }

        /// <summary>
        /// Retrieves all words categorized by their associated property names.
        /// </summary>
        /// <remarks>The returned dictionary provides a snapshot of the current state of the words.
        /// Modifications to the dictionary or its contents are not allowed.</remarks>
        /// <returns>A read-only dictionary where the keys represent property names and the values  are <see
        /// cref="TrueFalseWords"/> objects containing categorized true/false words.</returns>
        public IReadOnlyDictionary<PropertyName, TrueFalseWords> GetAllWords()
        {
            return _wrds;
        }

        /// <summary>
        /// Removes the specified word from the collection associated with the given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose associated word should be removed.  Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyName"/> is <see langword="null"/> or an empty string.</exception>
        public void RemoveWords(PropertyName propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            _wrds.Remove(propertyName);
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            _wrds.Clear();
        }

        /// <summary>
        /// Returns a string representation of the current object, listing key-value pairs in the format "Key: Value".
        /// </summary>
        /// <remarks>The string representation includes all key-value pairs in the collection, separated
        /// by commas.</remarks>
        /// <returns>A string containing the key-value pairs in the format "Key: Value", separated by commas.</returns>
        public override string ToString()
        {
            return string.Join(", ", _wrds.Select(kv => $"{kv.Key}: {kv.Value}"));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of key-value pairs.
        /// </summary>
        /// <remarks>The enumerator provides access to each <see cref="KeyValuePair{TKey, TValue}"/> in
        /// the collection, where the key is of type <see cref="PropertyName"/> and the value is of type <see
        /// cref="TrueFalseWords"/>.</remarks>
        /// <returns>An <see cref="IEnumerator{T}"/> for iterating through the collection of key-value pairs.</returns>
        public IEnumerator<KeyValuePair<PropertyName, TrueFalseWords>> GetEnumerator()
        {
            return new MultipleTrueFalseWordsEnumerator(_wrds.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly struct MultipleTrueFalseWordsEnumerator : IEnumerator<KeyValuePair<PropertyName, TrueFalseWords>>
        {
            private readonly Dictionary<PropertyName, TrueFalseWords>.Enumerator _enumerator;

            public MultipleTrueFalseWordsEnumerator(Dictionary<PropertyName, TrueFalseWords>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public KeyValuePair<PropertyName, TrueFalseWords> Current => _enumerator.Current;
            object IEnumerator.Current => Current;
            public void Dispose() => _enumerator.Dispose();
            public bool MoveNext() => _enumerator.MoveNext();

            // Remove the Reset method as Dictionary<TKey, TValue>.Enumerator does not support Reset.
            public void Reset() => throw new NotSupportedException("Reset is not supported for Dictionary<TKey, TValue>.Enumerator.");
        }
    }
}
