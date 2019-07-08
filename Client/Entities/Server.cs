using System.Collections.Generic;

namespace Client.Entities
{
    public class VirtualServer
    {
        public int Id;
        public string Name = "";
        public string IconUrl { get; set; }
        public Dictionary<int, User> Users;
        public Dictionary<int,Channel> Channels;

        public VirtualServer()
        {
            Users = new Dictionary<int, User>();
            Channels=new Dictionary<int, Channel>();
        }
    }
}
