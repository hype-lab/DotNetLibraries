using HypeLab.IO.Core.Data.Models.Common;

namespace HypeLab.IO.Core.Helpers.Excel
{
    /// <summary>
    /// Provides functionality to manage and assign unique indices to Excel style definitions.
    /// </summary>
    /// <remarks>This class maintains a collection of style definitions and ensures that each style is
    /// assigned a unique index. It also provides methods to retrieve existing indices or add new styles, as well as to
    /// reset the internal state.</remarks>
    public class ExcelStyleIndexBuilder
    {
        private readonly Dictionary<StyleDefinition, int> _styleCache = [];
        private readonly List<StyleDefinition> _styles = [];

        /// <summary>
        /// Retrieves the index of the specified <see cref="StyleDefinition"/> in the collection,  or adds it to the
        /// collection if it does not already exist.
        /// </summary>
        /// <remarks>This method ensures that each <see cref="StyleDefinition"/> is uniquely indexed
        /// within the collection.  If the style is not already present, it is added to the collection, and its index is
        /// returned.</remarks>
        /// <param name="style">The <see cref="StyleDefinition"/> to retrieve or add.</param>
        /// <returns>The zero-based index of the specified <see cref="StyleDefinition"/> in the collection.  If the style is
        /// newly added, the index corresponds to its position after addition.</returns>
        public int GetOrAddIndex(StyleDefinition style)
        {
            if (_styleCache.TryGetValue(style, out int index))
                return index;

            index = _styles.Count;
            _styleCache[style] = index;
            _styles.Add(style);
            return index;
        }

        /// <summary>
        /// Clears all cached styles and resets the internal style collections.
        /// </summary>
        /// <remarks>This method removes all entries from the style cache and the internal style
        /// collection, effectively resetting the state. Use this method to release cached styles and start
        /// fresh.</remarks>
        public void Flush()
        {
            _styleCache.Clear();
            _styles.Clear();
        }

        /// <summary>
        /// Gets the collection of style definitions associated with the current instance.
        /// </summary>
        /// <remarks>Use this property to access the styles defined for the current instance.  Any
        /// modifications to the styles must be performed on the underlying data source  before accessing this property
        /// again.</remarks>
        public IReadOnlyList<StyleDefinition> Styles => _styles.AsReadOnly();
    }
}
