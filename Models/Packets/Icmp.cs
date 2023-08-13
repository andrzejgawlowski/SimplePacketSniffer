
using System.Net;

namespace packetSniffer
{
    class Icmp : Packet, IPacket
    {
        public Icmp(byte[] rawByteConent, Protocol packetType, IPAddress sourceIP, IPAddress destinationIP, int totalLenght) : base(rawByteConent, packetType ,sourceIP, destinationIP, totalLenght){}

        public override void Send(int count, int delay, System.Threading.CancellationToken token)
        {
            //throw new System.NotImplementedException();
            // Not implemented
        }
    }
}
