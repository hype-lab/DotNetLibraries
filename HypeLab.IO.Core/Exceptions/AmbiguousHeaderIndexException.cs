using System;
using System.Collections.Generic;
using System.Text;

namespace HypeLab.IO.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when multiple columns in an Excel sheet are mapped to the same index,
    /// resulting in ambiguity during parsing.
    /// </summary>
    /// <remarks>This exception typically occurs when the header indices in the Excel sheet data are not
    /// unique,  causing conflicts during data processing. To avoid this error, ensure that each column in the Excel
    /// sheet  has a distinct index.</remarks>
    public class AmbiguousHeaderIndexException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmbiguousHeaderIndexException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AmbiguousHeaderIndexException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbiguousHeaderIndexException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AmbiguousHeaderIndexException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Represents an exception that is thrown when multiple columns in an Excel sheet are mapped to the same index,
        /// resulting in ambiguity during parsing.
        /// </summary>
        /// <remarks>This exception typically occurs when the header indices in the Excel sheet data are
        /// not unique,  causing conflicts during data processing. Ensure that each column in the Excel sheet has a
        /// distinct index  to avoid this error.</remarks>
        public AmbiguousHeaderIndexException()
            : base("An error occurred due to ambiguous header indices in the Excel sheet data. This usually happens when multiple columns are mapped to the same index, causing confusion during parsing.") { }
    }
}
