namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a combination of an instance of type <typeparamref name="T"/> and its associated zero-based row
    /// index.
    /// </summary>
    /// <remarks>This class is typically used to associate an object of type <typeparamref name="T"/> with a
    /// specific row index, such as in scenarios involving data grids, tables, or other row-based structures.</remarks>
    /// <typeparam name="T">The type of the instance associated with the row index.</typeparam>
    public class InstanceRowIndex<T>
    {
        /// <summary>
        /// The constructor for the <see cref="InstanceRowIndex{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance associated with the row index. Cannot be <see langword="null"/>.</param>
        /// <param name="rowIndex">The row index associated with the instance. Must be a non-negative integer.</param>
        public InstanceRowIndex(T instance, int rowIndex)
        {
            Instance = instance;
            RowIndex = rowIndex;
        }

        /// <summary>
        /// Gets the singleton instance of type <typeparamref name="T"/>.
        /// </summary>
        public T Instance { get; }
        /// <summary>
        /// Gets the zero-based index of the row associated with this instance.
        /// </summary>
        public int RowIndex { get; }
    }
}
