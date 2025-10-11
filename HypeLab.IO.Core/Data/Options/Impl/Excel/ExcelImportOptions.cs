namespace HypeLab.IO.Core.Data.Options.Impl.Excel
{
    // classe globale che incapsula le varie tipologie di opzioni per l'importazione di dati da file Excel (lettura, parsing, writing in arrivo)
    /// <summary>
    /// Encapsulates options for importing data from Excel files, including reading, parsing, and error handling.
    /// </summary>
    public class ExcelImportOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelImportOptions"/> class with default settings for importing
        /// Excel files.
        /// </summary>
        public ExcelImportOptions() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelImportOptions"/> class with the specified reader and
        /// parser options.
        /// </summary>
        /// <param name="readerOptions">The options to use when reading Excel files. Cannot be null.</param>
        /// <param name="parserOptions">The options to use when parsing Excel data. Cannot be null.</param>
        public ExcelImportOptions(ExcelReaderOptions readerOptions, ExcelParserOptions parserOptions)
        {
            Reader = readerOptions;
            Parser = parserOptions;
        }

        /// <summary>
        /// Gets or sets the options used to configure the behavior of the Excel reader.
        /// </summary>
        public ExcelReaderOptions Reader { get; set; } = new();
        /// <summary>
        /// Gets or sets the options used to configure the behavior of the Excel parser.
        /// </summary>
        public ExcelParserOptions Parser { get; set; } = new();

        /// <summary>
        /// Gets the default set of options for importing Excel files.
        /// </summary>
        public static ExcelImportOptions Default
            => new();
    }
}
