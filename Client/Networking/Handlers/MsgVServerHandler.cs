using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgVServerHandler
    {
        public static void Process(byte[] buffer)
        {
            var msgVServer = (MsgVServer)buffer;
            var server = new VirtualServer();
            server.Id = msgVServer.UniqueId;
            server.Name = msgVServer.GetServerName();
            server.IconUrl = msgVServer.GetServerIconUrl();
            Core.MyUser.Servers.TryAdd(server.Id, server);
            FConsole.WriteLine($"Received Server info for {server.Name}!");
        }
    }
}
