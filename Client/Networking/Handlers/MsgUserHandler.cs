using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace Client.Networking.Handlers
{
    public static class MsgUserHandler
    {
        public static void Process(byte[] buffer)
        {
            var msgUser = (MsgUser)buffer;
            var user = new User();
            user.Id = msgUser.UniqueId;
            user.Name = msgUser.GetNickname();
            user.AvatarUrl = msgUser.GetAvatarUrl();
            user.Online = msgUser.Online;
            Core.MyUser.Friends.TryAdd(user.Id, user);
            FConsole.WriteLine($"Received Friend info for {user.Name}!");
        }
    }
}
