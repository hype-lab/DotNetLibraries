using System.Buffers;

namespace HypeLab.IO.Core.Data.Models.Common
{
    /// <summary>
    /// Represents a buffer for storing rows of string data, backed by a pooled array to optimize memory usage.
    /// </summary>
    public ref struct RowBuffer
    {
        private readonly string[] _array;

        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowBuffer"/> class with the specified size.
        /// </summary>
        /// <remarks>The buffer is backed by a pooled array obtained from <see cref="ArrayPool{T}"/> to
        /// optimize memory usage. The buffer's capacity is determined by the <paramref name="size"/> parameter, and the
        /// initial count is set to zero.</remarks>
        /// <param name="size">The initial capacity of the buffer. Must be a positive integer.</param>
        public RowBuffer(int size)
        {
            _array = ArrayPool<string>.Shared.Rent(size);
            _count = 0;
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        public readonly int Count => _count;

        /// <summary>
        /// Sets the specified value at the given index in the array.
        /// </summary>
        /// <remarks>If the specified index exceeds the current count of elements, the count is updated to
        /// include the new index.</remarks>
        /// <param name="index">The zero-based index at which to set the value. Must be within the bounds of the array.</param>
        /// <param name="value">The value to set at the specified index.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than or equal to the length of the array.</exception>
        public void Set(int index, string value)
        {
            if ((uint)index >= (uint)_array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            _array[index] = value;

            if (index + 1 > _count)
                _count = index + 1;
        }

        /// <summary>
        /// Converts the current collection to an array.
        /// </summary>
        /// <returns>An array containing all elements in the collection, in the same order as they appear in the collection.</returns>
        public readonly string[] ToArray()
        {
            string[] result = new string[_count];
            Array.Copy(_array, 0, result, 0, _count);
            return result;
        }

        /// <summary>
        /// Resets the collection to its initial state by clearing all elements and setting the count to zero.
        /// </summary>
        public void Reset()
        {
            if (_count > 0)
                Array.Clear(_array, 0, _count);

            _count = 0;
        }

        /// <summary>
        /// Returns the array to the shared array pool and clears its contents.
        /// </summary>
        /// <remarks>This method releases the array back to the shared <see cref="ArrayPool{T}"/>
        /// instance, making it available for reuse. The array's contents are cleared before returning it to the pool to
        /// ensure no sensitive or stale data remains.</remarks>
        public readonly void Return()
        {
            ArrayPool<string>.Shared.Return(_array, clearArray: true);
        }
    }
}
