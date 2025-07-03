using HypeLab.IO.Core.Data.Options.Impl.Excel;
using System.Collections;

namespace HypeLab.IO.Core.Data.Models.Excel.Worksheet
{
    /// <summary>
    /// Represents a strongly-typed Excel worksheet containing data of a specified model type.
    /// </summary>
    /// <remarks>This class provides functionality for creating and managing an Excel worksheet, including its
    /// name, data, and optional configuration settings. It supports equality and comparison operations based on the
    /// worksheet's name and content.</remarks>
    /// <typeparam name="T">The type of the data model contained in the worksheet. Must be a reference type.</typeparam>
    public sealed class ExcelWorksheet<T> : IExcelWorksheet, IEquatable<ExcelWorksheet<T>>, IComparable<ExcelWorksheet<T>>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorksheet{T}"/> class.
        /// </summary>
        /// <remarks>This constructor creates an empty worksheet instance that can be used to represent
        /// and manipulate Excel worksheet data.</remarks>
        public ExcelWorksheet() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorksheet{T}"/> class with the specified sheet name, data,
        /// and optional configuration.
        /// </summary>
        /// <param name="name">The name of the worksheet. This value cannot be null or empty.</param>
        /// <param name="data">The collection of data to populate the worksheet. This value cannot be null.</param>
        /// <param name="options">Optional configuration for the worksheet, such as formatting or layout options. If not provided, default
        /// options will be used.</param>
        public ExcelWorksheet(SheetName name, IEnumerable<T> data, ExcelWorksheetOptions? options = null)
        {
            Name = name;
            Data = data;
            Options = options ?? new ExcelWorksheetOptions();
        }
        /// <summary>
        /// Gets or sets the name of the sheet.
        /// </summary>
        public SheetName Name { get; set; }
        /// <summary>
        /// Gets or sets the collection of data items to be written to the sheet.
        /// </summary>
        public IEnumerable<T> Data { get; set; } = []; // the data to write in the sheet 

        /// <summary>
        /// Gets or sets the options for writing the Excel worksheet.
        /// </summary>
        public ExcelWorksheetOptions? Options { get; set; } // options for writing the sheet, can be null to use default options

        /// <summary>
        /// Creates a new instance of the <see cref="ExcelWorksheet{T}"/> class with the specified sheet name, data, and
        /// options.
        /// </summary>
        /// <param name="name">The name of the worksheet. This value cannot be null or empty.</param>
        /// <param name="data">The collection of data to populate the worksheet. This value cannot be null.</param>
        /// <param name="options">Optional configuration settings for the worksheet. If null, default options will be applied.</param>
        /// <returns>A new <see cref="ExcelWorksheet{T}"/> instance containing the specified data and options.</returns>
        public static ExcelWorksheet<T> Create(SheetName name, IEnumerable<T> data, ExcelWorksheetOptions? options = null)
        {
            return new ExcelWorksheet<T>(name, data, options);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the model represented by the generic parameter <c>T</c>.
        /// </summary>
        public Type ModelType => typeof(T);

        /// <summary>
        /// Retrieves the collection of data items.
        /// </summary>
        /// <returns>An <see cref="IEnumerable"/> containing the data items. The collection may be empty if no data is available.</returns>
        public IEnumerable GetData() => Data;

        /// <summary>
        /// Returns a hash code for the current object.
        /// </summary>
        /// <remarks>The hash code is computed based on the values of the <see cref="Name"/>, <see
        /// cref="Data"/>,  and <see cref="Options"/> properties. This method ensures that objects with the same
        /// property  values produce the same hash code.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + Name.GetHashCode();
                hash = (hash * 23) + (Data?.GetHashCode() ?? 0);
                hash = (hash * 23) + (Options?.GetHashCode() ?? 0);
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is an <see cref="ExcelWorksheet{T}"/> and is equal to the
        /// current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ExcelWorksheet<T> other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current <see cref="ExcelWorksheet{T}"/> is equal to another <see
        /// cref="ExcelWorksheet{T}"/> instance.
        /// </summary>
        /// <remarks>Two <see cref="ExcelWorksheet{T}"/> instances are considered equal if their <see
        /// cref="Name"/>, <see cref="Data"/>, and <see cref="Options"/> properties are equal.</remarks>
        /// <param name="other">The <see cref="ExcelWorksheet{T}"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <paramref name="other"/> instance is equal to the current instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(ExcelWorksheet<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Name.Equals(other.Name) &&
                   EqualityComparer<IEnumerable<T>>.Default.Equals(Data, other.Data) &&
                   EqualityComparer<ExcelWorksheetOptions?>.Default.Equals(Options, other.Options);
        }

