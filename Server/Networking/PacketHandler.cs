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
        public static User User;
        public static Stopwatch Stopwatch = new Stopwatch();

        public static void Handle(ClientSocket client, byte[] packet)
        {
            var id = (PacketType)BitConverter.ToUInt16(packet, 4);
            ClientSocket = client;
            User = (User)client.StateObject;
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

                User = new User
                {
                    Username = user,
                    Password = pass
                };

                ClientSocket.StateObject = User;
                User.Socket = ClientSocket;

                if (Core.Db.Authenticate(ref User))
                    msgLogin->UniqueId = (uint)User.Id;
                else if (Core.Db.AddUser(User))
                    msgLogin->UniqueId = (uint)User.Id;

                User.Send(*msgLogin);
                
                Console.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
            }
        }
    }
}