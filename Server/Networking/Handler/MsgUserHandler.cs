using System.Diagnostics;
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


            FConsole.WriteLine($"MsgUser Deserializing & Processing took {((((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000):0} microsec");
        }
    }
}
