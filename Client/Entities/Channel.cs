using System.Collections.Generic;

namespace Client.Entities
{
    public class Channel
    {
        public int Id;
        public string Name;

        public List<Message> Messages;

        public Channel()
        {
            Messages=new List<Message>();
        }
    }
}