using HypeLab.DnsLookupClient.Helpers.Const;
using System;
using System.Diagnostics;

namespace HypeLab.DnsLookupClient.Data.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.OffsettHigherThanRequestBytes.DebuggerDisplay)]
    public class OffsetHigherThanResponseBytesException : Exception
    {
        public OffsetHigherThanResponseBytesException()
            : base(ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        public OffsetHigherThanResponseBytesException(string? message)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        public OffsetHigherThanResponseBytesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage, innerException) { }
    }
}
