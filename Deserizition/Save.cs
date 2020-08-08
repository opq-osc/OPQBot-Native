using System.Collections.Generic;

namespace Deserizition
{
    public class Message
    {
        public int MsgId;
        public long MsgRandom;
        public int Seq;
        public long groupid;
        public long time;
        public string text;

        public Message(int msgId, long msgRandom, int seq, long groupid, long time, string text)
        {
            this.MsgId = msgId;
            this.MsgRandom = msgRandom;
            this.Seq = seq;
            this.groupid = groupid;
            this.time = time;
            this.text = text;
        }
    }
    public class Requests
    {
        public string type;
        public string json;
    }
    public static class Save
    {
        public static string url { get; set; }
        public static long curentQQ { get; set; }
        public static List<Message> MsgList { get; set; } = new List<Message>();
    }
}
