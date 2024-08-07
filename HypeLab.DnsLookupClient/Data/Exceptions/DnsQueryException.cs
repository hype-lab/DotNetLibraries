using HypeLab.DnsLookupClient.Helpers.Const;
using System;
using System.Diagnostics;

namespace HypeLab.DnsLookupClient.Data.Exceptions
{
    [DebuggerDisplay(ExceptionDefaults.DnsQuery.DebuggerDisplay)]
    public class DnsQueryException : Exception
    {
        public DnsQueryException()
            : base(ExceptionDefaults.DnsQuery.DefaultMessage) { }

        public DnsQueryException(string? message)
            : base(message ?? ExceptionDefaults.DnsQuery.DefaultMessage) { }

        public DnsQueryException(string? message, Exception innerException)
            : base(message ?? ExceptionDefaults.DnsQuery.DefaultMessage, innerException) { }
    }
}
