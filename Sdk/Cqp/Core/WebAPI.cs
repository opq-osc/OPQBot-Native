using Deserizition;
using Launcher.Sdk.Cqp.Enum;
using Jie2GG.Tool.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading;

namespace Launcher.Sdk.Cqp.Core
{
    public static class WebAPI
    {
        /// <summary>
        /// 获取群成员列表
        /// </summary>
        /// <param name="groupid">群号</param>
        /// <returns>raw json</returns>
        public static string GetGroupMemberList(long groupid)
        {
            string url = $@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetGroupUserList&timeout=10";
            JObject data = new JObject
            {
                {"GroupUin",groupid},
                {"LastUin",0}
            };
            return SendRequest(url, data.ToString());
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns>raw json</returns>
        public static string GetFriendList()
        {
            string url = $@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetQQUserList&timeout=10";
            JObject data = new JObject
            {
                {"StartIndex",0}
            };
            return SendRequest(url, data.ToString());
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="data">负载消息</param>
        /// <returns>接口返回的消息</returns>
        public static string SendRequest(string url, string data)
        {
            HttpWebClient http = new HttpWebClient
            {
                ContentType = "application/json",
                Accept = "*/*",
                KeepAlive = true,
                Method = "POST",
                Encoding = Encoding.UTF8,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.87 Safari/537.36",
                TimeOut = 10000
            };
            string tmp = http.UploadString(url, data.ToString());
            Thread.Sleep(1000);
            return tmp;
        }
        /// <summary>
        /// 发送Post请求的同时写入一条日志
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="data">负载信息</param>
        /// <param name="origin">消息来源</param>
        /// <param name="type">消息类型</param>
        /// <param name="desc">消息内容</param>
        /// <param name="level">日志等级</param>
        /// <returns>接口返回的消息</returns>
        public static string SendRequest(string url, string data, string origin, string type, string desc, CQLogLevel level)
        {
            string result = SendRequest(url, data);
            CoreHelper.LogWriter(Save.logListView, (int)level, origin, type, "...", desc);
            return result;
        }
    }
}
