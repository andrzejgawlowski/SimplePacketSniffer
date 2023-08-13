using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace packetSniffer
{
    class ListenPortFinder
    {
        const string cmdPath = @"\system32\cmd.exe";

        static string windowPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

        static string netStatWithAno = "/c netstat -ano | find /i \"established\" | ";

        private static List<int> listenPorts = new List<int>();

        public static List<int>FindBy(string pid)
        {
            List<int> portList = new List<int>();
            var psi = new ProcessStartInfo
            {
               FileName = windowPath + cmdPath,
               Arguments = netStatWithAno +"find \"" + pid + "\"",
               RedirectStandardOutput = true,
               CreateNoWindow = true,
               UseShellExecute = false
            };
            var process = Process.Start(psi);
            var oiu = process.StandardOutput.ReadToEnd().Split(' ');
            if (oiu.Length == 1)
                return new List<int>();
            else
            {
                List<string> list = new List<string>();
                foreach (string element in oiu)
                {
                    //skip blanks
                    if (element.Trim() == "") continue;
                    list.Add(element);
                }
                for (int i = 1; i < list.Count; i += 5)
                    portList.Add(int.Parse(list[i].Split(':')[1]));
                return portList;
            }
        }

        public static List<int> GetPortList()
        {
            if (listenPorts.Count > 0)
                return listenPorts;
            return new List<int>();
        }
    }
}
