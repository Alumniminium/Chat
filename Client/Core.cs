using Client.Entities;

namespace Client
{
    public static class Core
    {
        public static readonly Client Client = new Client();
        public static VirtualServer SelectedServer = null;
        public static Channel SelectedChannel =null;
        public static User MyUser = new User();
        public const string SERVER_IP = "127.0.0.1";
        public const ushort SERVER_PORT = 65535;
    }
}