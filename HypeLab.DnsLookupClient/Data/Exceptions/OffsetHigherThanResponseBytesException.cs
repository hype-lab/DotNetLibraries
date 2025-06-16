using HypeLab.DnsLookupClient.Helpers.Const;
using System;
using System.Diagnostics;

namespace HypeLab.DnsLookupClient.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when the specified offset exceeds the number of bytes available in the
    /// response.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that an operation attempted to access a position
    /// in the response data that is beyond the available range of bytes. Ensure that the offset value is within the
    /// bounds of the response data before performing the operation.</remarks>
    [DebuggerDisplay(ExceptionDefaults.OffsettHigherThanRequestBytes.DebuggerDisplay)]
    public class OffsetHigherThanResponseBytesException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when an offset exceeds the total number of response bytes available.
        /// </summary>
        /// <remarks>This exception is typically used to indicate that an operation attempted to access a
        /// position in the response data that is beyond the available range of bytes.</remarks>
        public OffsetHigherThanResponseBytesException()
            : base(ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an offset exceeds the total number of response bytes available.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. If null, a default message is used.</param>
        public OffsetHigherThanResponseBytesException(string? message)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        /// <summary>
        /// Represents an exception that is thrown when an offset exceeds the total number of response bytes available.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. If null, a default message is used.</param>
        /// <param name="innerException">The exception that caused the current exception, or null if no inner exception is specified.</param>
        public OffsetHigherThanResponseBytesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage, innerException) { }
    }
}
