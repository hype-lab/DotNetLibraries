namespace HypeLab.IO.Core.Data.Options
{
    /// <summary>
    /// Gets or sets a value indicating whether validation is enabled for the reader.
    /// </summary>
    public interface IReaderOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether validation is enabled.
        /// </summary>
        bool EnableValidation { get; set; }
    }
}
