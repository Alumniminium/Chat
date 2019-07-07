using System.Collections.Generic;

namespace AlumniClient.Models
{
    public class Server
    {
        public ulong Id;
        public string Name = "";
        public List<User> Users;
        public List<Channel> Channels;
    }
}
