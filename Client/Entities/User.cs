
using System.Collections.Concurrent;

namespace Client.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Online { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public ConcurrentDictionary<int, User> Friends { get; set; }
        public ConcurrentDictionary<int, VirtualServer> Servers { get; set; }

        public User()
        {
            Friends = new ConcurrentDictionary<int, User>();
            Servers = new ConcurrentDictionary<int, VirtualServer>();
        }
    }
}