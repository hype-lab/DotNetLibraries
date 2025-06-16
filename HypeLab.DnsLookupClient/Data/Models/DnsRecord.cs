namespace HypeLab.DnsLookupClient.Data.Models
{
    /// <summary>
    /// Represents a base class for DNS (Domain Name System) records.
    /// </summary>
    /// <remarks>This class serves as the foundation for specific types of DNS records, such as A, CNAME, or
    /// MX records. It is intended to be inherited by derived classes that define the behavior and properties of
    /// specific DNS record types.</remarks>
#pragma warning disable S2094 // Classes should not be empty
    public abstract class DnsRecord { }
#pragma warning restore S2094 // Classes should not be empty
}
