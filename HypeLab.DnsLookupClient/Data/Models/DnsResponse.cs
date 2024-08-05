using System.Collections.Generic;

namespace HypeLab.DnsLookupClient.Data.Models
{
    internal class DnsResponse
    {
        public List<DnsRecord> Answers { get; }

        public DnsResponse(List<DnsRecord> answers)
        {
            Answers = answers;
        }
    }
}
