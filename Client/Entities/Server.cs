using System.Collections.Generic;

namespace Client.Entities
{
    public class VirtualServer
    {
        public ulong Id;
        public string Name = "";
        public List<User> Users;
        public List<Channel> Channels;
    }
}
