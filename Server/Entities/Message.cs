using System;

namespace Client.Database.Entities
{
    public class Message
    {
        public int Id;
        public int AuthorId;
        public string Text;
        public DateTime Timestamp;
    }
}
