using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Models;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient.Data.Interfaces
{
    /// <summary>
    /// Represents a client for performing DNS queries.
    /// </summary>
    /// <remarks>This interface provides methods for querying DNS records asynchronously or synchronously. 
    /// Use the <see cref="QueryAsync"/> method for non-blocking operations or <see cref="Query"/>  for immediate
    /// results in scenarios where blocking is acceptable.</remarks>
    public interface ILookupClient
    {
        /// <summary>
        /// Asynchronously performs a DNS query for the specified domain and query type.
        /// </summary>
        /// <remarks>This method performs the query asynchronously and does not block the calling thread. 
        /// Ensure that the <paramref name="domain"/> parameter is a valid domain name to avoid errors.</remarks>
        /// <param name="domain">The domain name to query. This must be a valid, non-null, and non-empty domain name.</param>
        /// <param name="queryType">The type of DNS query to perform, such as A, AAAA, or MX. This determines the type of DNS records to
        /// retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DnsQueryResult"/>
        /// object with the results of the DNS query.</returns>
        Task<DnsQueryResult> QueryAsync(string domain, DnsQueryType queryType);
        /// <summary>
        /// Performs a DNS query for the specified domain and query type.
        /// </summary>
        /// <remarks>The method performs a synchronous DNS query and returns the results based on the
        /// specified query type. Ensure that the domain name is valid and properly formatted before calling this
        /// method.</remarks>
        /// <param name="domain">The domain name to query. This cannot be null or empty.</param>
        /// <param name="queryType">The type of DNS record to query, such as A, AAAA, or MX.</param>
        /// <returns>A <see cref="DnsQueryResult"/> containing the results of the DNS query. If no records are found, the result
        /// may be empty.</returns>
        DnsQueryResult Query(string domain, DnsQueryType queryType);
    }
}
