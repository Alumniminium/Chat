using System.Diagnostics;
using Universal.IO.Sockets.Client;
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
                    FConsole.WriteLine("Invalid packet received from " + user.Username);
                    user.Socket.Disconnect("Server.PacketHandler.Handle() Invalid packet received");
                    break;
            }
        }

        private static void ProcessDataRequest(User user, byte[] packet)
        {
            var msg = (MsgDataRequest)packet;
            FConsole.WriteLine($"MsgDataRequest: {Oracle.GetUserFromId(msg.UserId)} requests {msg.Type} on {msg.TargetId} with param {msg.Param}");
            if (user == null)
                return;
            switch (msg.Type)
            {
                case MsgDataRequestType.Friends:
                    foreach (var friendId in user.Friends)
                    {
                        var friend = Oracle.GetUserFromId(friendId);
                        if (friend != null)
                        {
                            var online = false;
                            if (friend.Socket != null)
                                online = friend.Socket.IsConnected;

                            user.Send(MsgUser.Create(friend.Id, friend.Nickname, friend.AvatarUrl, friend.Email, online));
                        }
                    }
                    user.Send(msg);
                    break;
                case MsgDataRequestType.VServers:
                    foreach (var serverId in user.VirtualServers)
                    {
                        var server = Oracle.GetServerFromId(serverId);
                        if (server != null)
                        {
                            user.Send(MsgVServer.Create(server.Id, server.Name, server.IconUrl, server.CreatedTime, server.LastActivity));
                        }
                    }
                    user.Send(msg);
                    break;
                case MsgDataRequestType.Channels:
                    var dbServer = Oracle.GetServerFromId(msg.TargetId);
                    if (dbServer != null)
                    {
                        foreach (var channelId in dbServer.Channels)
                        {
                            user.Send(MsgChannel.Create(channelId.Key, dbServer.Id, channelId.Value.Name));
                        }
                    }

                    user.Send(msg);
                    break;
                case MsgDataRequestType.Messages:
                    var dbChannel = Oracle.GetServerChannelFromId(msg.TargetId, msg.Param);
                    if (dbChannel != null)
                    {
                        foreach (var message in dbChannel.Messages)
                        {
                            user.Send(MsgText.Create(message.Id, message.AuthorId, message.Text, msg.TargetId,
                                dbChannel.Id, message.Timestamp));
                        }
                    }

                    user.Send(msg);
                    break;
                default:
                    FConsole.WriteLine("Invalid packet received from " + user.Username);
                    user.Socket.Disconnect("Server.PacketHandler.ProcessDataRequest() Invalid packet received");
                    break;
            }

            FConsole.WriteLine($"MsgDataRequest: {Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }

        private static void ProcessLogin(ClientSocket userSocket, byte[] packet)
        {
            var msgLogin = (MsgLogin)packet;
            FConsole.WriteLine($"MsgLogin: {msgLogin.GetUsername()} with password {msgLogin.GetPassword()} requesting login...");
            var (username, password) = msgLogin.GetUserPass();

            var user = new User();
            user.Socket = userSocket;
            user.Socket.StateObject = user;
            user.Username = username;
            user.Password = password;

            if (Core.Db.Authenticate(username, password))
            {
                Core.Db.LoadUser(ref user);
                msgLogin.UniqueId = user.Id;
            }
            else if (Core.Db.AddUser(user))
            {
                msgLogin.UniqueId = user.Id;
            }

            user.Send(msgLogin);
            var userInfoPacket = MsgUser.Create(user.Id, user.Nickname, user.AvatarUrl, user.Email, true);
            foreach (var friendId in user.Friends)
            {
                var friend = Oracle.GetUserFromId(friendId);
                if (friend.Socket!=null && friend.Socket.IsConnected)
                {
                    friend.Send(userInfoPacket);
                }

            }
            user.Send(userInfoPacket);

            FConsole.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
        }
    }
}