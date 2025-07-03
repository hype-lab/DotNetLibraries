namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when invalid color settings are provided.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that one or more color values, such as fill
    /// color or font color,  are invalid or incompatible. It provides constructors for specifying detailed error
    /// messages and optionally  includes the invalid color values or an inner exception for additional
    /// context.</remarks>
    public class InvalidColorSettingException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that a color-related configuration or
        /// input does not meet the expected requirements or constraints.</remarks>
        public InvalidColorSettingException()
            : base("Invalid color settings provided.") { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid color setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidColorSettingException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid color setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidColorSettingException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        /// <param name="fillColor">The fill color that caused the exception.</param>
        /// <param name="fontColor">The font color that caused the exception.</param>
        public InvalidColorSettingException(string fillColor, string fontColor)
            : base($"Invalid color settings: FillColor='{fillColor}', FontColor='{fontColor}'.") { }

        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        /// <param name="fillColor">The invalid fill color value that caused the exception.</param>
        /// <param name="fontColor">The invalid font color value that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidColorSettingException(string fillColor, string fontColor, Exception innerException)
            : base($"Invalid color settings: FillColor='{fillColor}', FontColor='{fontColor}'.", innerException) { }
    }
}
