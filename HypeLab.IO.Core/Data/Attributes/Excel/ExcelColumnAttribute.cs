namespace HypeLab.IO.Core.Data.Attributes.Excel
{
    /// <summary>
    /// Specifies metadata for mapping a property to an Excel column during read and write operations.
    /// </summary>
    /// <remarks>This attribute is applied to properties to define their association with a specific column in
    /// an Excel file. It allows customization of the column name and enables or disables read and write operations for
    /// the property.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelColumnAttribute : Attribute
    {
        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the read operation is enabled. (default is true).
        /// </summary>
        public bool OnRead { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether write operations are enabled. (default is true).
        /// </summary>
        public bool OnWrite { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether an exception should be thrown if a specified column is not found.
        /// </summary>
        public bool ThrowExceptionIfNotFound { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelColumnAttribute"/> class with the specified column name.
        /// </summary>
        /// <param name="name">The name of the Excel column associated with this attribute. Cannot be null or empty.</param>
        public ExcelColumnAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelColumnAttribute"/> class with the specified column name
        /// and read/write behavior.
        /// </summary>
        /// <param name="name">The name of the Excel column associated with this attribute. Cannot be null or empty.</param>
        /// <param name="onRead">A value indicating whether the column should be included during read operations. <see langword="true"/> to
        /// include the column; otherwise, <see langword="false"/>.</param>
        /// <param name="onWrite">A value indicating whether the column should be included during write operations. <see langword="true"/> to
        /// include the column; otherwise, <see langword="false"/>.</param>
        public ExcelColumnAttribute(string name, bool onRead, bool onWrite)
        {
            Name = name;
            OnRead = onRead;
            OnWrite = onWrite;
        }
    }
}
