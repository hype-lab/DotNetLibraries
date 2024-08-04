using HypeLab.DnsLookupClient;
using HypeLab.DnsLookupClient.Data.Enums;
using HypeLab.DnsLookupClient.Data.Interfaces;
using HypeLab.DnsLookupClient.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Helpers
{
    internal static class DnsHelper
    {
        public static async Task<List<string>> GetMXRecordsAsync(this ILookupClient lookupClient, string domain)
        {
            List<string> mxRecords = new List<string>();
            DnsQueryResult result = await lookupClient.QueryAsync(domain, DnsQueryType.MX).ConfigureAwait(false);

            foreach (MxRecord record in result.Answers)
            {
                mxRecords.Add(record.Exchange);
            }

            return mxRecords;
        }

        public static List<string> GetMXRecords(this ILookupClient lookupClient, string domain)
        {
            List<string> mxRecords = new List<string>();
            DnsQueryResult result = lookupClient.Query(domain, DnsQueryType.MX);

            foreach (MxRecord record in result.Answers)
            {
                mxRecords.Add(record.Exchange);
            }

            return mxRecords;
        }
    }
}
