using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when invalid color settings are provided.
    /// </summary>
    [DebuggerDisplay("InvalidColorSettingException: {Message}")]
    [Serializable]
    public class InvalidColorSettingException : Exception
    {
        private const string _defaultMessage = "Invalid color settings provided.";
        private const string _defaultMessageWithReplaces = "Invalid color settings: FillColor='{0}', FontColor='{1}'.";

        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        public InvalidColorSettingException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid color setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidColorSettingException(string? message)
            : base(message ?? _defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an invalid color setting is encountered.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidColorSettingException(string? message, Exception? innerException)
            : base(message ?? _defaultMessage, innerException) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        /// <param name="fillColor">The fill color that caused the exception.</param>
        /// <param name="fontColor">The font color that caused the exception.</param>
        public InvalidColorSettingException(string fillColor, string fontColor)
            : base(string.Format(_defaultMessageWithReplaces, fillColor, fontColor)) { }

        /// <summary>
        /// Represents an exception that is thrown when invalid color settings are provided.
        /// </summary>
        /// <param name="fillColor">The invalid fill color value that caused the exception.</param>
        /// <param name="fontColor">The invalid font color value that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public InvalidColorSettingException(string fillColor, string fontColor, Exception? innerException)
            : base(string.Format(_defaultMessageWithReplaces, fillColor, fontColor), innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidColorSettingException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstitute the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidColorSettingException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
