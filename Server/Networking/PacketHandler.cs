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

        public static void Handle(ClientSocket userSocket, byte[] packet)
        {
            Stopwatch.Restart();
            var user = (User)userSocket.StateObject;

            switch (packet.GetPacketType())
            {
                case PacketType.MsgLogin:
                    ProcessLogin(userSocket, packet);
                    break;
                case PacketType.MsgDataRequest:
                    ProcessDataRequest(user, packet);
                    break;
                default:
                    FastConsole.WriteLine("Invalid packet received from " + user.Socket.IP);
                    user.Socket.Disconnect();
                    break;
            }
        }

        private static void ProcessDataRequest(User user, byte[] packet)
        {
            var msg = (MsgDataRequest)packet;
            FastConsole.WriteLine($"MsgDataRequest: {Oracle.GetUserFromId(msg.UserId)} requests {msg.Type} on {msg.TargetId} with param {msg.Param}");

            switch (msg.Type)
            {
                case MsgDataRequestType.Friends:
                    break;
                case MsgDataRequestType.VServers:
                    break;
                case MsgDataRequestType.Channels:
                    break;
                case MsgDataRequestType.Messages:
                    break;
                default:
                    FastConsole.WriteLine("Invalid packet received from " + user.Socket.IP);
                    user.Socket.Disconnect();
                    break;
            }
            
            FastConsole.WriteLine($"MsgDataRequest: {Stopwatch.Elapsed.TotalMilliseconds:0.0000}ms");
        }

        private static void ProcessLogin(ClientSocket userSocket, byte[] packet)
        {
            var msgLogin = (MsgLogin)packet;
            FastConsole.WriteLine($"MsgLogin: {msgLogin.GetUsername()} with password {msgLogin.GetPassword()} requesting login...");
            var (username, password) = msgLogin.GetUserPass();

            var user = new User();
            user.Socket = userSocket;
            user.Socket.StateObject = user;
            user.Username = username;
            user.Password = password;

            if (Core.Db.Authenticate(username,password))
            {
                Core.Db.LoadUser(user);
                msgLogin.UniqueId = user.Id;
            }
            else if (Core.Db.AddUser(user))
            {
                msgLogin.UniqueId = user.Id;
            }

            user.Send(msgLogin);

            FastConsole.WriteLine("MsgLogin: " + Stopwatch.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
        }
    }
}