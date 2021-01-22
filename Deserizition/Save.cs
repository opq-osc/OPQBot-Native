using System.Collections.Generic;
using System.Windows.Forms;


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
        /// <summary>
        /// 进程分离模式
        /// </summary>
        public static bool MutiProcessMode { get; set; } = false;
        /// <summary>
        /// 命名管道名称，用于应用进程分离
        /// </summary>
        public static string NamedPipeName { get; set; }
        public static int AuthCode { get; set; }
        public static PipeType PipeType { get; set; } = PipeType.NoPipe;
        public static int LogerMaxCount { get; set; } = 500;
        public static bool LoginStatus { get; set; } = false;
    }
    public enum PipeType
    {
        Server,
        Client,
        NoPipe
    }
}
