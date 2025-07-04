using System.Buffers;

namespace HypeLab.IO.Core.Data.Models.Common
{
    public ref struct RowBuffer
    {
        private readonly string[] _array;

        private int _count;

        public RowBuffer(int size)
        {
            _array = ArrayPool<string>.Shared.Rent(size);
            _count = 0;
        }

        public readonly int Count => _count;

        public void Set(int index, string value)
        {
            if ((uint)index >= (uint)_array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            _array[index] = value;

            if (index + 1 > _count)
                _count = index + 1;
        }

        public readonly string[] ToArray()
        {
            string[] result = new string[_count];
            Array.Copy(_array, 0, result, 0, _count);
            return result;
        }

        public void Reset()
        {
            if (_count > 0)
                Array.Clear(_array, 0, _count);

            _count = 0;
        }

        public readonly void Return()
        {
            ArrayPool<string>.Shared.Return(_array, clearArray: true);
        }
    }
}
