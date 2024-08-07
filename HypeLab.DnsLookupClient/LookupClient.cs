using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Exceptions;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient
{
    public class LookupClient : ILookupClient
    {
        /// <summary>
        /// Sends a DNS query to the specified domain and returns the result.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="queryType"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="DnsQueryException"></exception>
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
        /// Sends a DNS query to the specified domain and returns the result.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="queryType"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="DnsQueryException"></exception>
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
