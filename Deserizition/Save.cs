using System.Collections.Generic;
using System.Windows.Forms;


namespace Deserizition
{
    public class Message
    {
        public int MsgId;
        public long MsgRandom;
        public int Seq;
        public long userID;
        public long groupid;
        public long time;
        public string text;
        public long tempGroupID;

        public Message(int msgId, long msgRandom, int seq, long userID, long groupid, long time, string text, long tempGroupID)
        {
            MsgId = msgId;
            MsgRandom = msgRandom;
            Seq = seq;
            this.userID = userID;
            this.groupid = groupid;
            this.time = time;
            this.text = text;
            this.tempGroupID = tempGroupID;
        }
    }
    public class Requests
    {
        public string type;
        public string json;
    }
    public static class Save
    {
        /// <summary>
        /// 连接服务端的网址
        /// </summary>
        public static string url { get; set; }
        /// <summary>
        /// 需要接收消息的QQ号
        /// </summary>
        public static long curentQQ { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        public static List<Message> MsgList { get; set; } = new List<Message>();
        /// <summary>
        /// QQ昵称
        /// </summary>
        public static string name { get; set; }
        /// <summary>
        /// 日志列表的自动滚动状态
        /// </summary>
        public static bool AutoScroll { get; set; } = true;
        /// <summary>
        /// 受测试的插件名称列表, 用于消息模拟器
        /// </summary>
        public static List<string> TestPluginsList { get; set; } = new List<string>();
        /// <summary>
        /// 用于测试插件与框架交互的文本框
        /// </summary>
        public static RichTextBox TestPluginChatter { get; set; }
        /// <summary>
        /// 忽略进程重复检查
        /// </summary>
        public static bool IgnoreProcessChecking { get; set; } = false;
        public static int LogerMaxCount { get; set; } = 500;
        public static int BoardCastPort { get; set; } = 28634;
        public static bool LoginStatus { get; set; } = false;
        /// <summary>
        /// 网络重连次数
        /// </summary>
        public static int TryCount { get; set; } = 0;
        public static FriendsList FriendsList { get; set; } = new FriendsList();
        public static GroupList GroupList { get; set; } = new GroupList();
        public static GroupMemberList GroupMemberList { get; set; } = new GroupMemberList();
        public static bool ReceiveSelfMsg { get; set; } = true;
    }
}
