using System;
using AlumniClient.Models;
using Packets;
using Packets.Enums;

namespace AlumniClient.Networking
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
                            Console.WriteLine("Authentication successful. Your user Id is: " + uniqueId);
                        else
                            Console.WriteLine("Authentication failed.");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown Packet ID: "+packetId);
                        break;
                    }
            }
        }
    }
}
