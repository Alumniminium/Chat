using AvaloniaMVVMClient.Database;
using AvaloniaMVVMClient.Networking.Entities;
using AvaloniaMVVMClient.Networking.Handlers;
using Universal.IO.FastConsole;
using Universal.Packets;

namespace AvaloniaMVVMClient.Networking.Networking.Handlers
{
    public static class MsgUserHandler
    {
        public static void Process(byte[] buffer)
        {
            var msgUser = (MsgUser)buffer;
            var user = new User
            {
                Id = msgUser.UniqueId
            };

            if (user.Id == Core.MyUser.Id)
            {
                Core.MyUser.Name = msgUser.GetNickname();
                Core.MyUser.AvatarUrl = msgUser.GetAvatarUrl();
                Core.MyUser.Online = true;
                Core.MyUser.Servers[0].IconUrl = msgUser.GetAvatarUrl();
            }
            else
            {
                user.Name = msgUser.GetNickname();
                user.AvatarUrl = msgUser.GetAvatarUrl();
                user.Online = msgUser.Online;
                if (msgUser.ServerId == 0)
                {
                    Core.MyUser.Friends.TryAdd(user.Id, user);
                    Core.MyUser.Servers[msgUser.ServerId].Channels.Add(user.Id, new Channel(user.Id, user.Name));
                }
                else
                {
                    var server = Core.MyUser.GetServer(msgUser.ServerId);
                    server?.AddUser(user);
                }
                FConsole.WriteLine($"Received Friend info for {user.Name}!");
            }
            FConsole.WriteLine($"MsgUser: {PacketRouter.Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}
