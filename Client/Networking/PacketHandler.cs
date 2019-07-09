using Client.Networking.Handlers;
using Universal.Extensions;
using Universal.IO.FastConsole;
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
