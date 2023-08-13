using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace packetSniffer
{
    public class SniffMonitor
    {
        const int BUFF_SIZE = 1024*1024;

        private IReceive _receiveType;

        private Socket sniffSocket;
        public IPAddress iP_Address { get; set; }
        public ObservableCollection<Packet> packets { get; set; } = new ObservableCollection<Packet>();
        public List<Udp> udpPackets = new List<Udp>();
        private System.Threading.CancellationTokenSource source;
        private System.Threading.CancellationTokenSource source2;
        private System.Threading.CancellationToken token;
        private System.Threading.CancellationToken token2;
        private List<int> listenPorts;
        private byte[] buffer;
        
        public SniffMonitor(List<int> ports, IPAddress clientIP, IReceive receiveType)
        {
            this.buffer = new byte[BUFF_SIZE];
            this.iP_Address = clientIP; /**/
            listenPorts = ports;
            _receiveType = receiveType;
        }

        // Basic socket set-up

        private Socket SetUpRawIpSocket()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            socket.Blocking = false;
            socket.Bind(new IPEndPoint(iP_Address, 0));
            socket.IOControl(IOControlCode.ReceiveAll, BitConverter.GetBytes(1), null);
            return socket;
        }

        public void Start()
        {
            StopRecv();
            ByteCalculator.ResetTotalSize();
            UIControl.senderCounter = 0;
            UIControl.recvCounter = 0;

            if (sniffSocket is null)
            {
                source = new System.Threading.CancellationTokenSource();
                token = source.Token;
                try
                {
                    UIControl.ClearItemSourceList();
                    sniffSocket = SetUpRawIpSocket();
                    sniffSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);

                    if (Options.CaptureDuration > 0)
                        Task.Run(() => Timer((uint)Options.CaptureDuration));
                }
                catch (SocketException e)
                {
                    StopRecv();
                    MessageBox.Show("Socket error: " + e.Message);
                }
            }
        }
    
        private void ReadCallback(IAsyncResult result)
        {
            if (sniffSocket != null)
            {
                int read = sniffSocket.EndReceive(result);
                if (read > 20)
                {
                    byte[] reciveBuffer = new byte[read];
                    Array.Copy(buffer, 0, reciveBuffer, 0, read);
                    
                    try
                    {
                        _receiveType.Receive(result, token, reciveBuffer, listenPorts, source);
                        
                    }
                    catch (ArgumentException e)
                    {
                        MessageBox.Show(e.Message,"Warning",MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                if (sniffSocket != null && !token.IsCancellationRequested)
                    sniffSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);
            }
        }

        private void Timer(uint time)
        {
            while (0 < time--)
                System.Threading.Thread.Sleep(1000);
            if (token != null && source != null)
                StopRecv();
        }

        public void StopRecv()
        {
            if (token != null && source != null && !token.IsCancellationRequested)
            { source.Cancel(); source.Dispose(); }
            UIControl.SniffTurnOff();
        }

        public void StopSend()
        {
            if (token2 != null && source2 != null && !token2.IsCancellationRequested)
            { source2.Cancel(); source2.Dispose(); }
        }

        public void SetUpRecive(IReceive receive)
        {
            _receiveType = receive;
        }

        public void Send(int count, int delay)
        {
            source2 = new System.Threading.CancellationTokenSource();
            token2 = source2.Token;
            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var packet in udpPackets)
                    {
                        if (!token2.IsCancellationRequested)
                            packet.Send(count, delay, token);
                        else
                            return;
                    }
                }
            });
        }
    }
}
