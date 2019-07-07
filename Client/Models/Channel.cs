using System.Collections.Generic;

namespace AlumniClient.Models
{
    public class Channel
    {
        public ulong Id;
        public string Name;

        public List<Message> Messages;
    }
}