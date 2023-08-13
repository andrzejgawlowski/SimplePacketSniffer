using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace packetSniffer
{
    class Igmp : Packet, IPacket
    {
        public Igmp(byte[] rawByteConent, Protocol packetType, IPAddress sourceIP, IPAddress destinationIP, int totalLenght) : base(rawByteConent, packetType, sourceIP, destinationIP, totalLenght){}

        public override void Send(int count, int delay, CancellationToken token)
        {
            //throw new NotImplementedException();
            // Not implemented
        }
    }
}
