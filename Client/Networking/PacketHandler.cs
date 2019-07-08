using System;
using Client.Entities;
using Client.Networking.Handlers;
using Universal.Extensions;
using Universal.IO.FastConsole;
using Universal.Packets;
using Universal.Packets.Enums;

namespace Client.Networking
{
    public static class PacketHandler
    {
        public static void Handle(Client client, byte[] buffer)
        {
            var packetId = buffer.GetPacketType();

            switch (packetId)
            {
                case PacketType.MsgLogin:
                    MsgLoginHandler.Process(buffer);
                    break;
                case PacketType.MsgDataRequest:
                    MsgDataRequestHandler.Process(buffer);
                    break;
                case PacketType.MsgUser:
                    {
                        var msgUser = (MsgUser)buffer;
                        var user = new User();
                        user.Id = msgUser.UniqueId;
                        user.Name = msgUser.GetNickname();
                        user.AvatarUrl = msgUser.GetAvatarUrl();
                        user.Online = msgUser.Online;
                        Core.MyUser.Friends.TryAdd(user.Id, user);
                        FastConsole.WriteLine($"Received Friend info for {user.Name}!");
                        break;
                    }
                case PacketType.MsgVServer:
                    {
                        var msgVServer = (MsgVServer)buffer;
                        var server = new VirtualServer();
                        server.Id = msgVServer.UniqueId;
                        server.Name = msgVServer.GetServerName();
                        server.IconUrl = msgVServer.GetServerIconUrl();
                        Core.MyUser.Servers.TryAdd(server.Id, server);
                        FastConsole.WriteLine($"Received Server info for {server.Name}!");
                        break;
                    }
                case PacketType.MsgChannel:
                    {
                        var msgChannel = (MsgChannel)buffer;
                        var channel = new Channel();
                        channel.Id = msgChannel.UniqueId;
                        channel.Name = msgChannel.GetName();

                        if (msgChannel.ServerId != 0)
                            Core.MyUser.Servers[msgChannel.ServerId].Channels.Add(channel.Id, channel);

                        FastConsole.WriteLine($"Received Server info for {channel.Name}!");
                        break;
                    }
                case PacketType.MsgText:
                    {
                        
                        break;
                    }
                default:
                    {
                        FastConsole.WriteLine("Unknown Packet ID: " + packetId);
                        break;
                    }
            }
        }
    }
}
