using System.Collections.Generic;

namespace Client.Entities
{
    public class VirtualServer
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public Dictionary<int, User> Members { get; set; }
        public Dictionary<int, Channel> Channels { get; set; }


        public VirtualServer()
        {
            Members = new Dictionary<int, User>();
            Channels = new Dictionary<int, Channel>();
        }
    }
}
