using Server.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgUserHandler
    {
        public static void Process(User user, byte[] buffer)
        {
            var msgUser = (MsgUser)buffer;


            FConsole.WriteLine($"MsgUser: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}
