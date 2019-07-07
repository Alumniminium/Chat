using System;
using System.Diagnostics;
using Packets;
using Packets.Enums;
using AlumniSocketCore.Client;
using Server.Entities;

namespace Server.Networking
{
    public static class PacketHandler
    {
        public static ClientSocket ClientSocket;
        public static User Client;
        public static Stopwatch Stopwatch = new Stopwatch();

        public static void Handle(ClientSocket client, byte[] packet)
        {
            var id = (PacketType)BitConverter.ToUInt16(packet, 4);
            ClientSocket = client;
            Client = (User)client.StateObject;
            Stopwatch.Restart();

            switch (id)
            {
                case PacketType.MsgLogin:
                    ProcessLogin(packet);
                    break;
                default:
                    Console.WriteLine("Invalid packet received from " + client.Socket.RemoteEndPoint);
                    client.Disconnect();
                    break;
            }
        }

        private static unsafe void ProcessLogin(byte[] packet)
        {
            fixed (byte* p = packet)
            {
                var msgLogin = (MsgLogin*)p;
                var (user, pass) = msgLogin->GetUserPass();
                var id = msgLogin->UniqueId;
                Console.WriteLine(user + " " + pass + " " + id);

                Client = new User
                {
                    Username = user,
                    Password = pass
                };

                ClientSocket.StateObject = Client;
                Client.Socket = ClientSocket;

                if (Core.Db.Authenticate(ref Client))
                    msgLogin->UniqueId = (uint)Client.Id;
                else if (Core.Db.AddUser(Client))
                    msgLogin->UniqueId = (uint)Client.Id;

                Client.Send(*msgLogin);
                
                Console.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.00000") + "ms");
            }
        }
    }
}