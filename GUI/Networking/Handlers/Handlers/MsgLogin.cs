using GUI.Networking.Entities;
using GUI.Networking.Handlers;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace GUI.Networking.Networking.Handlers
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
                FConsole.WriteLine("Authentication successful. Your user Id is: " + Core.MyUser.Id);
                Core.Client.OnLoggedIn?.Invoke();
                var msgDataRequest = MsgDataRequest.CreateFriendListRequest(Core.MyUser.Id);
                Core.Client.Send(msgDataRequest);
            }
            else
                FConsole.WriteLine("Authentication failed.");

            FConsole.WriteLine($"MsgLogin: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}