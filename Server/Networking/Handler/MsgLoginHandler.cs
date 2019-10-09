using System.Diagnostics;
using Server.Entities;
using Universal.IO.FastConsole;
using Universal.IO.Sockets.Client;
using Universal.Packets;

namespace Server.Networking.Handler
{
    public static class MsgLoginHandler
    {
        public static void Process(ClientSocket userSocket, byte[] packet)
        {
            var msgLogin = (MsgLogin)packet;
            var username = msgLogin.GetUsername();
            var password = msgLogin.GetPassword();
            userSocket.UseCompression = msgLogin.ClientSupportCompression;
            FConsole.WriteLine($"MsgLogin: {username} with password {password} (compress: {userSocket.UseCompression}) requesting login.");

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
            }
            else
                Core.Db.AddUser(user);
            msgLogin.UniqueId = user.Id;
            Collections.OnlineUsers.TryAdd(user.Id, user);
            user.Send(msgLogin, true);

            var userInfoPacket = MsgUser.Create(user.Id, user.Nickname, user.AvatarUrl, user.Email, true);
            foreach (var friendId in user.Friends)
            {
                var friend = Oracle.GetUserFromId(friendId);
                if (friend.Socket != null && friend.Socket.IsConnected)
                    friend.Send(userInfoPacket);
            }
            user.Send(userInfoPacket);
            FConsole.WriteLine($"MsgLogin Deserializing & Processing took {((((float)PacketRouter.Stopwatch.ElapsedTicks) / Stopwatch.Frequency) * 1000000):0} microsec");
        }
    }
}