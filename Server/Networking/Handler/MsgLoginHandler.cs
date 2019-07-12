using System.Diagnostics;
using Server.Entities;
using Universal.IO.FastConsole;
using Universal.IO.Sockets.Client;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgLoginHandler
    {
        public static readonly Stopwatch Stopwatch = PacketRouter.Stopwatch;
        public static void Process(ClientSocket userSocket, byte[] packet)
        {
            var msgLogin = (MsgLogin)packet;
            var username = msgLogin.GetUsername();
            var password = msgLogin.GetPassword();
            var useCompression = msgLogin.ClientSupportCompression;
            FConsole.WriteLine($"MsgLogin: {username} with password {password} requesting login.");

            var user = new User
            {
                Socket = userSocket,
                Username = username,
                Password = password
            };

            user.Socket.OnDisconnect += user.OnDisconnect;
            user.Socket.StateObject = user;

            if (Core.Db.Authenticate(username, password))
            {
                Core.Db.LoadUser(ref user);
                msgLogin.UniqueId = user.Id;
            }
            else if (Core.Db.AddUser(user))
            {
                msgLogin.UniqueId = user.Id;
            }

            user.Socket.UseCompression = useCompression;
            Collections.OnlineUsers.TryAdd(user.Id, user);
            user.Send(msgLogin);

            var userInfoPacket = MsgUser.Create(user.Id, user.Nickname, user.AvatarUrl, user.Email, true);
            foreach (var friendId in user.Friends)
            {
                var friend = Oracle.GetUserFromId(friendId);
                if (friend.Socket != null && friend.Socket.IsConnected)
                {
                    friend.Send(userInfoPacket);
                }
            }
            user.Send(userInfoPacket);

            FConsole.WriteLine($"MsgLogin: {Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}