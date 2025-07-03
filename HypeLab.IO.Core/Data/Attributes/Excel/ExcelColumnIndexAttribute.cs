namespace HypeLab.IO.Core.Data.Attributes.Excel
{
    /// <summary>
    /// Specifies the zero-based column index in an Excel sheet that corresponds to the decorated property.
    /// </summary>
    /// <remarks>This attribute is used to map a property to a specific column in an Excel sheet during data
    /// import (only on read for now). The column index must be non-negative.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcelColumnIndexAttribute : Attribute // used just in read operations
    {
        /// <summary>
        /// Gets the zero-based index of the current item within a collection.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelColumnIndexAttribute"/> class with the specified column
        /// index.
        /// </summary>
        /// <param name="index">The zero-based index of the Excel column. Must be non-negative.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0.</exception>
        public ExcelColumnIndexAttribute(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Column index must be non-negative.");

            Index = index;
        }
    }
}
