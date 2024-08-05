namespace HypeLab.DnsLookupClient.Helpers.Const
{
    internal static class ExceptionDefaults
    {
        internal static class OffsettHigherThanRequestBytes
        {
            internal const string DebuggerDisplay = "OffsettHigherThanRequestBytesException: {Message}";
            internal const string DefaultMessage = "The offset is higher than the request bytes length.";
        }
    }
}
