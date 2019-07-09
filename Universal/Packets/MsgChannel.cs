﻿using System.Runtime.InteropServices;
using System.Text;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgChannel
    {
        public int Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public int ServerId { get; set; }

        public fixed byte Name[32];

        public string GetName()
        {
            fixed (byte* p = Name)
                return Encoding.ASCII.GetString(p, 32).Trim('\0');
        }
        public void SetName(string username)
        {
            for (var i = 0; i < username.Length; i++)
                Name[i] = (byte)username[i];
        }

        public static MsgChannel Create(int uniqueId,int serverId, string name)
        {
            var msg = stackalloc MsgChannel[1];
            msg->Length = sizeof(MsgChannel);
            msg->Id = PacketType.MsgChannel;
            msg->UniqueId = uniqueId;
            msg->ServerId = serverId;
            msg->SetName(name);
            return *msg;
        }
        public static implicit operator byte[] (MsgChannel msg)
        {
            var buffer = new byte[sizeof(MsgChannel)];
            fixed (byte* p = buffer)
                *(MsgChannel*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte* (MsgChannel msg)
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