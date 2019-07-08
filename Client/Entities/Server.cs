using System.Collections.Generic;

namespace Client.Entities
{
    public class VirtualServer
    {
        public int Id;
        public string Name = "";
        public List<User> Users;
        public List<Channel> Channels;
        public string IconUrl { get; set; }
    }
}
