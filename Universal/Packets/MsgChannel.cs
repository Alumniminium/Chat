using System.Runtime.InteropServices;
using System.Text;
using Universal.Extensions;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgChannel
    {
        public const int MAX_NAME_ENGTH = 48;
        public short Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public int ServerId { get; set; }

        public fixed byte Name[MAX_NAME_ENGTH];

        public string GetName()
        {
            fixed (byte* p = Name)
                return Encoding.ASCII.GetString(p, MAX_NAME_ENGTH).Trim('\0');
        }
        public void SetName(string username)
        {
            username = username.FillLength(MAX_NAME_ENGTH);
            for (var i = 0; i < username.Length; i++)
                Name[i] = (byte)username[i];
        }

        public static MsgChannel Create(int uniqueId, int serverId, string name)
        {
            var msg = stackalloc MsgChannel[1];
            msg->Length = (short)sizeof(MsgChannel);
            msg->Id = PacketType.MsgChannel;
            msg->UniqueId = uniqueId;
            msg->ServerId = serverId;
            msg->SetName(name);
            return *msg;
        }
        public static implicit operator byte[](MsgChannel msg)
        {
            var buffer = new byte[sizeof(MsgChannel)];
            fixed (byte* p = buffer)
                *(MsgChannel*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte*(MsgChannel msg)
        {
            var buffer = stackalloc byte[sizeof(MsgChannel)];
            *(MsgChannel*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgChannel(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgChannel*)p;
        }
        public static implicit operator MsgChannel(byte* msg) => *(MsgChannel*)msg;
    }
}