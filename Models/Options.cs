using System.Net.NetworkInformation;

namespace packetSniffer
{
    class Options
    {
        /*Interfaces*/

        public static int CaptureDuration { get; set; }
        public static string FilePath { get; set; }
        public static int MaxPacketSize { get; set; }
        public static int MaxPacketCount { get; set; }

        public static void SetUpInterfaces()
        {
            UIControl.SetUpInterfaces();
        }
        public static void SaveOptions(int captureDuration, string filePath, int maxpacketsize, int maxpacketcount)
        {
            CaptureDuration = captureDuration;
            FilePath = filePath;
            MaxPacketSize = maxpacketsize;
            MaxPacketCount = maxpacketcount;
        }
        public static void ResetOptions()
        {
            /*Default options*/
            UIControl.ResetControlsToDefault();
        }
    }
}
