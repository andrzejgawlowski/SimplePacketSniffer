using System;
using System.Collections.Generic;

namespace packetSniffer
{
    class UnlimitedReceive : IReceive
    {
        public void Receive(IAsyncResult result, System.Threading.CancellationToken token, byte[] buffer, List<int>listenPorts, System.Threading.CancellationTokenSource source)
        {
            // If not cancell
            if (!token.IsCancellationRequested)
            {
                var packetReader = new PacketReader(buffer);
                var packet = packetReader.GetProcessedPacket();

                if (packet.TotalLenght > 0)
                {
                    if (listenPorts.Capacity > 0)
                    {
                        if (packet is Udp || packet is Tcp)
                        {
                            if (listenPorts.Contains((packet as Tcp).PortSource) || listenPorts.Contains((packet as Tcp).PortDestination))
                            {
                                packet.Id = ++UIControl.recvCounter;
                                UIControl.DynamicUpdate(packet);
                                UIControl.UpdateTotalSize(ByteCalculator.CalculateByteString(packet.TotalLenght));
                                System.Threading.Thread.Sleep(5);
                            }
                        }
                    }
                    else
                    {
                        packet.Id = ++UIControl.recvCounter;
                        UIControl.DynamicUpdate(packet);
                        UIControl.UpdateTotalSize(ByteCalculator.CalculateByteString(packet.TotalLenght));
                        System.Threading.Thread.Sleep(5);
                    }
                }
            }
        }
    }
}
