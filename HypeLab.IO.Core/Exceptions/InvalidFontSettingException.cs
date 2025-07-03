namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when invalid font settings are provided.
    /// </summary>
    /// <remarks>This exception is typically thrown when one or more font-related parameters, such as font
    /// name, font color,  or font size, are invalid or do not meet the expected criteria. It provides detailed
    /// information about the  invalid settings to help diagnose the issue.</remarks>
    public class InvalidFontSettingException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that one or more font-related settings
        /// are invalid or incompatible with the expected configuration.</remarks>
        public InvalidFontSettingException()
            : base("Invalid font settings provided.") { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid font setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidFontSettingException(string message)
            : base(message) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid font setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidFontSettingException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        /// <param name="fontName">The name of the font that caused the exception.</param>
        /// <param name="fontColor">The color of the font that caused the exception.</param>
        /// <param name="fontSize">The size of the font that caused the exception.</param>
        public InvalidFontSettingException(string fontName, string fontColor, double fontSize)
            : base($"Invalid font settings: FontName='{fontName}', FontColor='{fontColor}', FontSize={fontSize}.") { }

        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        /// <param name="fontName">The name of the font that caused the exception. Cannot be null or empty.</param>
        /// <param name="fontColor">The color of the font that caused the exception. Cannot be null or empty.</param>
        /// <param name="fontSize">The size of the font that caused the exception. Must be a positive value.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidFontSettingException(string fontName, string fontColor, double fontSize, Exception innerException)
            : base($"Invalid font settings: FontName='{fontName}', FontColor='{fontColor}', FontSize={fontSize}.", innerException) { }
    }
}
