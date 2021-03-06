using System.Diagnostics;
using Server.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgVServerHandler
    {
        public static void Process(User user, byte[] buffer)
        {
            var msgVServer = (MsgVServer)buffer;
            var server = new VirtualServer();
            server.Id = msgVServer.UniqueId;
            server.Name = msgVServer.GetServerName();
            server.IconUrl = msgVServer.GetServerIconUrl();
            FConsole.WriteLine($"Received Server info for {server.Name}!");
            FConsole.WriteLine($"MsgVServer Deserializing & Processing took {((((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000):0} microsec");
        }
    }
}
