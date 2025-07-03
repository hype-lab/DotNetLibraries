using System.Globalization;

namespace HypeLab.IO.Core.Data.Options
{
    /// <summary>
    /// Defines options for configuring a writer, including culture-specific settings.
    /// </summary>
    /// <remarks>This interface allows customization of writer behavior, such as specifying the culture to be
    /// used for formatting and parsing operations. Implementations of this interface should ensure that the specified
    /// options are respected during writing operations.</remarks>
    public interface IWriterOptions
    {
        /// <summary>
        /// Gets or sets the culture information used for formatting and parsing operations.
        /// </summary>
        /// <remarks>This property determines the culture-specific formatting and parsing behavior, such
        /// as date, time,  number, and currency formats. Setting this property to <see langword="null"/> may result in
        /// the use  of the default culture.</remarks>
        CultureInfo Culture { get; set; }
    }
}
