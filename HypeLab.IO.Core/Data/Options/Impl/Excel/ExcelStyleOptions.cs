using HypeLab.IO.Core.Helpers.Const;

namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    /// <summary>
    /// Represents a set of customizable style options for formatting Excel sheets,  including header and row styles,
    /// gridline visibility, and number/date formats.
    /// </summary>
    /// <remarks>This class provides a comprehensive set of properties to define the appearance of Excel
    /// sheets,  such as font colors, background colors, border colors, font styles, and number/date formats.  It also
    /// includes default settings through the <see cref="Empty"/> property, which can be used  as a baseline for
    /// customization. Instances of this class are immutable once configured.</remarks>
    public sealed class ExcelStyleOptions : IEquatable<ExcelStyleOptions>, IComparable<ExcelStyleOptions>
    {
        // header styles
        /// <summary>
        /// Gets or sets the font color used for headers in the UI.
        /// </summary>
        public string HeaderFontColor { get; set; } = "#000000"; // default font color for header cells
        /// <summary>
        /// Gets or sets the background color of the header as a string representation.
        /// </summary>
        public string HeaderBackgroundColor { get; set; } = "#FFFFFF"; // default header background color
        /// <summary>
        /// Gets or sets the border color for header cells.
        /// </summary>
        public string HeaderBorderColor { get; set; } = "#E0E0E0"; // default border color for header cells
        /// <summary>
        /// Gets or sets the name of the font used for headers.
        /// </summary>
        public string HeaderFontName { get; set; } = "Times New Roman"; // default header font name
        /// <summary>
        /// Gets or sets the font size used for headers.
        /// </summary>
        public double HeaderFontSize { get; set; } = 11; // default header font size
        /// <summary>
        /// Gets or sets a value indicating whether the header font is bold.
        /// </summary>
        public bool IsHeaderFontBold { get; set; } = false; // default header font boldness

        // cell styles
        /// <summary>
        /// Gets or sets the default font color for cells in the row.
        /// </summary>
        public string RowFontColor { get; set; } = "#000000"; // default font color for cells
        /// <summary>
        /// Gets or sets an array of background colors used for alternating rows in a table or grid.
        /// </summary>
        public string[] RowBackgroundColors { get; set; } = ["#FFFFFF"]; // default row background colors, can be multiple for alternating rows
        /// <summary>
        /// Gets or sets the color of the border for rows in the table.
        /// </summary>
        /// <remarks>The value should be a valid color representation. If set to null, the default border
        /// color will be used.</remarks>
        public string RowBorderColor { get; set; } = "#E0E0E0"; // default border color for cells
        /// <summary>
        /// Gets or sets the name of the font used for rows in the display.
        /// </summary>
        public string RowFontName { get; set; } = "Times New Roman"; // default row font name
        /// <summary>
        /// Gets or sets the font size used for rows in the display.
        /// </summary>
        public double RowFontSize { get; set; } = 11; // default row font size
        /// <summary>
        /// Gets or sets a value indicating whether the font of the row is bold.
        /// </summary>
        public bool IsRowFontBold { get; set; } = false; // default row font boldness

        // non so bene se servono
        /// <summary>
        /// Gets or sets a value indicating whether grid lines are displayed. (experimental, not used)
        /// </summary>
        public bool ShowGridLines { get; set; } = false; // default grid lines visibility
        /// <summary>
        /// Gets or sets a value indicating whether a border is displayed around cells. (experimental, not used)
        /// </summary>
        public bool BorderAroundCells { get; set; } = false; // default border around cells

        /// <summary>
        /// Gets or sets the number format for integer values in the Excel sheet.
        /// </summary>
        public string IntegerNumberFormat { get; set; } = ExcelDefaults.NumberFormats.Integers.Default; // default integer number format (e.g., "#,##0")
        /// <summary>
        /// Gets or sets the format string used to represent decimal numbers.
        /// </summary>
        public string DecimalNumberFormat { get; set; } = ExcelDefaults.NumberFormats.Decimals.Default; // default decimal number format
        /// <summary>
        /// Gets or sets the date format string used for parsing and formatting dates.
        /// </summary>
        public string DateFormat { get; set; } = ExcelDefaults.NumberFormats.Dates.Iso; // default date format (e.g., "yyyy-MM-dd")

        /// <summary>
        /// Gets an instance of <see cref="ExcelStyleOptions"/> with default styling values.
        /// </summary>
        /// <remarks>Use this property to retrieve a predefined, unstyled configuration for Excel export operations. This
        /// instance can serve as a baseline for customization or as a fallback when no specific styles are required.</remarks>
        public static ExcelStyleOptions Empty => new()
        {
            HeaderFontColor = "#000000",
            HeaderBackgroundColor = "#FFFFFF",
            HeaderBorderColor = "#E0E0E0",
            HeaderFontName = "Times New Roman",
            HeaderFontSize = 11,
            IsHeaderFontBold = false,
            RowBackgroundColors = ["#FFFFFF"],
            RowFontSize = 11,
            RowFontName = "Times New Roman",
            IsRowFontBold = false,
            RowBorderColor = "#E0E0E0", // default border color for cells
            RowFontColor = "#000000",
            ShowGridLines = false,
            BorderAroundCells = false,
            IntegerNumberFormat = ExcelDefaults.NumberFormats.Integers.Default,
            DecimalNumberFormat = ExcelDefaults.NumberFormats.Decimals.Default,
            DateFormat = ExcelDefaults.NumberFormats.Dates.Iso // "yyyy-MM-dd"
        };

        /// <summary>
        /// Gets a predefined set of Excel style options for a table with a bold header and zebra-striped rows.
        /// </summary>
        /// <remarks>The header is bold, while the rows use a standard font
        /// weight. Default number formats are applied for integers, decimals, and dates.</remarks>
        public static ExcelStyleOptions GreyZebraBoldHeader => new()
        {
            HeaderFontColor = "#000000",
            HeaderBackgroundColor = "#D3D3D3", // light grey
            HeaderBorderColor = "#A9A9A9", // dark grey
            HeaderFontName = "Times New Roman",
            HeaderFontSize = 12,
            IsHeaderFontBold = true,
            RowBackgroundColors = ["#FFFFFF", "#F0F0F0"], // white and light grey for zebra striping
            RowFontSize = 11,
            RowFontName = "Arial",
            IsRowFontBold = false,
            RowBorderColor = "#D3D3D3", // light grey
            RowFontColor = "#000000",
            ShowGridLines = true,
            BorderAroundCells = true,
            IntegerNumberFormat = ExcelDefaults.NumberFormats.Integers.Default,
            DecimalNumberFormat = ExcelDefaults.NumberFormats.Decimals.Default,
            DateFormat = ExcelDefaults.NumberFormats.Dates.Iso // "yyyy-MM-dd"
        };

        /// <summary>
        /// Generates a hash code for the current object based on its property values.
        /// </summary>
        /// <remarks>The hash code is computed using the values of the object's properties, including
        /// nullable and non-nullable types. This method ensures that objects with the same property values produce the
        /// same hash code.</remarks>
        /// <returns>An integer representing the hash code of the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + HeaderFontColor.GetHashCode();
                hash = (hash * 23) + HeaderBackgroundColor.GetHashCode();
                hash = (hash * 23) + HeaderBorderColor.GetHashCode();
                hash = (hash * 23) + HeaderFontName.GetHashCode();
                hash = (hash * 23) + HeaderFontSize.GetHashCode();
                hash = (hash * 23) + IsHeaderFontBold.GetHashCode();
                hash = (hash * 23) + RowFontColor.GetHashCode();
                hash = (hash * 23) + RowBackgroundColors.GetHashCode();
                hash = (hash * 23) + RowBorderColor.GetHashCode();
                hash = (hash * 23) + RowFontName.GetHashCode();
                hash = (hash * 23) + RowFontSize.GetHashCode();
                hash = (hash * 23) + IsRowFontBold.GetHashCode();
                hash = (hash * 23) + ShowGridLines.GetHashCode();
                hash = (hash * 23) + BorderAroundCells.GetHashCode();
                hash = (hash * 23) + IntegerNumberFormat.GetHashCode();
                hash = (hash * 23) + DecimalNumberFormat.GetHashCode();
                hash = (hash * 23) + DateFormat.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the specified object is of type <c>ExcelStyleOptions</c> and is equal to the
        /// current instance;  otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is ExcelStyleOptions other && Equals(other);
        }

        /// <summary>
        /// Determines whether the current <see cref="ExcelStyleOptions"/> instance is equal to another specified <see
        /// cref="ExcelStyleOptions"/> instance.
        /// </summary>
        /// <remarks>Two <see cref="ExcelStyleOptions"/> instances are considered equal if all their
        /// corresponding properties, including font colors, background colors, border colors, font names, font sizes,
        /// boldness, grid line visibility, cell border settings, and formatting options, are equal.</remarks>
        /// <param name="other">The <see cref="ExcelStyleOptions"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="ExcelStyleOptions"/> instance is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(ExcelStyleOptions other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return HeaderFontColor == other.HeaderFontColor &&
                   HeaderBackgroundColor == other.HeaderBackgroundColor &&
                   HeaderBorderColor == other.HeaderBorderColor &&
                   HeaderFontName == other.HeaderFontName &&
                   HeaderFontSize.Equals(other.HeaderFontSize) &&
                   IsHeaderFontBold == other.IsHeaderFontBold &&
                   RowFontColor == other.RowFontColor &&
                   RowBackgroundColors?.SequenceEqual(other.RowBackgroundColors) == true &&
                   RowBorderColor == other.RowBorderColor &&
                   RowFontName == other.RowFontName &&
                   RowFontSize.Equals(other.RowFontSize) &&
                   IsRowFontBold == other.IsRowFontBold &&
                   ShowGridLines == other.ShowGridLines &&
                   BorderAroundCells == other.BorderAroundCells &&
                   IntegerNumberFormat == other.IntegerNumberFormat &&
                   DecimalNumberFormat == other.DecimalNumberFormat &&
                   DateFormat == other.DateFormat;
        }

        /// <summary>
        /// Compares the current <see cref="ExcelStyleOptions"/> instance with another instance of the same type and
        /// returns an integer that indicates their relative order.
        /// </summary>
        /// <remarks>The comparison is performed by evaluating the properties of the <see
        /// cref="ExcelStyleOptions"/> instance in a predefined order. Null values are considered less than any non-null
        /// instance.</remarks>
        /// <param name="other">The <see cref="ExcelStyleOptions"/> instance to compare with the current instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared: <list type="bullet">
        /// <item><description>A positive value if the current instance is greater than <paramref
        /// name="other"/>.</description></item> <item><description>Zero if the current instance is equal to <paramref
        /// name="other"/>.</description></item> <item><description>A negative value if the current instance is less
        /// than <paramref name="other"/>.</description></item> </list></returns>
        public int CompareTo(ExcelStyleOptions other)
        {
            if (other is null) return 1; // null is considered less than any instance
            int result = string.CompareOrdinal(HeaderFontColor, other.HeaderFontColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(HeaderBackgroundColor, other.HeaderBackgroundColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(HeaderBorderColor, other.HeaderBorderColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(HeaderFontName, other.HeaderFontName);
            if (result != 0) return result;
            result = HeaderFontSize.CompareTo(other.HeaderFontSize);
            if (result != 0) return result;
            result = IsHeaderFontBold.CompareTo(other.IsHeaderFontBold);
            if (result != 0) return result;
            result = string.CompareOrdinal(RowFontColor, other.RowFontColor);
            if (result != 0) return result;
            result = RowBackgroundColors?.SequenceEqual(other.RowBackgroundColors) == true ? 0 : -1; // simplistic comparison
            if (result != 0) return result;
            result = string.CompareOrdinal(RowBorderColor, other.RowBorderColor);
            if (result != 0) return result;
            result = string.CompareOrdinal(RowFontName, other.RowFontName);
            if (result != 0) return result;
            result = RowFontSize.CompareTo(other.RowFontSize);
            if (result != 0) return result;
            result = IsRowFontBold.CompareTo(other.IsRowFontBold);
            if (result != 0) return result;
            result = ShowGridLines.CompareTo(other.ShowGridLines);
            if (result != 0) return result;
            result = BorderAroundCells.CompareTo(other.BorderAroundCells);
            if (result != 0) return result;
            result = string.CompareOrdinal(IntegerNumberFormat, other.IntegerNumberFormat);
            if (result != 0) return result;
            return string.CompareOrdinal(DecimalNumberFormat, other.DecimalNumberFormat);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelStyleOptions"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether two <see cref="ExcelStyleOptions"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="ExcelStyleOptions"/> instances are not equal;  otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator !=(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelStyleOptions"/> instance is less than another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator <(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelStyleOptions"/> instance is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator >(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelStyleOptions"/> instance is less than or equal to another.
        /// </summary>
        /// <remarks>This operator uses the <see cref="CompareTo"/> method to perform the
        /// comparison.</remarks>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether one <see cref="ExcelStyleOptions"/> instance is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <param name="right">The second <see cref="ExcelStyleOptions"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(ExcelStyleOptions left, ExcelStyleOptions right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
