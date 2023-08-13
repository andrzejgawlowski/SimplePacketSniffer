using System.IO;

namespace packetSniffer
{
    public class FilePathValidator 
    {
        public static bool IsValid(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    Path.GetFullPath(path);
                    return true;
                }
                catch (System.Exception)
                { return false; }
            }
            else
                return false;
        }
    }
}
