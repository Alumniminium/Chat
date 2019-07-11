using System.Runtime.InteropServices;
using System.Text;
using Universal.Extensions;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgUser
    {
        public const int MAX_NICKNAME_LENGTH = 32;
        public const int MAX_AVATAR_URL_LENGTH = 128;
        public short Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public int ServerId { get; set; }
        public bool Online { get; set; }

        public fixed byte Nickname[MAX_NICKNAME_LENGTH];
        public fixed byte AvatarUrl[MAX_AVATAR_URL_LENGTH];

        public string GetNickname()
        {
            fixed (byte* p = Nickname)
                return Encoding.ASCII.GetString(p, MAX_NICKNAME_LENGTH).Trim('\0');
        }
        public string GetAvatarUrl()
        {
            fixed (byte* p = AvatarUrl)
                return Encoding.ASCII.GetString(p, MAX_AVATAR_URL_LENGTH).Trim('\0');
        }

        public void SetNickname(string nickname)
        {
            nickname = nickname.FillLength(MAX_NICKNAME_LENGTH);
            for (var i = 0; i < nickname.Length; i++)
                Nickname[i] = (byte)nickname[i];
            }
        public void SetAvatarUrl(string url)
        {
            url = url.FillLength(MAX_AVATAR_URL_LENGTH);
            for (var i = 0; i < url.Length; i++)
                AvatarUrl[i] = (byte)url[i];
        }

        public static MsgUser Create(int uniqueId, int serverId, string nickname, string avatarUrl, string email, bool online)
        {
            var msg = stackalloc MsgUser[1];
            msg->Length = (short)sizeof(MsgUser);
            msg->Id = PacketType.MsgUser;

            msg->UniqueId = uniqueId;
            msg->ServerId = serverId;
            msg->Online = online;
            msg->SetNickname(nickname);
            msg->SetAvatarUrl(avatarUrl);
            return *msg;
        }
        public static MsgUser Create(int uniqueId, string nickname, string avatarUrl, string email, bool online)
        {
            var msg = stackalloc MsgUser[1];
            msg->Length = (short)sizeof(MsgUser);
            msg->Id = PacketType.MsgUser;

            msg->UniqueId = uniqueId;
            msg->Online = online;
            msg->SetNickname(nickname);
            msg->SetAvatarUrl(avatarUrl);
            return *msg;
        }
        public static implicit operator byte[] (MsgUser msg)
        {
            var buffer = new byte[sizeof(MsgUser)];
            fixed (byte* p = buffer)
                *(MsgUser*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte* (MsgUser msg)
        {
            var buffer = stackalloc byte[sizeof(MsgUser)];
            *(MsgUser*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgUser(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgUser*)p;
        }
        public static implicit operator MsgUser(byte* msg) => *(MsgUser*)msg;
    }
}