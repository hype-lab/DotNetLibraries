namespace HypeLab.DnsLookupClient.Data.Enums
{
    /// <summary>
    /// Specifies the type of DNS query to perform.
    /// </summary>
    /// <remarks>This enumeration defines the query types supported by DNS, such as resolving host addresses,
    /// authoritative name servers, canonical names, and mail exchange records. Use these values to indicate the desired
    /// query type when performing DNS lookups.</remarks>
    public enum DnsQueryType : ushort
    {
        /// <summary>
        /// Represents the value A with an associated integer value of 1.
        /// </summary>
        A = 1,      // Host address
        /// <summary>
        /// Represents a namespace token in the parsing context.
        /// </summary>
        NS = 2,     // Authoritative name server
        /// <summary>
        /// Represents the Canonical Name (CNAME) record type in the Domain Name System (DNS).
        /// </summary>
        /// <remarks>A CNAME record maps an alias name to a true or canonical domain name.  This is
        /// commonly used to redirect one domain name to another.</remarks>
        CNAME = 5,  // Canonical name for an alias
        /// <summary>
        /// Represents the Mail Exchange (MX) record type in the Domain Name System (DNS).
        /// </summary>
        MX = 15     // Mail exchange
    }
}
