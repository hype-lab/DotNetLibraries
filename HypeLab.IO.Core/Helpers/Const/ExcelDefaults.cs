using HypeLab.IO.Core.Data.Models.Excel;
using System.Xml.Linq;

namespace HypeLab.IO.Core.Helpers.Const
{
    /// <summary>
    /// Provides default values, constants, and namespaces used for working with XML files in the Open XML Spreadsheet
    /// format.
    /// </summary>
    /// <remarks>This class contains predefined constants for file paths, local element names, and namespace
    /// URLs commonly used in  Open XML Spreadsheet documents. It also provides access to namespace objects for easier
    /// manipulation of XML data.</remarks>
    public static class ExcelDefaults
    {
        /// <summary>
        /// Represents the file name and path for the shared strings file in an Excel workbook.
        /// </summary>
        /// <remarks>The shared strings file is typically used in Excel files to store string values that
        /// are shared across the workbook. This constant provides the relative path to the shared strings file within
        /// the structure of an Excel file.</remarks>
        public const string SharedStringsFileName = "xl/sharedStrings.xml";

        /// <summary>
        /// Provides a collection of constant string values representing local names used in XML or other structured
        /// data.
        /// </summary>
        /// <remarks>This class defines commonly used local names for elements or attributes, such as
        /// "row", "cell", and "text". These constants are intended to simplify working with structured data formats by
        /// providing predefined names.</remarks>
        public static class LocalNames
        {
            /// <summary>
            /// Represents the string value "row", typically used to identify or refer to a row in a data structure or
            /// layout.
            /// </summary>
            public const string Row = "row";

            /// <summary>
            /// Represents the identifier for a shared item.
            /// </summary>
            public const string Si = "si";

            /// <summary>
            /// Represents the string value "c", typically used to denote a cell.
            /// </summary>
            public const string C = "c";

            /// <summary>
            /// Represents the identifier for rich text formatting.
            /// </summary>
            /// <remarks>This constant can be used to specify or identify rich text formatting in
            /// contexts where such formatting is supported.</remarks>
            public const string R = "r";

            /// <summary>
            /// Represents a shared string constant.
            /// </summary>
            /// <remarks>This constant can be used as a predefined value for scenarios requiring a
            /// shared string.</remarks>
            public const string S = "s";

            /// <summary>
            /// Represents the constant string value "t".
            /// </summary>
            /// <remarks>This constant can be used as a predefined value for text-related operations
            /// or identifiers.</remarks>
            public const string T = "t";

            /// <summary>
            /// Represents a constant string value "v".
            /// </summary>
            public const string V = "v";
        }

        /// <summary>
        /// Provides constants and properties related to XML namespaces used in Open XML documents.
        /// </summary>
        /// <remarks>This class contains predefined namespace URLs and properties for working with Open
        /// XML formats, such as SpreadsheetML and package relationships.</remarks>
        public static class Namespaces
        {
            /// <summary>
            /// Provides constants for commonly used URLs in the Open XML file formats.
            /// </summary>
            /// <remarks>These URLs represent namespaces used in Open XML documents, such as
            /// SpreadsheetML and package relationships. They are typically used in XML processing and validation when
            /// working with Open XML files.</remarks>
            public static class Urls
            {
                /// <summary>
                /// Represents the XML namespace URI for SpreadsheetML documents.
                /// </summary>
                /// <remarks>SpreadsheetML is the XML-based format used for representing
                /// spreadsheet data in OpenXML. This constant can be used to identify or work with
                /// SpreadsheetML-specific elements and attributes within OpenXML documents.</remarks>
                public const string SpreadsheetMl = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

                /// <summary>
                /// Represents the namespace URI for Open XML package relationships.
                /// </summary>
                /// <remarks>This constant defines the standard namespace used for relationships
                /// in Open XML packages. It is typically used to identify and work with relationships within Open XML
                /// documents.</remarks>
                public const string Relationships = "http://schemas.openxmlformats.org/package/2006/relationships";
            }

            /// <summary>
            /// Gets the XML namespace for SpreadsheetML, which is used to define elements and attributes  in Excel
            /// documents conforming to the SpreadsheetML schema.
            /// </summary>
            public static XNamespace SpreadsheetML => Urls.SpreadsheetMl;
        }

        /// <summary>
        /// Provides constants related to worksheet file paths and names in an Excel document.
        /// </summary>
        /// <remarks>This class contains predefined constants for referencing specific worksheets and
        /// their default names. It is intended for use in scenarios where consistent access to worksheet paths and
        /// names is required.</remarks>
        public static class Worksheets
        {
            /// <summary>
            /// Represents the file path to the XML document for the first worksheet in an Excel workbook.
            /// </summary>
            public const string Sheet1PathName = "xl/worksheets/sheet1.xml";

            /// <summary>
            /// The default name for the first sheet in a workbook.
            /// </summary>
            public const string Sheet1Name = "sheet1";

            /// <summary>
            /// Gets the sheet named "Sheet1".
            /// </summary>
            public static SheetName Sheet1 => new(Sheet1Name);
        }

