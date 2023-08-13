using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace packetSniffer
{
    public class Udp : Packet, IPacket
    {
        public int PortSource { get; set; }

        public int PortDestination { get; set; }

        public Udp(byte[] rawByteConent, Protocol packetType ,IPAddress sourceIP, IPAddress destinationIP, int portSource, int portDestination, int totalLenght) : base(rawByteConent, packetType, sourceIP, destinationIP, totalLenght)
        {
            PortSource = portSource;
            PortDestination = portDestination;
        }

        public override void Send(int count, int delay, System.Threading.CancellationToken token)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (!DestinationIp.Equals(IPAddress.Broadcast))
            {
                var EndPoint = new IPEndPoint(DestinationIp, PortSource);
                socket.Connect(EndPoint);
                socket.SendTo(rawByteConent, EndPoint);
                UIControl.senderCounter++;
                UIControl.UpdateTotalSended();
                System.Threading.Thread.Sleep(delay);
            }
        }
    }
}
