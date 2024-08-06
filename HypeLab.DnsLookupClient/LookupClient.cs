using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HypeLab.DnsLookupClient
{
    public class LookupClient : ILookupClient
    {
        public async Task<DnsQueryResult> QueryAsync(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

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

        public DnsQueryResult Query(string domain, DnsQueryType queryType)
        {
            if (queryType != DnsQueryType.MX)
                throw new NotSupportedException("Only MX queries are supported in this custom implementation.");

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
    }
}
