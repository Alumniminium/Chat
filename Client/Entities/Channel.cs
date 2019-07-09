using System;
using System.Collections.Generic;
using Universal.Packets;

namespace Client.Entities
{
    public class Channel
    {
        public int UniqueId { get; private set; }
        public string Name { get; private set; }
        private List<Message> Messages { get; set; }

        public Action<Message> OnMessage { get; set; }

        public static Channel FromMsg(MsgChannel msg)
        {
            var channel = new Channel(msg.UniqueId, msg.GetName());
            return channel;
        }

        public Channel(int id, string name)
        {
            UniqueId = id;
            Name = name;
            Messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
            OnMessage?.Invoke(message);
        }
    }
}