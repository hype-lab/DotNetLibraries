using HypeLab.IO.Core.Data.Options.Impl.Excel;

namespace HypeLab.IO.Core.Data.Models.Excel
{
    /// <summary>
    /// Defines a contract for reading rows from an Excel file sequentially.
    /// </summary>
    public interface IExcelRowReader : IDisposable
    {
        /// <summary>
        /// Advances the reader to the next record in the data source.
        /// </summary>
        bool ReadNext(); // avanzamento

        /// <summary>
        /// Gets the current row of data as an array of nullable strings.
        /// </summary>
        string?[] Current { get; } // riga attuale

        /// <summary>
        /// Gets the current zero-based index of the row.
        /// </summary>
        int RowIndex { get; } // indice attuale
    }
}
