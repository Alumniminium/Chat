using Universal.Packets;

namespace Client.Entities
{
    public class Message
    {
        public int Id;
        public int AuthorId;
        public string Text;
        public long Timestamp;

        public static Message CreateFromMsg(MsgText msgTxt)
        {
            var msg = new Message();
            msg.Id = msgTxt.UniqueId;
            msg.AuthorId = msgTxt.AuthorId;
            msg.Text = msgTxt.GetText();
            msg.Timestamp = msgTxt.SentTime;

            return msg;
        }
    }
}