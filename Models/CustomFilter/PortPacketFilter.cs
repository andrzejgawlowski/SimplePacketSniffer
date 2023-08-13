using System.Collections.Generic;
using System.Linq;

namespace packetSniffer
{
    class PortPacketFilter : IPacketFilter
    {
        private int _srcPort;
        private int _dstPort;

        public PortPacketFilter(string srcPort, string dstPort)
        {
            int.TryParse(srcPort, out _srcPort);
            int.TryParse(dstPort, out _dstPort);
        }

        public IEnumerable<Packet> Filter(IEnumerable<Packet> packets)
        {
            // Dało by się porawić dodając klasę pośrednia która jest wyżej od Packet ale niżej niż konkretny typ taki jak UDP czy TCP

            var listU = new List<Udp>();
            var listT = new List<Tcp>();

            if (_dstPort > 0 && _srcPort > 0)
            {
                listU = packets.OfType<Udp>().Where(x => (x as Udp).PortSource == _srcPort && (x as Udp).PortDestination == _dstPort).ToList();
                listT = packets.OfType<Tcp>().Where(x => (x as Tcp).PortSource == _srcPort && (x as Tcp).PortDestination == _dstPort).ToList();
            }
            else
            {
                if (_srcPort > 0)
                {
                    listU.AddRange(packets.OfType<Udp>().Where(x => (x as Udp).PortSource == _srcPort));
                    listT.AddRange(packets.OfType<Tcp>().Where(x => (x as Tcp).PortSource == _srcPort));
                }

                else if (_dstPort > 0)
                {
                    listU.AddRange(packets.OfType<Udp>().Where(x => (x as Udp).PortDestination == _dstPort));
                    listT.AddRange(packets.OfType<Tcp>().Where(x => (x as Tcp).PortDestination == _dstPort));
                }
                else
                    return packets;
            }
            return listU.Cast<Packet>().Concat(listT.Cast<Packet>());
        }
    }
}
