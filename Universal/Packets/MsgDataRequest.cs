using System.Runtime.InteropServices;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgDataRequest
    {
        public short Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public int Param { get; set; }
        public MsgDataRequestType Type { get; set; }

        public static MsgDataRequest Create(int userId, int targetId,int param, MsgDataRequestType type)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->TargetId = targetId;
            msg->Param = param;
            msg->Type = type;

            return *msg;
        }
        public static MsgDataRequest CreateFriendListRequest(int userId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->Type = MsgDataRequestType.Friends;

            return *msg;
        }
        public static MsgDataRequest CreateServerListRequest(int userId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->Type = MsgDataRequestType.VServers;

            return *msg;
        }
        public static byte[] CreateServerChannelListRequest(int userId, int serverId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->TargetId = serverId;
            msg->Type = MsgDataRequestType.Channels;

            return *msg;
        }
        public static MsgDataRequest CreateRequestMissedMessagesPacket(int userId, int serverId, int channelId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->TargetId = serverId;
            msg->Param = channelId;
            msg->Type = MsgDataRequestType.Messages;

            return *msg;
        }
        public static MsgDataRequest CreateRequestMissedMessagesPacket(int userId, int friendId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->Param = friendId;
            msg->Type = MsgDataRequestType.Messages;

            return *msg;
        }
        public static implicit operator byte[] (MsgDataRequest msg)
        {
            var buffer = new byte[sizeof(MsgDataRequest)];
            fixed (byte* p = buffer)
                *(MsgDataRequest*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte* (MsgDataRequest msg)
        {
            var buffer = stackalloc byte[sizeof(MsgDataRequest)];
            *(MsgDataRequest*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgDataRequest(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgDataRequest*)p;
        }
        public static implicit operator MsgDataRequest(byte* msg) => *(MsgDataRequest*)msg;

    }
}