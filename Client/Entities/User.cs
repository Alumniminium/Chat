using System;
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
        public DateTime LastActivity { get; set; }

        public User()
        {
            Friends = new ConcurrentDictionary<int, User>();
            Servers = new ConcurrentDictionary<int, VirtualServer>();
        }

        public VirtualServer GetServer(int serverId)
        {
            Servers.TryGetValue(serverId, out var server);
            return server;
        }
        public User GetFriend(int friendId)
        {
            Friends.TryGetValue(friendId, out var friend);
            return friend;
        }
    }
}