        /// <summary>
        /// Compares the current <see cref="ExcelWorksheet{T}"/> instance to another instance and returns an integer that
        /// indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is based on the <see cref="Name"/> property of the <see
        /// cref="ExcelWorksheet{T}"/> instances. Null is considered less than any instance, and two instances are
        /// considered equal if they reference the same object.</remarks>
        /// <param name="other">The <see cref="ExcelWorksheet{T}"/> instance to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the instances: <list type="bullet"> <item><description>Less
        /// than zero if this instance precedes <paramref name="other"/> in the sort order.</description></item>
        /// <item><description>Zero if this instance is equal to <paramref name="other"/>.</description></item>
        /// <item><description>Greater than zero if this instance follows <paramref name="other"/> in the sort
        /// order.</description></item> </list></returns>
        public int CompareTo(ExcelWorksheet<T> other)
        {
            if (other is null) return 1; // null is considered less than any instance
            if (ReferenceEquals(this, other)) return 0; // same instance
            return Name.CompareTo(other.Name); // compare by name
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelWorksheet{T}"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelWorksheet{T}"/> instances are equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelWorksheet{T}"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelWorksheet{T}"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelWorksheet{T}"/> instance is less than another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="IComparable{T}.CompareTo(T)"/> method to determine
        /// the relative order of the two <see cref="ExcelWorksheet{T}"/> instances. A <see langword="null"/> instance is
        /// considered less than any non-<see langword="null"/> instance.</remarks>
        /// <param name="left">The first <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>. If <paramref name="left"/> is <see langword="null"/>, the result is <see
        /// langword="true"/> unless <paramref name="right"/> is also <see langword="null"/>.</returns>
        public static bool operator <(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            if (left is null) return right is not null; // null is less than any instance
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelWorksheet{T}"/> instance is greater than the right <see
        /// cref="ExcelWorksheet{T}"/> instance.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheet{T}"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the <paramref name="left"/> instance is greater than the <paramref name="right"/>
        /// instance; otherwise, <see langword="false"/>. If <paramref name="left"/> is <see langword="null"/>, the
        /// method returns <see langword="false"/>.</returns>
        public static bool operator >(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            if (left is null) return false; // null is not greater than any instance
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelWorksheet{T}"/> is less than or equal to the right <see
        /// cref="ExcelWorksheet{T}"/>.
        /// </summary>
        /// <remarks>This operator evaluates to <see langword="true"/> if the left operand is either less
        /// than or equal to the right operand. The comparison logic depends on the implementation of the <c>&lt;</c>
        /// and <c>==</c> operators for <see cref="ExcelWorksheet{T}"/>.</remarks>
        /// <param name="left">The left-hand <see cref="ExcelWorksheet{T}"/> operand to compare.</param>
        /// <param name="right">The right-hand <see cref="ExcelWorksheet{T}"/> operand to compare.</param>
        /// <returns><see langword="true"/> if the left <see cref="ExcelWorksheet{T}"/> is less than or equal to the right <see
        /// cref="ExcelWorksheet{T}"/>;  otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            return left < right || left == right;
        }

        /// <summary>
        /// Determines whether the left <see cref="ExcelWorksheet{T}"/> is greater than or equal to the right <see
        /// cref="ExcelWorksheet{T}"/>.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelWorksheet{T}"/> to compare.</param>
        /// <param name="right">The second <see cref="ExcelWorksheet{T}"/> to compare.</param>
        /// <returns><see langword="true"/> if the left <see cref="ExcelWorksheet{T}"/> is greater than or equal to the right <see
        /// cref="ExcelWorksheet{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(ExcelWorksheet<T> left, ExcelWorksheet<T> right)
        {
            return left > right || left == right;
        }
    }
}
