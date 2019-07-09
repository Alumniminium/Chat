using Server.Entities;

namespace Server
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

        public static Channel GetServerChannelFromId(int serverId, int channelId)
        {
            if (!Collections.VirtualServers.TryGetValue(serverId, out var server))
                return null;

            return server.Channels.TryGetValue(channelId, out var channel) ? channel : null;
        }
    }
}