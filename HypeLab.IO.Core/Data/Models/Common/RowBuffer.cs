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

        public readonly List<string> ToList()
        {
            List<string> list = new(_count);
            for (int i = 0; i < _count; i++)
            {
                list.Add(_array[i]);
            }

            return list;
        }

        public readonly string[] ToArray()
        {
            return [.. _array.Take(_count)];
        }

        public void Reset()
        {
            for (int i = 0; i < _count; i++)
                _array[i] = string.Empty;
            _count = 0;
        }

        public readonly void Return()
        {
            ArrayPool<string>.Shared.Return(_array, clearArray: true);
        }
    }
}
