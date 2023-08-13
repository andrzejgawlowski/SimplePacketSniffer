using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packetSniffer
{
    class TxtPacketSaver : IPacketSaver
    {
        public void Save(IEnumerable<Packet> packets)
        {
            string contentString = "";
            foreach (var packet in packets)
            {
                string portSrc = (packet is Tcp || packet is Udp) ? packet.SourceIp.ToString() : "";
                string portDst = (packet is Tcp || packet is Udp) ? packet.DestinationIp.ToString() : "";
                contentString += $"{packet.SourceIp.ToString()}\n" +
                    $"{packet.DestinationIp.ToString()}\n" +
                    $"{packet.PacketType.ToString()}\n" +
                    $"{portSrc}\n" +
                    $"{portDst}\n" +
                    $"{packet.TotalLenght}\n" +
                    $"{PacketReader.getEncodedString(packet.rawByteConent)}\n";
            }
            using (var writer = new StreamWriter(PacketSaver.FilePath))
            {
                writer.Write(contentString);
            }
        }
    }
}
