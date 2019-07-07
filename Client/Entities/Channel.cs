using System.Collections.Generic;

namespace Client.Entities
{
    public class Channel
    {
        public ulong Id;
        public string Name;

        public List<Message> Messages;
    }
}