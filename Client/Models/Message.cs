using System;

namespace AlumniClient.Models
{
    public class Message
    {
        public ulong Id;
        public ulong AuthorId;
        public string Text;
        public DateTime Timestamp;
    }
}