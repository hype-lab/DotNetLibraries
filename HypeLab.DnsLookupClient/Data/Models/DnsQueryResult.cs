using System.Collections.Generic;

namespace HypeLab.DnsLookupClient.Data.Models
{
    /// <summary>
    /// Represents the result of a DNS query, containing a collection of DNS records.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate the results of a DNS query operation,  such as
    /// retrieving mail exchange (MX) records. The <see cref="Answers"/> property  provides access to the list of
    /// records returned by the query.</remarks>
    public class DnsQueryResult
    {
        /// <summary>
        /// Gets the list of DNS MX (Mail Exchange) records returned as a result of the query.
        /// </summary>
        public List<MxRecord> Answers { get; }
        /// <summary>
        /// Represents the result of a DNS query, containing a collection of MX (Mail Exchange) records.
        /// </summary>
        /// <param name="answers">A list of <see cref="MxRecord"/> objects representing the MX records returned by the DNS query. This
        /// parameter cannot be null.</param>
        public DnsQueryResult(List<MxRecord> answers)
        {
            Answers = answers;
        }
    }
}
