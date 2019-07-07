using System;

namespace Client.Database.Entities
{
    public class Message
    {
        public ulong Id;
        public ulong AuthorId;
        public string Text;
        public DateTime Timestamp;
    }
}
