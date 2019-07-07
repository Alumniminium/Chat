using System.Collections.Concurrent;
using Server.Entities;

namespace Server
{
    public static class Collections
    {
        public static ConcurrentDictionary<int, VirtualServer> VirtualServers = new ConcurrentDictionary<int, VirtualServer>();
        public static ConcurrentDictionary<int, User> Users = new ConcurrentDictionary<int, User>();
    }
}