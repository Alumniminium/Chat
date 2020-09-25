using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GUI
{
    public class StateFile
    {
        public string ServerIP = "127.0.0.1";
        public ushort Port = 65535;
        public string Username;
        public string Password;
        public bool RememberCredentials;
        public ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();
    }
}