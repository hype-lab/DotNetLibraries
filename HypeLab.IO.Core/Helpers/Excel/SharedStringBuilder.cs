namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides a mechanism for managing and deduplicating strings, ensuring that each unique string is stored only
    /// once.
    /// </summary>
    /// <remarks>This class is designed to optimize memory usage and improve performance in scenarios where a
    /// large number of duplicate strings need to be managed. It maintains a collection of unique strings and allows
    /// efficient retrieval of their indices or addition of new strings.</remarks>
    public class SharedStringBuilder
    {
        private readonly Dictionary<string, int> _stringIndex = new(StringComparer.Ordinal);
        private readonly List<string> _strings = [];

        /// <summary>
        /// Gets the number of unique strings in the collection.
        /// </summary>
        public int UniqueCount => _strings.Count;
        /// <summary>
        /// Gets the number of elements currently stored in the collection.
        /// </summary>
        public int Count => _stringIndex.Count;

        /// <summary>
        /// Attempts to retrieve the index of the specified string from the collection.  If the string does not exist,
        /// it is added to the collection, and its new index is returned.
        /// </summary>
        /// <param name="value">The string to retrieve or add to the collection. Cannot be <see langword="null"/>.</param>
        /// <returns>The index of the specified string in the collection. If the string was not previously in the collection, 
        /// the index of the newly added string is returned.</returns>
        public int TryGetOrAddIndex(string value)
        {
            if (_stringIndex.TryGetValue(value, out int index))
                return index;

            index = _strings.Count;
            _strings.Add(value);
            _stringIndex[value] = index;
            return index;
        }

        /// <summary>
        /// Clears all stored strings and their associated indices.
        /// </summary>
        /// <remarks>This method resets the internal state by removing all entries from the string
        /// collection and their corresponding index mappings. After calling this method, the collection will be
        /// empty.</remarks>
        public void Flush()
        {
            _stringIndex.Clear();
            _strings.Clear();
        }

        /// <summary>
        /// Retrieves a read-only list of strings in their current order.
        /// </summary>
        /// <remarks>The returned list reflects the current order of the strings in the underlying
        /// collection. Any modifications to the underlying collection will not affect the returned list.</remarks>
        /// <returns>A read-only list of strings representing the current state of the collection.</returns>
        public IReadOnlyList<string> GetOrderedStrings() => _strings.AsReadOnly();
    }
}
