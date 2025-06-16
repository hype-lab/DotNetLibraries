using HypeLab.DnsLookupClient.Helpers.Const;
using System;
using System.Diagnostics;

namespace HypeLab.DnsLookupClient.Data.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during a DNS query operation.
    /// </summary>
    /// <remarks>This exception is typically thrown when a DNS query fails due to an error, such as an invalid
    /// response from the DNS server or a network-related issue. The exception message provides details about the
    /// specific error encountered.</remarks>
    [DebuggerDisplay(ExceptionDefaults.DnsQuery.DebuggerDisplay)]
    public class DnsQueryException : Exception
    {
        /// <summary>
        /// Represents an exception that is thrown when a DNS query operation fails.
        /// </summary>
        /// <remarks>This exception is typically thrown when a DNS query encounters an error, such as a
        /// timeout or an invalid response. The default message provides a general description of the failure.</remarks>
        public DnsQueryException()
            : base(ExceptionDefaults.DnsQuery.DefaultMessage) { }

        /// <summary>
        /// Represents an exception that occurs during a DNS query operation.
        /// </summary>
        /// <param name="message">The error message that describes the reason for the exception. If <paramref name="message"/> is <see
        /// langword="null"/>, a default error message is used.</param>
        public DnsQueryException(string? message)
            : base(message ?? ExceptionDefaults.DnsQuery.DefaultMessage) { }

        /// <summary>
        /// Represents an exception that occurs during a DNS query operation.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. If null, a default message is used.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public DnsQueryException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DnsQuery.DefaultMessage, innerException) { }
    }
}
