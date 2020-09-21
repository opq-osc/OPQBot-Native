using Deserizition;
using Launcher.Sdk.Cqp.Enum;
using Launcher.Sdk.Cqp.Expand;
using Launcher.Sdk.Cqp.Model;
using Native.Tool.Http;
using Native.Tool.IniConfig;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Launcher.Sdk.Cqp.Core
{
    public static class WebAPI
    {
        public static int CQ_sendGroupMsg(int authcode, long groupid, IntPtr msg)
        {
            string text = Marshal.PtrToStringAnsi(msg);
            string url = $@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=SendMsg&timeout=10";
            List<CQCode> cqCodeList = CQCode.Parse(text);
            JObject data = new JObject
            {
                {"toUser",groupid},
                {"sendToType",2},
                {"groupid",0},
                {"fileMd5","" }
            };
            bool Picflag = false, Atflag = false; ;
            foreach (var item in cqCodeList)
            {
                switch (item.Function)
                {
                    case CQFunction.At://[CQ:at,qq=xxxx]
                        {
                            if (!data.ContainsKey("atUser"))
                            {
                                data.Add("atUser", Convert.ToInt64(item.Items["qq"]));
                            }
                            else if (data["atUser"].ToString() == "0")
                                data["atUser"] = Convert.ToInt64(item.Items["qq"]);
                            Atflag = true;
                            break;
                        }
                    case CQFunction.Image:
                        {
                            if (!data.ContainsKey("content")) data.Add("content", "");
                            if (!data.ContainsKey("picBase64Buf")) data.Add("picBase64Buf", "");
                            if (!data.ContainsKey("picUrl")) data.Add("picUrl", "");
                            if (!data.ContainsKey("atUser")) data.Add("atUser", 0);
                            if (!data.ContainsKey("picUrl")) data.Add("picUrl", "");
                            if (item.Items.ContainsKey("url"))
                                data["picUrl"] = item.Items["url"];
                            else if (item.Items.ContainsKey("file"))
                            {
                                if (File.Exists("data\\image\\" + item.Items["file"] + ".cqimg"))
                                {
                                    IniConfig ini = new IniConfig("data\\image\\" + item.Items["file"] + ".cqimg"); ini.Load();
                                    data["picUrl"] = ini.Object["image"]["url"].ToString();
                                    Picflag = true;
                                    break;
                                }
                                string path = item.Items["file"], base64buf = string.Empty;
                                if (File.Exists(path))
                                {
                                    base64buf = BinaryReaderExpand.ImageToBase64(path);
                                }
                                data["picBase64Buf"] = base64buf;
                            }
                            else if (item.Items.ContainsKey("md5"))
                            {
                                data["fileMd5"] = item.Items["file"];
                            }
                            Picflag = true;
                            break;
                        }
                }
            }
            string filtered = Regex.Replace(text, @"\[CQ.*\]", "");
            if (!string.IsNullOrEmpty(filtered)) data["content"] = filtered;
            if (Picflag)
                data.Add("sendMsgType", "PicMsg");
            else if (Atflag)
                data.Add("sendMsgType", "AtMsg");
            else
                data.Add("sendMsgType", "TextMsg");
            if (!data.ContainsKey("atUser")) data.Add("atUser", 0);
            Console.WriteLine($"发送消息,群号{groupid}");
            Console.WriteLine(msg);
            SendRequest(url, data.ToString());
            return 0;
        }

        public static string GetGroupMemberList(long groupid)
        {
            string url = $@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetGroupUserList&timeout=10";
            JObject data = new JObject
            {
                {"GroupUin",groupid},
                {"LastUin",0},
            };
            return SendRequest(url, data.ToString());
        }
        public static string GetFriendList()
        {
            string url = $@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetQQUserList&timeout=10";
            JObject data = new JObject
            {
                {"StartIndex",0}
            };
            return SendRequest(url, data.ToString());
        }
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
        public static string SendRequest(string url,string data,string origin,string type,string desc,CQLogLevel level)
        {
            string result = SendRequest(url, data);
            CoreHelper.LogWriter(Save.logListView,(int)level,origin,type,"...",desc);
            return result;
        }
    }
}
