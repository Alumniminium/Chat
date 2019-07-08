using Server.Entities;

namespace Server.Networking
{
    public static class Oracle
    {
        public static User GetUserFromId(int id)
        {
            Collections.Users.TryGetValue(id, out var user);
            return user;
        }

        public static VirtualServer GetServerFromId(int id)
        {
            Collections.VirtualServers.TryGetValue(id, out var server);
            return server;
        }
    }
}