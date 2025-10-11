using System.Diagnostics;
using System.Runtime.Serialization;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specified column is not found in the data source.
    /// </summary>
    [DebuggerDisplay("ColumnNotFoundException: {Message}")]
    [Serializable]
    public class ColumnNotFoundException : Exception
    {
        private const string _defaultMessage = "The specified column was not found in the data source.";
        private const string _defaultMessageWithReplaces = "The column '{0}' was not found in the data source.";

        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        public ColumnNotFoundException()
            : base(_defaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        /// <param name="columnName">The name of the column that could not be found.</param>
        public ColumnNotFoundException(string columnName)
            : base(string.Format(_defaultMessageWithReplaces, columnName)) { }

        /// <summary>
        /// Represents an exception that is thrown when a specified column is not found in the data source.
        /// </summary>
        /// <param name="columnName">The name of the column that was not found.</param>
        /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is
        /// specified.</param>
        public ColumnNotFoundException(string columnName, Exception? innerException)
            : base(string.Format(_defaultMessageWithReplaces, columnName), innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnNotFoundException"/> class with serialized data.
        /// </summary>
        /// <remarks>This constructor is used during deserialization to reconstitute the exception object
        /// transmitted over a stream.</remarks>
        /// <param name="info">The <see cref="SerializationInfo"/> object that holds the serialized object data about the exception being
        /// thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> object that contains contextual information about the source or
        /// destination.</param>
        protected ColumnNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
