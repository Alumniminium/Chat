using System.Collections.Generic;

namespace Server.Entities
{
    public class VirtualServer
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public List<int> Members { get; set; }
        public Dictionary<int, Channel> Channels { get; set; }


        public VirtualServer()
        {
            Members = new List<int>();
            Channels = new Dictionary<int, Channel>();
        }
    }
}
