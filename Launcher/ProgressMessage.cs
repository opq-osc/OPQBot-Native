using Deserizition;
using Newtonsoft.Json;
using System.Linq;
using Launcher.Sdk.Cqp.Core;
using Launcher.Sdk.Cqp;
using Native.Tool.IniConfig;
using System.IO;
using Native.Tool.IniConfig.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Launcher
{
    public static class ProgressMessage
    {
        /// <summary>
        /// 将消息中的东西替换为CQ码
        /// </summary>
        /// <param name="message">原始消息</param>
        /// <returns></returns>
        public static string Start(ReceiveMessage message)
        {
            string result = string.Empty;
            string msg = message.CurrentPacket.Data.Content;
            switch (message.CurrentPacket.Data.MsgType)
            {
                case "AtMsg":
                    {
                        TextMessage textMessage = JsonConvert.DeserializeObject<TextMessage>(msg);
                        result = textMessage.Content;
                        GroupMemberList ls = JsonConvert.DeserializeObject<GroupMemberList>(WebAPI.GetGroupMemberList(message.CurrentPacket.Data.FromGroupId));
                        foreach (var item in textMessage.UserID)
                        {
                            GroupMemberList.Memberlist mem = ls.MemberList.Where(x => x.MemberUin == item).First();
                            foreach (var pro in mem.GetType().GetProperties())
                            {
                                if (string.IsNullOrEmpty(pro.GetValue(mem).ToString()))
                                    pro.SetValue(mem, null);
                            }
                            string originStr = "@" +(mem.AutoRemark ?? mem.GroupCard ?? mem.NickName);
                            result = result.Replace(originStr, CQApi.CQCode_At(item).ToSendString());
                        }
                        break;
                    }
                case "TextMsg":
                    result = msg;
                    break;
                case "PicMsg":
                    {
                        PicMessage picMessage = JsonConvert.DeserializeObject<PicMessage>(message.CurrentPacket.Data.Content);
                        if (!Directory.Exists("data\\image"))
                            Directory.CreateDirectory("data\\image");
                        result = picMessage.Content;
                        foreach (var item in picMessage.GroupPic)
                        {
                            string md5 = GenerateMD5(item.FileMd5).ToUpper();
                            string path = $"data\\image\\{md5}.cqimg";
                            if (!File.Exists(path))
                            {
                                IniConfig ini = new IniConfig(path);
                                ini.Object.Add(new ISection("image"));
                                ini.Object["image"]["md5"] = item.FileMd5;
                                ini.Object["image"]["size"] = item.FileSize;
                                ini.Object["image"]["url"] = item.Url;
                                ini.Save();
                            }
                            result += CQApi.CQCode_Image(md5);
                        }
                        break;
                    }
                case "VoiceMsg":
                    {
                        if (!Directory.Exists("data\\record\\"))
                            Directory.CreateDirectory("data\\record\\");
                        JObject json = JObject.Parse(msg);
                        string url = json["Url"].ToString();
                        string MD5 = GenerateMD5(url);
                        string path = "data\\record\\" + MD5 + ".silk";
                        if (!File.Exists(path))
                        {
                            IniConfig ini = new IniConfig(path);
                            ini.Object.Add(new ISection("record"));
                            ini.Object["record"]["url"] = url;
                            ini.Save();
                        }
                        result = CQApi.CQCode_Record(MD5+".silk").ToString();
                        break;
                    }
            }
            result = Regex.Replace(result, "\\[表情(\\d*)\\]", "[CQ:face,id=$1]");
            return result;
        }
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
