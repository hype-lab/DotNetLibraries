using System;

namespace HypeLab.DnsLookupClient.Helpers
{
    internal static class DnsRequestHelper
    {
        internal static ushort ConvertEndianness(this ushort value)
        {
            return (ushort)((value << 8) | (value >> 8));
        }

        internal static uint ConvertEndianness(this uint value)
        {
            return (value >> 24) |
                   ((value << 8) & 0x00FF0000) |
                   ((value >> 8) & 0x0000FF00) |
                   (value << 24);
        }

        internal static byte[] GetBytesBigEndian(this ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}
