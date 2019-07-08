using System;
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
                        var uniqueId = msgLogin.UniqueId;
                        if (uniqueId != 0)
                            FastConsole.WriteLine("Authentication successful. Your user Id is: " + uniqueId);
                        else
                            FastConsole.WriteLine("Authentication failed.");
                        break;
                    }
                default:
                    {
                        FastConsole.WriteLine("Unknown Packet ID: "+packetId);
                        break;
                    }
            }
        }
    }
}
