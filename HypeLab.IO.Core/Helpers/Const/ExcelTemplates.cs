using System.Text;
using System.Xml.Linq;

namespace HypeLab.IO.Core.Helpers.Const
{
    /// <summary>
    /// Provides a collection of predefined XML templates and helper methods for generating OpenXML-based Excel
    /// documents. This class includes static properties for common document parts and methods for creating dynamic
    /// content such as shared strings, worksheets, and cells.
    /// </summary>
    /// <remarks>The <see cref="ExcelTemplates"/> class is designed to simplify the creation of OpenXML-based
    /// Excel files by providing reusable XML templates and utility methods. It includes predefined templates for
    /// content types, relationships, workbook structure, styles, and more. Additionally, it provides methods to
    /// dynamically generate XML elements for shared strings, worksheets, and individual cells.  This class is
    /// particularly useful for scenarios where lightweight, programmatic generation of Excel files is required without
    /// relying on external libraries or tools.</remarks>
    public static class ExcelTemplates
    {
        /// <summary>
        /// Gets an <see cref="XDocument"/> representing the default and override content types for an Open XML package,
        /// as defined by the Open Packaging Conventions (OPC).
        /// </summary>
        /// <remarks>This property provides a pre-defined set of content types commonly used in Open XML
        /// spreadsheet documents. It includes default mappings for file extensions such as <c>.rels</c> and
        /// <c>.xml</c>, as well as overrides for specific parts like the workbook, worksheets, shared strings, and
        /// styles.</remarks>
        public static XDocument ContentTypes => XDocument.Parse("""
            <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
              <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
              <Default Extension="xml" ContentType="application/xml"/>
              <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
              <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
              <Override PartName="/xl/sharedStrings.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml"/>
              <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
            </Types>
            """);

        /// <summary>
        /// Gets an <see cref="XDocument"/> representing the root relationships of an Open Packaging Convention (OPC)
        /// package.
        /// </summary>
        /// <remarks>This property provides a pre-defined XML structure for the root relationships of an
        /// OPC package,  which is commonly used in file formats such as OpenXML. The document includes a relationship
        /// to the  main workbook file in an Excel package.</remarks>
        public static XDocument RootRels => XDocument.Parse("""
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
            </Relationships>
            """);

        /// <summary>
        /// Gets an <see cref="XDocument"/> representing the default relationships for a workbook in the Open XML
        /// format.
        /// </summary>
        /// <remarks>The returned document defines relationships commonly used in an Open XML workbook,
        /// such as: <list type="bullet"> <item><description>A relationship to the primary
        /// worksheet.</description></item> <item><description>A relationship to the styles file.</description></item>
        /// <item><description>A relationship to the shared strings file.</description></item> </list> This property is
        /// useful for generating or manipulating Open XML workbooks programmatically.</remarks>
        public static XDocument WorkbookRels => XDocument.Parse("""
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
              <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
              <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings" Target="sharedStrings.xml"/>
            </Relationships>
            """);

        /// <summary>
        /// Gets an <see cref="XDocument"/> representing the default structure of an Excel workbook in the Open XML
        /// Spreadsheet format.
        /// </summary>
        /// <remarks>The returned workbook uses the Open XML Spreadsheet namespace 
        /// ("http://schemas.openxmlformats.org/spreadsheetml/2006/main") and includes a single sheet with the name
        /// "Sheet1", a sheet ID of 1, and a relationship ID of "rId1".</remarks>
        public static XDocument Workbook => XDocument.Parse("""
            <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"
                      xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
              <sheets>
                <sheet name="Sheet1" sheetId="1" r:id="rId1"/>
              </sheets>
            </workbook>
            """);

        /// <summary>
        /// Gets an <see cref="XDocument"/> representing the default styles for an OpenXML spreadsheet.
        /// </summary>
        /// <remarks>The returned <see cref="XDocument"/> contains a predefined XML structure that defines
        /// default styles,  including fonts, fills, borders, and cell formats, adhering to the OpenXML SpreadsheetML
        /// schema. This can be used as a base for creating or modifying styles in an OpenXML spreadsheet.</remarks>
        public static XDocument Styles => XDocument.Parse("""
            <styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
              <fonts count="1"><font><sz val="11"/><color theme="1"/><name val="Calibri"/></font></fonts>
              <fills count="1"><fill><patternFill patternType="none"/></fill></fills>
              <borders count="1"><border/></borders>
              <cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
              <cellXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/></cellXfs>
            </styleSheet>
            """);

        /// <summary>
        /// Creates an XML document representing the shared strings table for an OpenXML spreadsheet.
        /// </summary>
        /// <remarks>The resulting XML document conforms to the OpenXML SpreadsheetML specification for
        /// shared strings. The <c>count</c> and <c>uniqueCount</c> attributes of the <c>sst</c> element are set to the
        /// total number of strings provided.</remarks>
        /// <param name="strings">A list of strings to include in the shared strings table. Each string will be added as a unique entry.</param>
        /// <returns>An <see cref="XDocument"/> containing the shared strings table, where each string is represented as an
        /// <c>si</c> element within the <c>sst</c> root element.</returns>
        public static XDocument SharedStrings(List<string> strings)
        {
            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            return new XDocument(
                new XElement(ns + "sst",
                    new XAttribute("count", strings.Count),
                    new XAttribute("uniqueCount", strings.Count),
                    strings.Select(s => new XElement(ns + "si", new XElement(ns + "t", s)))
                )
            );
        }

        /// <summary>
        /// Creates an XML document representing a worksheet in the OpenXML SpreadsheetML format.
        /// </summary>
        /// <remarks>The returned document uses the namespace
        /// "http://schemas.openxmlformats.org/spreadsheetml/2006/main"  to conform to the OpenXML SpreadsheetML
        /// specification.</remarks>
        /// <param name="sheetData">The XML element containing the data to include in the worksheet.</param>
        /// <returns>An <see cref="XDocument"/> representing the worksheet, with the specified data wrapped in the appropriate
        /// namespace.</returns>
        public static XDocument Sheet(XElement sheetData)
        {
            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            return new XDocument(
                new XElement(ns + "worksheet", sheetData)
            );
        }

        /// <summary>
        /// Creates an XML representation of a cell in an OpenXML spreadsheet.
        /// </summary>
        /// <remarks>The cell is created with a reference in the format of column letter followed by row
        /// number (e.g., "A1", "B2") and is assigned a type of "s" (shared string). The value of the cell is determined
        /// by the <paramref name="sharedStringIndex"/> parameter, which corresponds to an entry in the shared string
        /// table.</remarks>
        /// <param name="colIndex">The zero-based column index of the cell.</param>
        /// <param name="rowIndex">The one-based row index of the cell.</param>
        /// <param name="sharedStringIndex">The index of the shared string to be used as the cell's value.</param>
        /// <returns>An <see cref="XElement"/> representing the cell, including its reference, type, and value.</returns>
        public static XElement Cell(int colIndex, int rowIndex, int sharedStringIndex)
        {
            string colLetter = GetColumnLetter(colIndex);
            string cellRef = $"{colLetter}{rowIndex}";
            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

            return new XElement(ns + "c",
                new XAttribute("r", cellRef),
                new XAttribute("t", "s"),
                new XElement(ns + "v", sharedStringIndex.ToString())
            );
        }

        private static string GetColumnLetter(int index)
        {
            StringBuilder colBuilder = new();
            while (index >= 0)
            {
                colBuilder.Insert(0, (char)('A' + (index % 26))); // 26 is the number of letters in the alphabet
                index = (index / 26) - 1;
            }

            return colBuilder.ToString();
        }
    }
}
