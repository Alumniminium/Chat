using System;
using System.Collections.Generic;
using Universal.Packets;

namespace Client.Entities
{
    public class Channel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Dictionary<int,Message> Messages { get; set; }

        public Action<Message> OnMessage { get; set; }

        public static Channel FromMsg(MsgChannel msg)
        {
            var channel = new Channel(msg.UniqueId, msg.GetName());
            return channel;
        }

        public Channel(int id, string name)
        {
            Id = id;
            Name = name;
            Messages = new Dictionary<int, Message>();
        }

        public void AddMessage(Message message)
        {
            Messages.Add(message.Id, message);
            OnMessage?.Invoke(message);
        }
    }
}