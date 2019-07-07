using System.Collections.Generic;

namespace Client.Entities
{
    public class VirtualServer
    {
        public uint Id { get; set; }
        public uint OwnerId { get; set; }
        public string Name { get; set; }
        public Dictionary<uint, User> Members { get; set; }
        public Dictionary<uint, Channel> Channels { get; set; }


        public VirtualServer()
        {
            Members = new Dictionary<uint, User>();
            Channels = new Dictionary<uint, Channel>();
        }
    }
}
