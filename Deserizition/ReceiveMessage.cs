
namespace Deserizition
{
    public class ReceiveMessage
    {
        public Currentpacket CurrentPacket { get; set; }
        public long CurrentQQ { get; set; }

        public class Currentpacket
        {
            public string WebConnId { get; set; }
            public Data Data { get; set; }
        }

        public class Data
        {
            public long FromGroupId { get; set; }
            public string FromGroupName { get; set; }
            public long FromUserId { get; set; }
            public string FromNickName { get; set; }
            public string Content { get; set; }
            public string MsgType { get; set; }
            public long MsgTime { get; set; }
            public int MsgSeq { get; set; }
            public long MsgRandom { get; set; }
            public object RedBaginfo { get; set; }
            public long FromUin { get; set; }
            public long ToUin { get; set; }
        }
    }
}
