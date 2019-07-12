using System.Diagnostics;
using Server.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;
using Universal.Packets.Enums;

namespace Server.Networking.Handler
{
    public static class MsgDataRequestHandler
    {
        public static readonly Stopwatch Stopwatch = PacketRouter.Stopwatch;
        public static void Process(User user, byte[] packet)
        {
            var msg = (MsgDataRequest)packet;
            FConsole.WriteLine($"MsgDataRequest: {Oracle.GetUserFromId(msg.UserId).Username} requests {msg.Type} on {msg.TargetId} with param {msg.Param}");
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
                        foreach (var (_, message) in dbChannel.Messages)
                        {
                            user.Send(MsgText.Create(message.Id, message.AuthorId, message.Text, msg.TargetId, dbChannel.Id, message.Timestamp));
                        }
                    }

                    user.Send(msg);
                    break;
                default:
                    FConsole.WriteLine("Invalid packet received from " + user.Username);
                    user.Socket.Disconnect("Server.Networking.PacketRouter.ProcessDataRequest() Invalid packet received");
                    break;
            }

            FConsole.WriteLine($"MsgDataRequest: {Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }
    }
}