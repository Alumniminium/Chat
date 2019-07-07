using System;

namespace Client.Entities
{
    public class ServerSettings
    {
        public ushort Port;
        public byte PerUserCreateServerAllowance;
        public TimeSpan DeleteInactiveServersAfter;
        public TimeSpan DeleteInactiveUsersAfter;
        public byte MaxMessagesPerUserPerSecond;
        public byte MaxLoginAttempts;
        public byte AutobanDurationSeconds;
    }
}