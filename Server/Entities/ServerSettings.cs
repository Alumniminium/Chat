using System;

namespace Server.Entities
{
    public class ServerSettings
    {
        public ushort Port { get; set; }
        public byte PerUserCreateServerAllowance { get; set; }
        public TimeSpan DeleteInactiveServersAfter { get; set; }
        public TimeSpan DeleteInactiveUsersAfter { get; set; }
        public byte MaxMessagesPerUserPerSecond { get; set; }
        public byte MaxLoginAttempts { get; set; }
        public byte AutobanDurationSeconds { get; set; }
    }
}