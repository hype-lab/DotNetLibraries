using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Exceptions;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient
{
    /// <summary>
    /// Provides methods for performing DNS queries, including support for querying specific record types.
    /// </summary>
    /// <remarks>This implementation currently supports only MX (Mail Exchange) record queries.  Use the <see
    /// cref="QueryAsync"/> or <see cref="Query"/> methods to perform DNS lookups.</remarks>
    public class LookupClient : ILookupClient
    {
        /// <summary>
        /// Asynchronously performs a DNS query for the specified domain and query type.
        /// </summary>
        /// <remarks>This method only supports MX (Mail Exchange) record queries. If a different query
        /// type is specified,  a <see cref="NotSupportedException"/> will be thrown. The method processes the DNS
        /// response to extract  MX records and returns them in the result.</remarks>
        /// <param name="domain">The domain name to query. This cannot be null or empty.</param>
        /// <param name="queryType">The type of DNS query to perform. Only <see cref="DnsQueryType.MX"/> is supported in this implementation.</param>
        /// <returns>A <see cref="DnsQueryResult"/> containing the results of the DNS query. If no matching records are found, 
        /// the result will contain an empty collection.</returns>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="queryType"/> is not <see cref="DnsQueryType.MX"/>.</exception>
        /// <exception cref="DnsQueryException">Thrown if an error occurs while querying the DNS server.</exception>
        public async Task<DnsQueryResult> QueryAsync(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

            try
            {
                List<MxRecord> mxRecords = new List<MxRecord>();
                DnsRequest dnsRequest = new DnsRequest(domain, queryType);
                DnsResponse dnsResponse = await dnsRequest.ResolveAsync().ConfigureAwait(false);

                foreach (DnsRecord record in dnsResponse.Answers)
                {
                    if (record is MxRecord mxRecord)
                        mxRecords.Add(mxRecord);
                }

                return new DnsQueryResult(mxRecords);
            }
            catch (Exception ex)
            {
                throw new DnsQueryException("An error occurred while querying the DNS server.", ex);
            }
        }

        /// <summary>
        /// Performs a DNS query for the specified domain and query type.
        /// </summary>
        /// <remarks>This method is a custom implementation that only supports MX (Mail Exchange) record queries. If the
        /// query type is not <see cref="DnsQueryType.MX"/>, a <see cref="NotSupportedException"/> is thrown.</remarks>
        /// <param name="domain">The domain name to query. This cannot be null or empty.</param>
        /// <param name="queryType">The type of DNS query to perform. Only <see cref="DnsQueryType.MX"/> is supported.</param>
        /// <returns>A <see cref="DnsQueryResult"/> containing the results of the query, including any MX records found.</returns>
        /// <exception cref="NotSupportedException">Thrown if <paramref name="queryType"/> is not <see cref="DnsQueryType.MX"/>.</exception>
        /// <exception cref="DnsQueryException">Thrown if an error occurs while querying the DNS server.</exception>
        public DnsQueryResult Query(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

            try
            {
                List<MxRecord> mxRecords = new List<MxRecord>();
                DnsRequest dnsRequest = new DnsRequest(domain, queryType);
                DnsResponse dnsResponse = dnsRequest.Resolve();

                foreach (DnsRecord record in dnsResponse.Answers)
                {
                    if (record is MxRecord mxRecord)
                    {
                        mxRecords.Add(mxRecord);
                    }
                }

                return new DnsQueryResult(mxRecords);
            }
            catch (Exception ex)
            {
                throw new DnsQueryException("An error occurred while querying the DNS server.", ex);
            }
        }
    }
}
