using Server.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgChannelHandler
    {
        public static void Process(User user, byte[] buffer)
        {
            var msgChannel = (MsgChannel)buffer;


            FConsole.WriteLine($"Received Server info for {msgChannel.GetName()}!");
            FConsole.WriteLine($"MsgChannel: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}
