using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace packetSniffer
{
    public class PacketReader
    {
        // Protocol Type
        private Protocol PacketProtocolType { get; set; }
        // Raw Packet Data 
        public byte[] rawPacket { get; set; }
        // Source IP
        private IPAddress source_IP;
        // Dest IP
        private IPAddress destination_IP;

        private int srcPort;

        private int dstPort;

        private int headLenght;

        private int totalLenght;

        private byte[] rawByteConent;

        public void rawByteConentSet()
        {
            rawByteConent = new byte[rawPacket.Length - headLenght];
            Array.Copy(rawPacket, headLenght, rawByteConent, 0, rawByteConent.Length);
        }
           
        public PacketReader(byte[] packetData)
        {
            rawPacket = packetData;
            headLenght = (rawPacket[0] & 0x0F) * 4;
            totalLenght = rawPacket[2] * 256 + rawPacket[3];

            if (headLenght < 5)
                throw new ArgumentException("Header is wrong for the length is incorrect"); // header is wrong for the length is incorrect;

            ///the actual length is the same with the header length indicator?
            if (totalLenght != rawPacket.Length)
                throw new ArgumentException("Length is incorrect"); // length is incorrect;

            if (totalLenght < packetData.Length)
                throw new ArgumentException("Incorect packet lenght");

            source_IP = new IPAddress(BitConverter.ToUInt32(rawPacket,12));

            destination_IP = new IPAddress(BitConverter.ToUInt32(rawPacket,16));

            try
            {
                GetPacketProtocol();
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid packet protocol");
            }
        }

        private void GetPacketProtocol()
        {
            // Checking that enum has proper packet type if not enum = other

            if (Enum.IsDefined(typeof(Protocol), (Protocol)rawPacket[9]))
                PacketProtocolType = (Protocol)rawPacket[9];
            else
                PacketProtocolType = Protocol.OTHERS;

            // Two other situation for different packet types

            if (PacketProtocolType == Protocol.TCP || PacketProtocolType == Protocol.UDP)
            {

                srcPort = rawPacket[headLenght] * 256 + rawPacket[headLenght + 1];
                dstPort = rawPacket[headLenght + 2] * 256 + rawPacket[headLenght + 3];

                if (PacketProtocolType == Protocol.TCP)
                    headLenght += 20;
                if (PacketProtocolType == Protocol.UDP)
                    headLenght += 8;
            }
            else
            {
                srcPort = -1;
                dstPort = -1;
            }
        }

        // Return objects -- add case if nedded

        public Packet GetProcessedPacket()
        {
            switch(PacketProtocolType)
            {
                case Protocol.TCP:
                    return new Tcp(rawPacket, PacketProtocolType, source_IP, destination_IP, srcPort, dstPort, totalLenght);
                case Protocol.UDP:
                    return new Udp(rawPacket, PacketProtocolType, source_IP, destination_IP, srcPort, dstPort, totalLenght);
                case Protocol.ICMP:
                    return new Icmp(rawPacket, PacketProtocolType, source_IP, destination_IP, totalLenght);
                case Protocol.IGMP:
                    return new Igmp(rawPacket, PacketProtocolType, source_IP, destination_IP, totalLenght);
                default:
                    return new Icmp(rawPacket, PacketProtocolType, source_IP, destination_IP, totalLenght);
            }
        }

        // Some string conversion is here

        public static string getHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = (bytes[0] & 0x0F) * 4; i < bytes.Length; i++)
            {
                if (bytes[i] >= 0 && bytes[i] <= 15)
                    sb.Append("0" + bytes[i].ToString("X") + " ");
                else
                    sb.Append(bytes[i].ToString("X") + " ");
            }
            return sb.ToString();
        }

        public static string getEncodedString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

                for (int i = (bytes[0] & 0x0F) * 4; i < bytes.Length; i++)
                {
                    if (bytes[i] > 31 && bytes[i] < 128)
                        sb.Append((char)bytes[i]);
                    else
                        sb.Append(".");
                }
            return sb.ToString();
        }

        private static IEnumerable<string> DivideToChunk(string str, int chunksize)
        {
            for (int i = 0; i < str.Length; i += chunksize)
            {
                yield return str.Substring(i, Math.Min(chunksize, str.Length - i));
            }
        }

        public static void DivideContentBytesToHexView(string content, string encodedString)
        {
            int i = 0;
            using (var hexChunks = DivideToChunk(content, 17 * 3).GetEnumerator())
            using (var encodedChunks = DivideToChunk(encodedString, 17).GetEnumerator())
                while (hexChunks.MoveNext() && encodedChunks.MoveNext())
                {
                    string chunkHex = hexChunks.Current as string;
                    string chunkEncoded = encodedChunks.Current as string;

                    i++;

                    string[] stringarr = new string[19];
                    chunkHex.Split(' ').CopyTo(stringarr, 1);
                    stringarr[0] = (i * 16).ToString("X");
                    stringarr[stringarr.Length - 1] = chunkEncoded;

                    UIControl.UpdateHexView(stringarr);
                }
        }

    }
}
