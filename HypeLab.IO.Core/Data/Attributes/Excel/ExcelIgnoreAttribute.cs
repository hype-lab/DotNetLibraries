namespace HypeLab.IO.Core.Data.Attributes.Excel
{
    /// <summary>
    /// Specifies that a property should be ignored during Excel serialization or deserialization.
    /// </summary>
    /// <remarks>This attribute can be applied to properties to exclude them from being processed when reading
    /// from or writing to an Excel file. By default, the property is ignored for both operations unless explicitly
    /// configured.</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExcelIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the read operation is enabled. (default is <see langword="true"/>).
        /// </summary>
        public bool OnRead { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether write operations are enabled. (default is <see langword="true"/>).
        /// </summary>
        public bool OnWrite { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelIgnoreAttribute"/> class.
        /// </summary>
        public ExcelIgnoreAttribute() { }

        /// <summary>
        /// Specifies whether a property or field should be ignored during Excel serialization or deserialization.
        /// </summary>
        /// <param name="onRead">Indicates whether the property or field should be ignored during deserialization (reading from Excel).</param>
        /// <param name="onWrite">Indicates whether the property or field should be ignored during serialization (writing to Excel).</param>
        public ExcelIgnoreAttribute(bool onRead, bool onWrite)
        {
            OnRead = onRead;
            OnWrite = onWrite;
        }
    }
}