        /// <summary>
        /// Provides predefined number and date format strings for use in applications,  such as formatting values for
        /// display or exporting to external systems.
        /// </summary>
        /// <remarks>The <see cref="NumberFormats"/> class contains nested static classes that group
        /// format strings  by type, including integers, decimals, and dates. These format strings are compatible with 
        /// Excel-style formatting and can be used in scenarios where consistent formatting is required.</remarks>
        public static class NumberFormats
        {
            /// <summary>
            /// Provides predefined Excel format strings for integer values.
            /// </summary>
            /// <remarks>This class contains constants representing commonly used Excel number formats
            /// for integers,  including formats with thousand separators and formats for negative numbers. These format
            /// strings can be used to apply consistent formatting to integer values in Excel documents.</remarks>
            public static class Integers
            {
                /// <summary>
                /// Represents the default Excel number format for integers.
                /// </summary>
                /// <remarks>This constant is typically used to specify the format string for
                /// integer values in Excel-compatible applications or libraries. The format string corresponds to
                /// Excel's default integer formatting.</remarks>
                public const string Default = "#0"; // Excel format for integers
                /// <summary>
                /// Represents the Excel number format string for integers with a thousand separator.
                /// </summary>
                /// <remarks>This format string can be used to display integer values with commas
                /// as thousand separators. For example, the value 1234567 will be displayed as "1,234,567".</remarks>
                public const string WithThousandSeparator = "#,##0"; // Excel format for integers with thousand separator
                /// <summary>
                /// Represents the Excel number format string for negative integers with a thousand separator.
                /// </summary>
                /// <remarks>This format string displays negative integers with a comma as the
                /// thousand separator and a leading negative sign. For example, a value of -12345 would be formatted as
                /// "-12,345".</remarks>
                public const string NegativeWithThosandSeparator = "#,##0;-#,##0"; // Excel format for negative integers with thousand separator
                /// <summary>
                /// Represents an Excel number format string for negative integers with a thousand separator.
                /// </summary>
                /// <remarks>This format displays negative integers in red with a thousand
                /// separator.  For example, a value of -12345 would be formatted as "[Red]-12,345".</remarks>
                public const string RedNegativeWithThosandSeparator = "#,##0;[Red]-#,##0"; // Excel format for negative integers with thousand separator
            }

            /// <summary>
            /// Provides predefined Excel-compatible format strings for decimal numbers.
            /// </summary>
            /// <remarks>This class contains constant string values representing various numeric
            /// formats commonly used in Excel. These formats can be used to format decimal numbers for display or
            /// export purposes.</remarks>
            public static class Decimals
            {
                /// <summary>
                /// Represents the default Excel number format for decimal numbers.
                /// </summary>
                /// <remarks>The format string uses the standard Excel format syntax, where
                /// numbers are displayed with a thousands separator and two decimal places.</remarks>
                public const string Default = "#,##0.00"; // Excel format for decimal numbers
                /// <summary>
                /// Represents the Excel number format string for decimal numbers with a thousand separator.
                /// </summary>
                /// <remarks>This format string can be used to display numbers with commas as
                /// thousand separators and two decimal places. For example, the number 1234567.89 will be formatted as
                /// "1,234,567.89".</remarks>
                public const string WithThousandSeparator = "#,##0.00"; // Excel format for decimal numbers with thousand separator
                /// <summary>
                /// Represents the Excel number format string for decimal numbers,  displaying up to two decimal places
                /// only if needed.
                /// </summary>
                /// <remarks>This format string can be used in Excel to display numbers with up to
                /// two decimal places.  Trailing zeros are omitted for whole numbers or numbers with fewer than two
                /// decimal places.</remarks>
                public const string IfNeeded = "#0.##"; // Excel format for decimal numbers, showing up to two decimal places if needed
                /// <summary>
                /// Represents an Excel number format string for displaying decimal numbers with up to two decimal
                /// places, including a negative sign for negative values.
                /// </summary>
                /// <remarks>This format string ensures that positive numbers are displayed
                /// without a sign, while negative numbers are prefixed with a negative sign. Trailing zeros are omitted
                /// unless required to display up to two decimal places.</remarks>
                public const string NegativeIfNeeded = "#0.##;-#0.##"; // Excel format for negative decimal numbers, showing up to two decimal places if needed
            }

            /// <summary>
            /// Provides commonly used date format strings for various regional and standard formats.
            /// </summary>
            /// <remarks>This class contains constant string representations of date formats,
            /// including ISO, European, and American styles. These formats can be used with date and time parsing or
            /// formatting methods, such as <see cref="DateTime.ToString(string)"/>.</remarks>
            public static class Dates
            {
                /// <summary>
                /// Represents the ISO 8601 date format string ("yyyy-MM-dd").
                /// </summary>
                /// <remarks>This format is commonly used for representing dates in a
                /// standardized, unambiguous way.</remarks>
                public const string Iso = "yyyy-MM-dd"; // ISO format for dates
                /// <summary>
                /// Represents the ISO 8601 date and time format string with hours, minutes, and seconds.
                /// </summary>
                /// <remarks>This format string can be used to format or parse date and time
                /// values in the "yyyy-MM-dd hh:mm:ss" pattern. Note that the "hh" specifier represents a 12-hour
                /// clock. For a 24-hour clock, use "HH" instead.</remarks>
                public const string IsOWithTime = "yyyy-MM-dd hh:mm:ss"; // ISO format with time for dates
                /// <summary>
                /// Represents the European date format string.
                /// </summary>
                /// <remarks>The format string follows the "day/month/year" pattern, commonly used
                /// in European countries.</remarks>
                public const string European = "dd/MM/yyyy"; // European format for dates
                /// <summary>
                /// Represents the American date format string.
                /// </summary>
                /// <remarks>The format follows the pattern "MM/dd/yyyy", where: <list
                /// type="bullet"> <item><description><c>MM</c> represents the two-digit month.</description></item>
                /// <item><description><c>dd</c> represents the two-digit day.</description></item>
                /// <item><description><c>yyyy</c> represents the four-digit year.</description></item> </list> This
                /// constant can be used for formatting or parsing dates in the American style.</remarks>
                public const string American = "MM/dd/yyyy"; // American format for dates
            }
        }
    }
}
