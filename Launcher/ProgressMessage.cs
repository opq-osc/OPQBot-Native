using Deserizition;
using Newtonsoft.Json;
using System.Linq;
using Launcher.Sdk.Cqp.Core;
using Launcher.Sdk.Cqp;
using Jie2GG.Tool.IniConfig;
using System.IO;
using Jie2GG.Tool.IniConfig.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace Launcher
{
    public static class ProgressMessage
    {
        public static List<GroupMemberList> MemberSave = new List<GroupMemberList>();
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
                case "TempSessionMsg":
                    if(msg.Contains("图片"))
                    {
                        var c = JsonConvert.DeserializeObject<PicMessage>(msg).FriendPic;
                        foreach(var item in c)
                        {
                            result += MakeCQImage(item);
                        }
                    }
                    else
                    {
                        result = JObject.Parse(msg)["Content"].ToString();
                    }
                    break;
                case "AtMsg":
                    {
                        //at消息主要将消息中的at消息转变为CQ码
                        //而@人名 中的人名格式可能会不同,不能直接用群名片替换,所以要从群成员列表寻找这个人
                        //按备注->群名片->昵称的顺序,替换可能出现的名称
                        TextMessage textMessage = JsonConvert.DeserializeObject<TextMessage>(msg);
                        result = textMessage.Content;
                        //从缓存寻找这个群
                        GroupMemberList ls = MemberSave.Find(x => x.GroupUin == message.CurrentPacket.Data.FromGroupId);
                        if (ls == null)//未在缓存找到,将这个群加入缓存
                        {
                            ls = JsonConvert.DeserializeObject<GroupMemberList>(WebAPI.GetGroupMemberList(message.CurrentPacket.Data.FromGroupId));
                            MemberSave.Add(ls);
                        }
                        foreach (var item in textMessage.UserID)
                        {
                            GroupMemberList.Memberlist mem = ls.MemberList.Where(x => x.MemberUin == item).First();
                            foreach (var pro in mem.GetType().GetProperties())
                            {
                                //将空文本变成null,方便后续??运算符
                                try
                                {
                                    if (string.IsNullOrEmpty(pro.GetValue(mem).ToString()))
                                        pro.SetValue(mem, null);
                                }
                                catch (NullReferenceException e)
                                {
                                    pro.SetValue(mem, null);//如果是null则会跳至catch块
                                }
                            }
                            string originStr = "@" + (mem.AutoRemark ?? mem.GroupCard ?? mem.NickName);
                            result = result.Replace(originStr, CQApi.CQCode_At(item).ToSendString());
                        }
                        break;
                    }
                case "TextMsg":
                    result = msg;
                    break;
                case "PicMsg":
                    {
                        //图片消息是将图片消息的信息配置进image文件夹下的以MD5为名称的cqimg文件内
                        PicMessage picMessage = JsonConvert.DeserializeObject<PicMessage>(message.CurrentPacket.Data.Content);
                        if (!Directory.Exists("data\\image"))
                            Directory.CreateDirectory("data\\image");
                        result = picMessage.Content;
                        if (picMessage.GroupPic != null)//是群图片消息
                        {
                            foreach (var item in picMessage.GroupPic)
                            {
                                result += MakeCQImage(item);
                            }
                        }
                        else//是好友图片消息
                        {
                            foreach (var item in picMessage.FriendPic)
                            {
                                result += MakeCQImage(item);
                            }
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
                        result = CQApi.CQCode_Record(MD5 + ".silk").ToString();
                        break;
                    }
            }
            result = Regex.Replace(result, "\\[表情(\\d*)\\]", "[CQ:face,id=$1]");//处理QQ表情信息
            //处理emoji消息
            foreach (var a in result)
            {
                //UTF-8下，大部分的emoji都是以\ud83d开头
                if (a == '\ud83d' && result.IndexOf(a) != result.Length - 1)
                {
                    //取这个emoji
                    string str = a.ToString() + result[result.IndexOf(a) + 1].ToString();
                    UTF32Encoding enc = new UTF32Encoding(true, false);
                    byte[] bytes = enc.GetBytes(str);//转换字节数组
                    //使用BitConvert将字节数组转换为16进制，之后转换为10进制即可
                    result = result.Replace(str, CQApi.CQCode_Emoji(Convert.ToInt32(BitConverter.ToString(bytes).Replace("-", ""), 16)).ToString());
                    break;
                }
            }
            return result;
        }
        private static string MakeCQImage(PicMessage.Friendpic item)
        {
            return MakeCQImage(new PicMessage.Grouppic
            { 
                FileMd5 = item.FileMd5, 
                FileSize = item.FileSize, 
                Url = item.Url 
            });
        }
        private static string MakeCQImage(PicMessage.Grouppic item)
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
            return CQApi.CQCode_Image(md5).ToSendString();
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
