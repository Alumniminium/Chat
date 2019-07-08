using System.Diagnostics;
using AlumniSocketCore.Client;
using Universal.Packets;
using Universal.Packets.Enums;
using Server.Entities;
using Universal.Extensions;
using Universal.IO.FastConsole;

namespace Server.Networking
{
    public static class PacketHandler
    {
        public static readonly Stopwatch Stopwatch = new Stopwatch();

        public static void Handle(ClientSocket userSocket, byte[] packet)
        {
            Stopwatch.Restart();
            var user = (User)userSocket.StateObject;

            switch (packet.GetPacketType())
            {
                case PacketType.MsgLogin:
                    ProcessLogin(userSocket, packet);
                    break;
                case PacketType.MsgDataRequest:
                    ProcessDataRequest(user, packet);
                    break;
                default:
                    FastConsole.WriteLine("Invalid packet received from " + user.Socket.IP);
                    user.Socket.Disconnect();
                    break;
            }
        }

        private static void ProcessDataRequest(User user, byte[] packet)
        {
            var msg = (MsgDataRequest)packet;
            FastConsole.WriteLine($"MsgDataRequest: {Oracle.GetUserFromId(msg.UserId)} requests {msg.Type} on {msg.TargetId} with param {msg.Param}");

            switch (msg.Type)
            {
                case MsgDataRequestType.Friends:
                    foreach (var friendId in user.Friends)
                    {
                        var friend = Oracle.GetUserFromId(friendId);

                        var online = false;
                        if (friend.Socket != null)
                            online = friend.Socket.IsConnected;

                        user.Send(MsgUser.Create(friend.Id, friend.Nickname, friend.AvatarUrl, friend.Email, online));
                    }
                    user.Send(msg);
                    break;
                case MsgDataRequestType.VServers:
                    foreach (var serverId in user.VirtualServers)
                    {
                        var server = Oracle.GetServerFromId(serverId);
                        #warning JULIAN UNCOMMENT AND MAKE IT WORK
                        //user.Send(MsgVServer.Create(server.Id, server.Name, server.OwnerId));
                    }
                    user.Send(msg);
                    break;
                case MsgDataRequestType.Channels:
                    var dbServer = Oracle.GetServerFromId(msg.TargetId);
                    foreach (var channelId in dbServer.Channels)
                    {
                        #warning JULIAN UNCOMMENT AND MAKE IT WORK
                        //user.Send(MsgChannel.Create(server.Id, id, server.OwnerId));
                    }
                    user.Send(msg);
                    break;
                case MsgDataRequestType.Messages:
                    var dbChannel = Oracle.GetServerFromId(msg.TargetId).Channels[msg.Param];
                    foreach (var message in dbChannel.Messages)
                    {
                        user.Send(MsgText.Create(message.Id, message.AuthorId, message.Text, msg.TargetId, dbChannel.Id, message.Timestamp));
                    }
                    user.Send(msg);
                    break;
                default:
                    FastConsole.WriteLine("Invalid packet received from " + user.Socket.IP);
                    user.Socket.Disconnect();
                    break;
            }
            
            FastConsole.WriteLine($"MsgDataRequest: {Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }

        private static void ProcessLogin(ClientSocket userSocket, byte[] packet)
        {
            var msgLogin = (MsgLogin)packet;
            FastConsole.WriteLine($"MsgLogin: {msgLogin.GetUsername()} with password {msgLogin.GetPassword()} requesting login...");
            var (username, password) = msgLogin.GetUserPass();

            var user = new User();
            user.Socket = userSocket;
            user.Socket.StateObject = user;
            user.Username = username;
            user.Password = password;

            if (Core.Db.Authenticate(username,password))
            {
                Core.Db.LoadUser(user);
                msgLogin.UniqueId = user.Id;
            }
            else if (Core.Db.AddUser(user))
            {
                msgLogin.UniqueId = user.Id;
            }

            user.Send(msgLogin);

            FastConsole.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
        }
    }
}