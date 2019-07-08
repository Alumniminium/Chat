using System;
using Client.Entities;
using Universal.IO.FastConsole;
using Universal.Packets;
using Universal.Packets.Enums;

namespace Client.Networking
{
    public static class PacketHandler
    {
        public static void Handle(Client client, byte[] buffer)
        {
            var packetId = (PacketType)BitConverter.ToUInt16(buffer, 4);
            switch (packetId)
            {
                case PacketType.MsgLogin:
                    {
                        var msgLogin = (MsgLogin)buffer;
                        if (msgLogin.UniqueId != 0)
                        {
                            client.Me = new User();
                            client.Me.Username = msgLogin.GetUsername();
                            client.Me.Password = msgLogin.GetPassword();
                            FastConsole.WriteLine("Authentication successful. Your user Id is: " + client.Me.Id);
                            var msgDataRequest = MsgDataRequest.CreateFriendListRequest(client.Me.Id);
                            client.Send(msgDataRequest);
                        }
                        else
                            FastConsole.WriteLine("Authentication failed.");
                        break;
                    }
                case PacketType.MsgDataRequest:
                    {
                        var msgDataRequest = (MsgDataRequest)buffer;

                        switch (msgDataRequest.Type)
                        {
                            case MsgDataRequestType.Friends:
                            {
                                FastConsole.WriteLine("Stage Completed: Sync Friendlist");

                                foreach (var friend in client.Me.Friends)
                                {
                                    var request = MsgDataRequest.CreateRequestMissedMessagesPacket(client.Me.Id, friend.Key);
                                    client.Send(request);
                                }

                                var request2 = MsgDataRequest.CreateServerListRequest(client.Me.Id);
                                client.Send(request2);
                                break;
                            }

                            case MsgDataRequestType.VServers:
                            {
                                FastConsole.WriteLine("Stage Completed: Sync VServers");
                                foreach (var server in client.Me.Servers)
                                {
                                    var request = MsgDataRequest.CreateServerChannelListRequest(client.Me.Id, server.Key);
                                    client.Send(request);
                                }
                                break;
                            }

                            case MsgDataRequestType.Channels:
                                FastConsole.WriteLine("Stage Completed: Sync Channels of " + client.Me.Servers[msgDataRequest.TargetId].Name);
                                foreach (var server in client.Me.Servers.Values)
                                {
                                    foreach (var channel in server.Channels)
                                    {
                                        var request = MsgDataRequest.CreateRequestMissedMessagesPacket(client.Me.Id, server.Id, channel.Key);
                                        client.Send(request);
                                    }
                                }
                                break;
                            case MsgDataRequestType.Messages:
                                FastConsole.WriteLine("Stage Completed: Sync Messages of " + client.Me.Servers[msgDataRequest.TargetId].Channels[msgDataRequest.Param].Name);
                                break;
                            default:
                                FastConsole.WriteLine("Invalid stage.");
                                break;
                        }

                        break;
                    }

                case PacketType.MsgUser:
                    {
                        var msgUser = (MsgUser)buffer;
                        var user = new User();
                        user.Id = msgUser.UniqueId;
                        user.Name = msgUser.GetNickname();
                        user.AvatarUrl = msgUser.GetAvatarUrl();
                        user.Online = msgUser.Online;
                        client.Me.Friends.TryAdd(user.Id, user);
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
                    client.Me.Servers.TryAdd(server.Id, server);
                    FastConsole.WriteLine($"Received Server info for {server.Name}!");
                    break;
                }
                case PacketType.MsgChannel:
                {
                    var msgChannel = (MsgChannel)buffer;
                    var channel = new Channel();
                    channel.Id = msgChannel.UniqueId;
                    channel.Name = msgChannel.GetName();

                    if(msgChannel.ServerId!=0)
                        client.Me.Servers[msgChannel.ServerId].Channels.Add(channel.Id,channel);

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
