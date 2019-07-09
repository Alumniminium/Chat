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
                FConsole.WriteLine("Authentication successful. Your user Id is: " + Core.MyUser.Id);
                var msgDataRequest = MsgDataRequest.CreateFriendListRequest(Core.MyUser.Id);
                Core.Client.Send(msgDataRequest);
            }
            else
                FConsole.WriteLine("Authentication failed.");
        }
    }
}