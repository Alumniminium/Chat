using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgVServer
    {
        public int Length { get; private set; }
        public PacketType Id { get; private set; }
        public int ServerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActivity { get; set; }

        public fixed byte ServerName[16];
        public fixed byte ServerIconUrl[64];


        public string GetServerName()
        {
            fixed (byte* p = ServerName)
                return Encoding.ASCII.GetString(p, 16).Trim('\0');
        }

        public string GetServerIconUrl()
        {
            fixed (byte* p = ServerIconUrl)
                return Encoding.ASCII.GetString(p, 64).Trim('\0');
        }

        public void SetServerName(string serverName)
        {
            for (var i = 0; i < serverName.Length; i++)
                ServerName[i] = (byte)serverName[i];
        }

        public void SetServerIconUrl(string url)
        {
            for (var i = 0; i < url.Length; i++)
                ServerIconUrl[i] = (byte)url[i];
        }

        public static MsgVServer Create(int id, string name, string url, DateTime created, DateTime lastActivity)
        {
            var msg = stackalloc MsgVServer[1];
            msg->Length = sizeof(MsgVServer);
            msg->Id = PacketType.MsgVServer;

            msg->ServerId = id;
            msg->Created = created;
            msg->LastActivity = lastActivity;
            msg->SetServerName(name);
            msg->SetServerIconUrl(url);

            return *msg;
        }

        public static implicit operator byte[](MsgVServer msg)
        {
            var buffer = new byte[sizeof(MsgVServer)];
            fixed (byte* p = buffer)
                *(MsgVServer*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte*(MsgVServer msg)
        {
            var buffer = stackalloc byte[sizeof(MsgVServer)];
            *(MsgVServer*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgVServer(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgVServer*)p;
        }
        public static implicit operator MsgVServer(byte* msg) => *(MsgVServer*)msg;
    }
}
