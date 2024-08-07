namespace HypeLab.DnsLookupClient.Helpers.Const
{
    internal static class ExceptionDefaults
    {
        internal static class OffsettHigherThanRequestBytes
        {
            internal const string DebuggerDisplay = "OffsettHigherThanRequestBytesException: {Message}";
            internal const string DefaultMessage = "The offset is higher than the request bytes length.";
        }

        internal static class DnsQuery
        {
            internal const string DebuggerDisplay = "DnsQueryException: {Message}";
            internal const string DefaultMessage = "An error occurred while querying the DNS server.";
        }
    }
}
