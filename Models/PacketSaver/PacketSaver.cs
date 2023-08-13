
using System.Collections.Generic;

namespace packetSniffer
{
    public class PacketSaver
    {
        private static string filePath;
        public static string FilePath {
            get => filePath;
            set
            {
                if (FilePathValidator.IsValid(value))
                    filePath = value;
                else
                    System.Windows.MessageBox.Show("File path is invalid try another one");
            }
        }
        private IPacketSaver _packetSaver;

        public PacketSaver(string filePath, IPacketSaver packetSaver)
        {
            FilePath = filePath;
            _packetSaver = packetSaver;
        }
        public void Save(IEnumerable<Packet> packets)
        {
            if ((_packetSaver is JsonPacketSaver && System.IO.Path.GetExtension(FilePath) == ".js") || (_packetSaver is TxtPacketSaver && System.IO.Path.GetExtension(FilePath) == ".txt"))
                _packetSaver.Save(packets);
            else
                throw new System.ArgumentException("File Path is invalid for this file try choose diffrent format");
        }
    }
}
