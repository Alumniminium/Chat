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


            FConsole.WriteLine($"MsgUser Deserializing & Processing took {(PacketRouter.Stopwatch.Elapsed.TotalMilliseconds * 1000):0.00} microsecs");
        }
    }
}
