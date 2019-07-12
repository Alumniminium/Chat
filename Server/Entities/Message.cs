using System;
using Universal.Packets;

namespace Server.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public static Message FromMsg(MsgText msgTxt)
        {
            var channel = Oracle.GetServerChannelFromId(msgTxt.ServerId, msgTxt.ChannelId);
            var msg = new Message
            {
                Id = channel.Messages.Count + 1, AuthorId = msgTxt.AuthorId, Text = msgTxt.GetText()
            };
            return msg;
            //msg.Timestamp = msgTxt.SentTime;
        }
    }
}
