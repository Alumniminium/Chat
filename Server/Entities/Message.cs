using System;

namespace Server.Database.Entities
{
    public class Message
    {
        public ulong Id;
        public ulong AuthorId;
        public string Text;
        public DateTime Timestamp;
    }
}
