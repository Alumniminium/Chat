using System;
using System.Runtime.InteropServices;
using System.Text;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgText
    {
        public const int MAX_TEXT_LENGTH = 2048;
        public int Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public int ServerId { get; set; } // 0 == Direct Message
        public int ChannelId { get; set; }// if ServerId == 0  then  ChannelId = FriendId
        public int AuthorId { get; set; }
        public long SentTime { get; set; }

        public fixed byte Text[MAX_TEXT_LENGTH];

        public string GetText()
        {
            fixed (byte* p = Text)
                return Encoding.ASCII.GetString(p, 2048).Trim('\0');
        }

        public void SetText(string text)
        {
            for (var i = 0; i < text.Length; i++)
                Text[i] = (byte)text[i];
        }

        public int FriendId => ChannelId;

        public static byte[] Create(int uniqueId, int authorId, string text, int serverId, int channelId, DateTime createdTime)
        {
            var msg = stackalloc MsgText[1];
            msg->Length = sizeof(MsgText);
            msg->Id = PacketType.MsgText;

            msg->UniqueId = uniqueId;
            msg->ServerId = serverId;
            msg->AuthorId = authorId;
            msg->ChannelId = channelId;
            msg->SentTime = createdTime.Ticks;

            msg->SetText(text);

            return *msg;
        }
        public static byte[] Create(int uniqueId, int authorId, string text, int channelId, DateTime createdTime)
        {
            var msg = stackalloc MsgText[1];
            msg->Length = sizeof(MsgText);
            msg->Id = PacketType.MsgText;

            msg->UniqueId = uniqueId;
            msg->AuthorId = authorId;
            msg->ChannelId = channelId;
            msg->SentTime = createdTime.Ticks;

            msg->SetText(text);

            return *msg;
        }

        public static implicit operator byte[](MsgText msg)
        {
            var buffer = new byte[sizeof(MsgText)];
            fixed (byte* p = buffer)
                *(MsgText*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte*(MsgText msg)
        {
            var buffer = stackalloc byte[sizeof(MsgText)];
            *(MsgText*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgText(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgText*)p;
        }
        public static implicit operator MsgText(byte* msg) => *(MsgText*)msg;

    }
}