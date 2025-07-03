using System.Collections;

namespace HypeLab.IO.Core.Data.Models.Excel.Worksheet
{
    /// <summary>
    /// Represents a collection of Excel worksheets that provides list-like functionality for managing <see
    /// cref="IExcelWorksheet"/> objects.
    /// </summary>
    /// <remarks>This class implements the <see cref="IList{T}"/> interface, allowing for operations such as
    /// adding, removing, and accessing worksheets by index. The collection is not read-only, and null values are not
    /// permitted as elements.</remarks>
    public class ExcelWorksheetList : IList<IExcelWorksheet>
    {
        private readonly List<IExcelWorksheet> _worksheets = [];

        /// <summary>
        /// Gets or sets the <see cref="IExcelWorksheet"/> at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the worksheet to get or set. Must be within the valid range of the collection.</param>
        /// <returns></returns>
        public IExcelWorksheet this[int index]
        {
            get => _worksheets[index];
            set => _worksheets[index] = value;
        }

        /// <summary>
        /// Gets the number of worksheets in the collection.
        /// </summary>
        public int Count => _worksheets.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds the specified worksheet to the collection.
        /// </summary>
        /// <param name="item">The worksheet to add. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        public void Add(IExcelWorksheet item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Worksheet cannot be null.");

            _worksheets.Add(item);
        }

        /// <summary>
        /// Removes all worksheets from the collection.
        /// </summary>
        /// <remarks>After calling this method, the collection will be empty.</remarks>
        public void Clear()
        {
            _worksheets.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified worksheet.
        /// </summary>
        /// <param name="item">The worksheet to locate in the collection. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified worksheet is found in the collection; otherwise, <see
        /// langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        public bool Contains(IExcelWorksheet item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Worksheet cannot be null.");

            return _worksheets.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the current collection to the specified array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection. The array must
        /// have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in the destination array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="arrayIndex"/> is less than 0, or if the number of elements in the source
        /// collection  exceeds the available space from <paramref name="arrayIndex"/> to the end of the destination
        /// <paramref name="array"/>.</exception>
        public void CopyTo(IExcelWorksheet[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), "Array cannot be null.");
            if (arrayIndex < 0 || arrayIndex + _worksheets.Count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Invalid array index.");
            _worksheets.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of Excel worksheets.
        /// </summary>
        /// <remarks>Use this method to iterate over the worksheets in the collection using a `foreach`
        /// loop.</remarks>
        /// <returns>An enumerator for the collection of <see cref="IExcelWorksheet"/> objects.</returns>
        public IEnumerator<IExcelWorksheet> GetEnumerator()
        {
            return _worksheets.GetEnumerator();
        }

        /// <summary>
        /// Returns the zero-based index of the specified worksheet in the collection.
        /// </summary>
        /// <param name="item">The worksheet to locate in the collection. Cannot be <see langword="null"/>.</param>
        /// <returns>The zero-based index of the specified worksheet if found; otherwise, -1 if the worksheet is not in the
        /// collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        public int IndexOf(IExcelWorksheet item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Worksheet cannot be null.");
            return _worksheets.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified worksheet at the given index in the collection.
        /// </summary>
        /// <remarks>The method shifts any existing worksheets at or after the specified index to the next
        /// position.</remarks>
        /// <param name="index">The zero-based index at which the worksheet should be inserted. Must be within the range of the collection.</param>
        /// <param name="item">The worksheet to insert. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than the number of worksheets in the
        /// collection.</exception>
        public void Insert(int index, IExcelWorksheet item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Worksheet cannot be null.");
            if (index < 0 || index > _worksheets.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            _worksheets.Insert(index, item);
        }

        /// <summary>
        /// Removes the specified worksheet from the collection.
        /// </summary>
        /// <param name="item">The worksheet to remove from the collection. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the worksheet was successfully removed from the collection;  otherwise, <see
        /// langword="false"/> if the worksheet was not found in the collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        public bool Remove(IExcelWorksheet item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Worksheet cannot be null.");
            return _worksheets.Remove(item);
        }

        /// <summary>
        /// Removes the worksheet at the specified index from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the worksheet to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than or equal to the number of worksheets in
        /// the collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _worksheets.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            _worksheets.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
