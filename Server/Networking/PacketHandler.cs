using System;
using System.Diagnostics;
using AlumniSocketCore.Client;
using Universal.Packets;
using Universal.Packets.Enums;
using Server.Entities;
using Universal.Extensions;
using Universal.IO.FastConsole;

namespace Server.Networking
{
    public static class PacketHandler
    {
        public static readonly Stopwatch Stopwatch = new Stopwatch();
        public static ClientSocket Socket;
        public static User User;

        public static void Handle(ClientSocket socket, byte[] packet)
        {
            Socket = socket;
            User = (User)socket.StateObject;
            Stopwatch.Restart();

            switch (packet.GetPacketType())
            {
                case PacketType.MsgLogin:
                    ProcessLogin(packet);
                    break;
                default:
                    FastConsole.WriteLine("Invalid packet received from " + socket.Socket.RemoteEndPoint);
                    socket.Disconnect();
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
                FastConsole.WriteLine(user + " " + pass + " " + id);

                User = new User
                {
                    Username = user,
                    Password = pass
                };

                Socket.StateObject = User;
                User.Socket = Socket;

                if (Core.Db.Authenticate(ref User))
                    msgLogin->UniqueId = (uint)User.Id;
                else if (Core.Db.AddUser(User))
                    msgLogin->UniqueId = (uint)User.Id;

                User.Send(*msgLogin);
                
                FastConsole.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
            }
        }
    }
}