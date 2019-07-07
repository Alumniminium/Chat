using System;
using System.Diagnostics;
using Packets;
using Packets.Enums;
using AlumniSocketCore.Client;
using Client.Entities;
using Client.IO.FastConsole;

namespace Client.Networking
{
    public static class PacketHandler
    {
        public static ClientSocket Socket;
        public static User User;
        public static Stopwatch Stopwatch = new Stopwatch();

        public static void Handle(ClientSocket socket, byte[] packet)
        {
            var id = (PacketType)BitConverter.ToUInt16(packet, 4);
            Socket = socket;
            User = (User)socket.StateObject;
            Stopwatch.Restart();

            switch (id)
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