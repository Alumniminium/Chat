using System.Diagnostics;
using Client.Networking.Handlers;
using Universal.Extensions;
using Universal.IO.FastConsole;
using Universal.Packets.Enums;

namespace Client.Networking
{
    public static class PacketRouter
    {
        public static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        public static void Route(Client client, byte[] buffer)
        {
            Stopwatch.Restart();
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
                    MsgUserHandler.Process(buffer);
                    break;
                case PacketType.MsgVServer:
                    MsgVServerHandler.Process(buffer);
                    break;
                case PacketType.MsgChannel:
                    MsgChannelHandler.Process(buffer);
                    break;
                case PacketType.MsgText:
                    MsgTextHandler.Process(buffer);
                    break;
                default:
                    {
                        FConsole.WriteLine("Unknown Packet ID: " + packetId);
                        break;
                    }
            }
        }
    }
}
