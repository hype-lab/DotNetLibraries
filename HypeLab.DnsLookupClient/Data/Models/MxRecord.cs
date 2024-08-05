namespace HypeLab.DnsLookupClient.Data.Models
{
    public class MxRecord : DnsRecord
    {
        public string Exchange { get; }
        public ushort Preference { get; }

        public MxRecord(string exchange, ushort preference)
        {
            Exchange = exchange;
            Preference = preference;
        }

        public override string ToString()
        {
            return $"{Preference} {Exchange}";
        }
    }
}
