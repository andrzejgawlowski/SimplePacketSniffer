
namespace packetSniffer
{
    class Tcp : Packet, IPacket
    {
        public int PortSource { get; set; }

        public int PortDestination { get; set; }

        public Tcp(byte[] rawByteConent, Protocol packetType, System.Net.IPAddress sourceIp, System.Net.IPAddress destinationIp, int portSource, int portDestination, int totalLenght) : base(rawByteConent, packetType ,sourceIp, destinationIp, totalLenght)
        {
            PortSource = portSource;
            PortDestination = portDestination;
        }

        public override void Send(int count, int delay, System.Threading.CancellationToken token)
        {
            // Not implemented
        }
    }
}
