using System.Diagnostics;
using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgLoginHandler
    {
        public static void Process(MsgLogin msgLogin)
        {
            if (msgLogin.UniqueId != 0)
            {
                Core.MyUser = new User();
                Core.MyUser.Id = msgLogin.UniqueId;
                Core.MyUser.Username = msgLogin.GetUsername();
                Core.MyUser.Password = msgLogin.GetPassword();
                Core.Client.Socket.UseCompression = msgLogin.ClientSupportCompression;
                FConsole.WriteLine("Authentication successful. Your Username Id is: " + Core.MyUser.Id);
                var msgDataRequest = MsgDataRequest.CreateFriendListRequest(Core.MyUser.Id);
                Core.Client.Send(msgDataRequest);
            }
            else
                FConsole.WriteLine("Authentication failed.");

            FConsole.WriteLine($"MsgLogin Deserializing & Processing took {(((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000000} ns");
        }
    }
}