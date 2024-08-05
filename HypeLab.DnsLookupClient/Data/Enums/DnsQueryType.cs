namespace HypeLab.DnsLookupClient.Data.Enums
{
    public enum DnsQueryType : ushort
    {
        A = 1,      // Host address
        NS = 2,     // Authoritative name server
        CNAME = 5,  // Canonical name for an alias
        MX = 15     // Mail exchange
    }
}
