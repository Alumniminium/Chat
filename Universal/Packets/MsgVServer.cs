using System;
using System.Runtime.InteropServices;
using System.Text;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgVServer
    {
        public const int MAX_SERVER_NAME_LENGTH = 32;
        public const int MAX_SERVER_ICON_LENGTH = 128;
        public int Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public long Created { get; set; }
        public long LastActivity { get; set; }

        public fixed byte ServerName[MAX_SERVER_NAME_LENGTH];
        public fixed byte ServerIconUrl[MAX_SERVER_ICON_LENGTH];


        public string GetServerName()
        {
            fixed (byte* p = ServerName)
                return Encoding.ASCII.GetString(p, MAX_SERVER_NAME_LENGTH).Trim('\0');
        }

        public string GetServerIconUrl()
        {
            fixed (byte* p = ServerIconUrl)
                return Encoding.ASCII.GetString(p, MAX_SERVER_ICON_LENGTH).Trim('\0');
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

        public static MsgVServer Create(int serverId, string name, string url, DateTime created, DateTime lastActivity)
        {
            var msg = stackalloc MsgVServer[1];
            msg->Length = sizeof(MsgVServer);
            msg->Id = PacketType.MsgVServer;

            msg->UniqueId = serverId;
            msg->Created = created.Ticks;
            msg->LastActivity = lastActivity.Ticks;
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
