using System.Collections.Concurrent;
using Client.Entities;

namespace Client
{
    public static class Collections
    {
        public static ConcurrentDictionary<int, VirtualServer> VirtualServers = new ConcurrentDictionary<int, VirtualServer>();
        public static ConcurrentDictionary<int, User> Users = new ConcurrentDictionary<int, User>();
    }
}