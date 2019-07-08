using System;

namespace Client.Entities
{
    public class Message
    {
        public int Id;
        public int AuthorId;
        public string Text;
        public ulong Timestamp;
    }
}