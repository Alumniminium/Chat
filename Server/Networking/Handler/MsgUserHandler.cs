using Server.Entities;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgUserHandler
    {
        public static void Process(User user, byte[] buffer)
        {
            var msgUser = (MsgUser)buffer;
            
        }
    }
}
