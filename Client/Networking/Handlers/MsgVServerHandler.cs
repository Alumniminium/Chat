using System.Diagnostics;
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

            if (Core.SelectedServer == null)
                Core.SelectedServer = server;

            FConsole.WriteLine($"Received Server info for {server.Name}!");

            FConsole.WriteLine($"MsgVServer Deserializing & Processing took {(((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000000} ns");
        }
    }
}
