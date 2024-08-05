using HypeLab.DnsLookupClient.Data.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HypeLab.DnsLookupClient.Helpers.Const;
using HypeLab.DnsLookupClient.Data.Clients;
using System.Net.Sockets;
using HypeLab.DnsLookupClient.Helpers;
using HypeLab.DnsLookupClient.Data.Exceptions;

namespace HypeLab.DnsLookupClient.Data.Models
{
    internal class DnsRequest
    {
        private readonly string _domain;
        private readonly DnsQueryType _queryType;

        public DnsRequest(string domain, DnsQueryType queryType)
        {
            _domain = domain;
            _queryType = queryType;
        }

        public async Task<DnsResponse> ResolveAsync(HypeLabUdpClient udpClient)
        {
            udpClient.Connect(DnsLookupDefaults.DnsServer, DnsLookupDefaults.DnsServerPort);

            byte[] requestBytes = CreateDnsQueryPacket(_domain, _queryType);

            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, DnsLookupDefaults.DnsServerPort)

            // Log the request bytes
            //Console.WriteLine("Request Bytes: " + BitConverter.ToString(requestBytes))

            await udpClient.SendAsync(requestBytes, requestBytes.Length).ConfigureAwait(false);

            UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync().ConfigureAwait(false);

            // Log the response bytes
            //Console.WriteLine("Response Bytes: " + BitConverter.ToString(udpReceiveResult.Buffer))

            return ParseDnsResponse(udpReceiveResult.Buffer);
        }

        public DnsResponse Resolve(HypeLabUdpClient udpClient)
        {
            udpClient.Connect(DnsLookupDefaults.DnsServer, DnsLookupDefaults.DnsServerPort);

            byte[] requestBytes = CreateDnsQueryPacket(_domain, _queryType);
            udpClient.Send(requestBytes, requestBytes.Length);

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, DnsLookupDefaults.DnsServerPort);
            byte[] responseBytes = udpClient.Receive(ref endpoint);

            return ParseDnsResponse(responseBytes);
        }

        public static byte[] GetBytesBigEndian(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        private byte[] CreateDnsQueryPacket(string domain, DnsQueryType queryType)
        {
            List<byte> packet = new List<byte>();

            // Transaction ID
            ushort transactionId = (ushort)new Random().Next(ushort.MaxValue);
            packet.AddRange(GetBytesBigEndian(transactionId));
            //packet.AddRange(BitConverter.GetBytes(transactionId))

            // Flags
            const ushort flags = 0x0100; // Standard query
            packet.AddRange(GetBytesBigEndian(flags));
            ////packet.AddRange(BitConverter.GetBytes(flags))

            // Questions
            const ushort questions = 1;
            //packet.AddRange(BitConverter.GetBytes(questions))
            packet.AddRange(GetBytesBigEndian(questions));

            // Answer RRs, Authority RRs, Additional RRs
            const ushort answerRRs = 0;
            const ushort authorityRRs = 0;
            const ushort additionalRRs = 0;
            //packet.AddRange(BitConverter.GetBytes(answerRRs))
            packet.AddRange(GetBytesBigEndian(answerRRs));
            //packet.AddRange(BitConverter.GetBytes(authorityRRs))
            //packet.AddRange(BitConverter.GetBytes(additionalRRs))
            packet.AddRange(GetBytesBigEndian(authorityRRs));
            packet.AddRange(GetBytesBigEndian(additionalRRs));

            // Query
            foreach (string part in domain.Split('.'))
            {
                packet.Add((byte)part.Length);
                packet.AddRange(Encoding.UTF8.GetBytes(part));
            }

            packet.Add(0); // End of domain name

            // Query Type
            ushort queryTypeValue = (ushort)queryType;
            //packet.AddRange(BitConverter.GetBytes(queryTypeValue))
            packet.AddRange(GetBytesBigEndian(queryTypeValue));

            // Query Class
            const ushort queryClass = 1; // IN (Internet)
            //packet.AddRange(BitConverter.GetBytes(queryClass))
            packet.AddRange(GetBytesBigEndian(queryClass));

            return packet.ToArray();
        }

        private string ParseDomainName(byte[] responseBytes, ref int offset)
        {
            StringBuilder domainName = new StringBuilder();
            int length = responseBytes[offset];
            while (length != 0)
            {
                if (offset >= responseBytes.Length)
                    throw new OffsettHigherThanResponseBytesException("Offset is outside the bounds of the array.");

                if (length >= 192) // Check for pointer
                {
                    if (offset + 1 >= responseBytes.Length)
                        throw new OffsettHigherThanResponseBytesException("Pointer offset is outside the bounds of the array.");

                    int pointerOffset = ((length - 192) << 8) + responseBytes[offset + 1];
                    offset += 2;
                    int savedOffset = offset; // Save the current offset
                    offset = pointerOffset; // Follow the pointer
                    domainName.Append(ParseDomainName(responseBytes, ref offset));
                    offset = savedOffset; // Restore the original offset
                    return domainName.ToString();
                }
                else
                {
                    offset++;
                    if (offset + length > responseBytes.Length)
                        throw new OffsettHigherThanResponseBytesException("Length is outside the bounds of the array.");

                    domainName.Append(Encoding.ASCII.GetString(responseBytes, offset, length));
                    offset += length;
                    if (offset < responseBytes.Length)
                        length = responseBytes[offset];
                    else
                        throw new OffsettHigherThanResponseBytesException("Offset is outside the bounds of the array.");

                    if (length != 0)
                        domainName.Append('.');
                }
            }
            offset++;
            return domainName.ToString();
        }

        private DnsResponse ParseDnsResponse(byte[] responseBytes)
        {
            List<DnsRecord> answers = new List<DnsRecord>();

            // Skip the header and question sections
            int offset = 12;
            ushort questionCount = BitConverter.ToUInt16(responseBytes, 4).ConvertEndianness();
            for (int i = 0; i < questionCount; i++)
            {
                while (responseBytes[offset] != 0)
                {
                    offset++;
                }
                offset += 5; // Skip null byte and QTYPE, QCLASS
            }

            // Read the answer section
            ushort answerCount = BitConverter.ToUInt16(responseBytes, 6).ConvertEndianness();
            for (int i = 0; i < answerCount; i++)
            {
#pragma warning disable S1481 // Unused local variables should be removed - necessary for updating the offset correctly
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                string name = ParseDomainName(responseBytes, ref offset);
                ushort type = BitConverter.ToUInt16(responseBytes, offset).ConvertEndianness();
                offset += 2;
                ushort classCode = BitConverter.ToUInt16(responseBytes, offset).ConvertEndianness();
                offset += 2;
                uint ttl = BitConverter.ToUInt32(responseBytes, offset).ConvertEndianness();
                offset += 4;
                ushort dataLength = BitConverter.ToUInt16(responseBytes, offset).ConvertEndianness();
                offset += 2;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
#pragma warning restore S1481 // Unused local variables should be removed

                if (type == (ushort)DnsQueryType.MX)
                {
                    ushort preference = BitConverter.ToUInt16(responseBytes, offset).ConvertEndianness();
                    offset += 2;
                    string exchange = ParseDomainName(responseBytes, ref offset);
                    answers.Add(new MxRecord(exchange, preference));
                }
                else
                {
                    offset += dataLength;
                }
            }

            return new DnsResponse(answers);
        }
    }
}
