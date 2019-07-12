using System.Diagnostics;
using Server.Entities;
using Server.Networking.Handler;
using Universal.Extensions;
using Universal.IO.FastConsole;
using Universal.IO.Sockets.Client;
using Universal.Packets.Enums;

namespace Server.Networking
{
    public static class PacketRouter
    {
        public static readonly Stopwatch Stopwatch = new Stopwatch();

        public static void Route(ClientSocket userSocket, byte[] packet)
        {
            Stopwatch.Restart();
            var user = (User)userSocket.StateObject;

            switch (packet.GetPacketType())
            {
                case PacketType.MsgLogin:
                    MsgLoginHandler.Process(userSocket, packet);
                    break;
                case PacketType.MsgDataRequest:
                    MsgDataRequestHandler.Process(user, packet);
                    break;
                case PacketType.MsgUser:
                    MsgUserHandler.Process(user, packet);
                    break;
                case PacketType.MsgVServer:
                    MsgVServerHandler.Process(user, packet);
                    break;
                case PacketType.MsgChannel:
                    MsgChannelHandler.Process(user, packet);
                    break;
                case PacketType.MsgText:
                    MsgTextHandler.Process(user, packet);
                    break;
                default:
                    FConsole.WriteLine("Invalid packet received from " + user.Username);
                    user.Socket.Disconnect("Server.Networking.PacketRouter.Route() Invalid packet received");
                    break;
            }
        }

        
    }
}