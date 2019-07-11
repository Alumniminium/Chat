using System;
using System.Runtime.InteropServices;
using System.Text;
using Universal.Packets.Enums;

namespace Universal.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgLogin
    {
        public const int MAX_USERNAME_LENGTH = 32;
        public const int MAX_PASSWORD_LENGTH = 32;

        public short Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UniqueId { get; set; }
        public fixed byte Username[MAX_USERNAME_LENGTH];
        public fixed byte Password[MAX_PASSWORD_LENGTH];

        public string GetUsername()
        {
            fixed (byte* p = Username)
                return Encoding.ASCII.GetString(p, MAX_USERNAME_LENGTH).Trim('\0');
        }
        public string GetPassword()
        {
            fixed (byte* p = Password)
                return Encoding.ASCII.GetString(p, MAX_PASSWORD_LENGTH).Trim('\0');
        }

        public (string user, string pass) GetUserPass()
        {
            var stringBuilder = new StringBuilder();
            var pair = ValueTuple.Create(string.Empty, string.Empty);
            for (var i = 0; i < 64; i++)
            {
                if (i == 32)
                {
                    pair.Item1 = stringBuilder.ToString().Trim('\0');
                    stringBuilder.Clear();
                }
                stringBuilder.Append((char)Username[i]);
            }
            pair.Item2 = stringBuilder.ToString().Trim('\0');
            return pair;
        }

        public void SetUsername(string username)
        {
            for (var i = 0; i < username.Length; i++)
                Username[i] = (byte)username[i];
            for (var i = username.Length; i < MAX_USERNAME_LENGTH; i++)
                Username[i] = (byte)'\0';
        }
        public void SetPassword(string password)
        {
            for (var i = 0; i < password.Length; i++)
                Password[i] = (byte)password[i];
            for (var i = password.Length; i < MAX_PASSWORD_LENGTH; i++)
                Password[i] = (byte)'\0';
        }

        public static MsgLogin Create(string user, string pass)
        {
            var msg = stackalloc MsgLogin[1];
            msg->Length = (short)sizeof(MsgLogin);
            msg->Id = PacketType.MsgLogin;

            msg->SetUsername(user);
            msg->SetPassword(pass);
            return *msg;
        }
        public static implicit operator byte[] (MsgLogin msg)
        {
            var buffer = new byte[sizeof(MsgLogin)];
            fixed (byte* p = buffer)
                *(MsgLogin*)p = *&msg;
            return buffer;
        }
        public static implicit operator byte* (MsgLogin msg)
        {
            var buffer = stackalloc byte[sizeof(MsgLogin)];
            *(MsgLogin*)buffer = msg;
            return buffer;
        }
        public static implicit operator MsgLogin(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgLogin*)p;
        }
        public static implicit operator MsgLogin(byte* msg) => *(MsgLogin*)msg;
    }
}
