using HypeLab.DnsLookupClient.Helpers.Const;
using System;
using System.Diagnostics;

namespace HypeLab.DnsLookupClient.Data.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.OffsettHigherThanRequestBytes.DebuggerDisplay)]
    public class OffsettHigherThanResponseBytesException : Exception
    {
        public OffsettHigherThanResponseBytesException()
            : base(ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        public OffsettHigherThanResponseBytesException(string? message)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage) { }

        public OffsettHigherThanResponseBytesException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.OffsettHigherThanRequestBytes.DefaultMessage, innerException) { }
    }
}
