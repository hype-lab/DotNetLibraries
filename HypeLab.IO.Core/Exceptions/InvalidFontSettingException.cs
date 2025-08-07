using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when invalid font settings are provided.
    /// </summary>
    [DebuggerDisplay("InvalidFontSettingException: {Message}")]
    [Serializable]
    public class InvalidFontSettingException : Exception
    {
        private const string _defaultMessage = "Invalid font settings provided.";
        private const string _defaultMessageWithReplaces = "Invalid font settings: FontName='{0}', FontColor='{1}', FontSize={2}.";

        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        public InvalidFontSettingException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid font setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidFontSettingException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid font setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidFontSettingException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        /// <param name="fontName">The name of the font that caused the exception.</param>
        /// <param name="fontColor">The color of the font that caused the exception.</param>
        /// <param name="fontSize">The size of the font that caused the exception.</param>
        public InvalidFontSettingException(string fontName, string fontColor, double fontSize)
            : base(string.Format(_defaultMessageWithReplaces, fontName, fontColor, fontSize)) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid font settings are provided.
        /// </summary>
        /// <param name="fontName">The name of the font that caused the exception. Cannot be null or empty.</param>
        /// <param name="fontColor">The color of the font that caused the exception. Cannot be null or empty.</param>
        /// <param name="fontSize">The size of the font that caused the exception. Must be a positive value.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidFontSettingException(string fontName, string fontColor, double fontSize, Exception? innerException)
            : base(string.Format(_defaultMessageWithReplaces, fontName, fontColor, fontSize), innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFontSettingException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstitute the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data about the exception being
        /// thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected InvalidFontSettingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
