using Deserizition;
using Lancher.Sdk.Cqp;
using Lancher.Sdk.Cqp.Enum;
using Native.Tool.IniConfig;
using Native.Tool.IniConfig.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using SocketIOClient.Messages;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ErrorEventArgs = SocketIOClient.ErrorEventArgs;

namespace Lancher
{
    class Program
    {
        public static Client socket;

        static void Main(string[] args)
        {
            IniConfig ini = new IniConfig("Config.ini");
            ini.Load();
            try
            {
                Save.curentQQ = ini.Object["Config"]["QQ"].GetValueOrDefault((long)0);
                Save.url = ini.Object["Config"]["url"].GetValueOrDefault("http://127.0.0.1:8888/");
            }
            catch
            {
                ini.Clear();
                ini.Object.Add(new ISection("Config"));
                ini.Object["Config"].Add("QQ", 0);
                ini.Object["Config"].Add("url", "");
                long qq;
                while (true)
                {
                    Console.Write("请输入目前服务端挂着的QQ号:");
                    if (long.TryParse(Console.ReadLine(), out qq))
                        break;
                }
                Save.curentQQ = qq;
                Console.Write("请输入用于连接服务端的url:");
                string url = Console.ReadLine();
                if (url.Reverse().First() != '/')
                    url += '/';
                ini.Object["Config"]["QQ"] = new IValue(qq);
                ini.Object["Config"]["url"] = new IValue(Save.url);
                ini.Save();
                Console.WriteLine("需要修改请在目录下的Config.ini内修改");
            }
            socket = new Client(Save.url);
            socket.Opened += SocketOpened;
            socket.Message += SocketMessage;
            socket.SocketConnectionClosed += SocketConnectionClosed;
            socket.Error += SocketError;
            string QQ = Save.curentQQ.ToString();//框架在线的QQ号
            PluginManagment pluginManagment = new PluginManagment();
            pluginManagment.Load();
            LogHelper.WriteLine("遍历启动事件……");
            pluginManagment.CallFunction("StartUp");
            pluginManagment.CallFunction("Enable");
            LogHelper.WriteLine("插件载入完成，开始连接服务器");
            socket.Connect();
            // register for 'connect' event with io server
            socket.On("connect", (fn) =>
            {
                LogHelper.WriteLine(((ConnectMessage)fn).ConnectMsg.ToString());
                //重连成功 取得 在线QQ的websocket 链接connid
                //Ack
                socket.Emit("GetWebConn", QQ, null, (callback) =>
                {
                    //var jsonMsg = callback as string;
                    //LogHelper.WriteLine(string.Format("callback [root].[messageAck]: {0} \r\n", jsonMsg));
                    LogHelper.WriteLine($"服务器连接成功，开始处理事件……");
                }
                 );
                //获取已登录QQ的群列表
                //socket.Emit("GetGroupList", QQ);

                //获取已登录QQ的好友列表
                //socket.Emit("GetQQUserList", QQ);
                //获取已登录QQ的群123456789成员列表
                // 
                //string JSON = "{" + string.Format("\"Uid\":\"{0}\",\"Group\":{1}", QQ, 123456789) + "}";
                //socket.Emit("GetGroupUserList", JSON.Replace("\"", "\\\""));//json 需要处理一下转义

                //获取登录二维码
                socket.Emit("GetQrcode", "", null, (callback) =>
                {
                    var jsonMsg = callback as string;
                    LogHelper.WriteLine(string.Format("GetQrcode From Websocket: {0} \r\n", jsonMsg));
                }
                   );
            });
            //二维码检测事件
            socket.On("OnCheckLoginQrcode", (fn) =>
            {
                LogHelper.WriteLine("OnCheckLoginQrcode\n" + ((JSONMessage)fn).MessageText);
            });
            //收到群消息的回调事件
            socket.On("OnGroupMsgs", (fn) =>
              {
                  //群文件合并到此事件
                  //{"FileID":"/9d784a08-d918-11ea-a715-5452007bdaa4","FileName":"Log.lua","FileSize":1159,"Tips":"[群文件]"}
                  //语音消息
                  //{"Tips":"[语音]","Url":"http://grouptalk.c2c.qq.com/?ver=0\u0026rkey=3062020101045b30590201010201010204b40f1411042439306a33504b526d42394567685178716f6f464c4c67676639234e5f4a42535a7734764202045f2e03de041f0000000866696c6574797065000000013100000005636f64656300000001310400\u0026filetype=1\u0026voice_codec=1"}
                  Task task = new Task(() =>
                  {
                      Stopwatch stopwatch = new Stopwatch();
                      stopwatch.Start();
                      ReceiveMessage groupMessage = JsonConvert.DeserializeObject<ReceiveMessage>(((JSONMessage)fn).MessageText);
                      string message = ProgressMessage.Start(groupMessage);
                      ReceiveMessage.Data data = groupMessage.CurrentPacket.Data;
                      if (groupMessage.CurrentPacket.Data.FromUserId == Save.curentQQ)
                      {
                          Dll.AddMsgToSave(new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message));
                          return;
                      }
                      LogHelper.WriteLine(CQLogLevel.InfoReceive, "[↓]收到消息", $"来源群:{data.FromGroupId}({data.FromGroupName}) 来源QQ:{data.FromUserId}({data.FromNickName}) {message}");
                      var c = new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message);
                      Dll.AddMsgToSave(c);
                      pluginManagment.CallFunction("GroupMsg", 2, Save.MsgList.Count + 1, data.FromGroupId, data.FromUserId,
                            "", Marshal.StringToHGlobalAnsi(message), 0);
                      stopwatch.Stop();
                      LogHelper.WriteLine($"耗时 {stopwatch.ElapsedMilliseconds} ms");
                  }); task.Start();
                  // new Event_GroupMessage().GroupMessage(new object(), e);
              });
            //收到好友消息的回调事件
            socket.On("OnFriendMsgs1", (fn) =>
            {
                Task task = new Task(() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    ReceiveMessage groupMessage = JsonConvert.DeserializeObject<ReceiveMessage>(((JSONMessage)fn).MessageText);
                    string message = ProgressMessage.Start(groupMessage);
                    ReceiveMessage.Data data = groupMessage.CurrentPacket.Data;
                    if (groupMessage.CurrentPacket.Data.FromUserId == Save.curentQQ)
                    {
                        Dll.AddMsgToSave(new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message));
                        return;
                    }
                    LogHelper.WriteLine(CQLogLevel.InfoReceive, "[↓]收到消息", $"来源群:{data.FromGroupId}({data.FromGroupName}) 来源QQ:{data.FromUserId}({data.FromNickName}) {message}");
                    var c = new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message);
                    Dll.AddMsgToSave(c);
                    pluginManagment.CallFunction("PrivateMsg", 11, Save.MsgList.Count + 1, data.FromUserId, Marshal.StringToHGlobalAnsi(message), 0);
                    stopwatch.Stop();
                    LogHelper.WriteLine($"耗时 {stopwatch.ElapsedMilliseconds} ms");
                }); task.Start();
            });
            //统一事件管理如好友进群事件 好友请求事件 退群等事件集合
            socket.On("OnEvents", (fn) =>
            {
                Task task = new Task(() =>
                {
                    JObject events = JObject.Parse(((JSONMessage)fn).MessageText);
                    Requests request = new Requests();
                    switch (events["CurrentPacket"]["Data"]["EventName"].ToString())
                    {
                        case "ON_EVENT_GROUP_JOIN"://入群事件 _eventSystem_GroupMemberIncrease id=7
                            long GroupJoin_JoinGroup, GroupJoin_JoinUin, GroupJoin_InviteUin;
                            string GroupJoin_JoinUserName;
                            JToken GroupJoin_data = events["CurrentPacket"]["Data"]["EventData"];
                            GroupJoin_JoinGroup = Convert.ToInt64(events["CurrentPacket"]["Data"]["EventMsg"]["FromUin"].ToString());
                            GroupJoin_JoinUin = Convert.ToInt64(GroupJoin_data["UserID"].ToString());
                            GroupJoin_JoinUserName = GroupJoin_data["UserName"].ToString();
                            GroupJoin_InviteUin = Convert.ToInt64(GroupJoin_data["InviteUin"].ToString());
                            if(GroupJoin_JoinUin!=Save.curentQQ)
                                pluginManagment.CallFunction("GroupMemberIncrease", 1, GetTimeStamp(), GroupJoin_JoinGroup, 0, GroupJoin_JoinUin);
                            LogHelper.WriteLine($"入群事件 群{GroupJoin_JoinGroup}加入{GroupJoin_JoinUserName}({GroupJoin_JoinUin})");
                            break;
                        case "ON_EVENT_GROUP_EXIT"://退群事件 _eventSystem_GroupMemberDecrease id=6
                            long GroupExit_FromUin, GroupExit_UserID;
                            JToken GroupExit_data = events["CurrentPacket"]["Data"];
                            GroupExit_FromUin = Convert.ToInt64(GroupExit_data["EventMsg"]["FromUin"].ToString());
                            GroupExit_UserID = Convert.ToInt64(GroupExit_data["EventData"]["UserID"].ToString());
                            pluginManagment.CallFunction("GroupMemberDecrease", 1, GetTimeStamp(), GroupExit_FromUin, 0, GroupExit_UserID);
                            LogHelper.WriteLine($"退群事件 {GroupExit_UserID}退出群{GroupExit_FromUin} ");
                            break;
                        case "ON_EVENT_GROUP_ADMIN"://群管变动事件 _eventSystem_GroupAdmin id=5
                            long GroupAdmin_GroupID, GroupAdmin_UserID, GroupAdmin_Flag;
                            JToken GroupAdmin_data = events["CurrentPacket"]["Data"]["EventData"];
                            GroupAdmin_GroupID = Convert.ToInt64(GroupAdmin_data["GroupID"].ToString());
                            GroupAdmin_UserID = Convert.ToInt64(GroupAdmin_data["UserID"].ToString());
                            GroupAdmin_Flag = Convert.ToInt64(GroupAdmin_data["Flag"].ToString());
                            pluginManagment.CallFunction("AdminChange", 1, GetTimeStamp(), GroupAdmin_GroupID, GroupAdmin_UserID);
                            LogHelper.WriteLine($"群管理员变动事件 {GroupAdmin_UserID}{(GroupAdmin_Flag == 1 ? "升为" : "消去")}群{GroupAdmin_GroupID}的管理员");
                            break;
                        case "ON_EVENT_GROUP_ADMINSYSNOTIFY"://加群请求事件 _eventRequest_AddGroup id=12
                            request.type = "GroupRequest";
                            request.json = ((JSONMessage)fn).MessageText;
                            Dll.AddRequestToSave(request);

                            long GroupRequest_GroupId, GroupRequest_InviteUin, GroupRequest_Who;
                            string GroupRequest_WhoName, GroupRequest_GroupName, GroupRequest_InviteName;
                            JToken GroupRequest_extdata = events["CurrentPacket"]["Data"]["EventData"];
                            GroupRequest_GroupId = Convert.ToInt64(GroupRequest_extdata["GroupId"].ToString());
                            GroupRequest_InviteUin = Convert.ToInt64(GroupRequest_extdata["InviteUin"].ToString());
                            GroupRequest_Who = Convert.ToInt64(GroupRequest_extdata["Who"].ToString());
                            GroupRequest_WhoName = GroupRequest_extdata["WhoName"].ToString();
                            GroupRequest_GroupName = GroupRequest_extdata["GroupName"].ToString();
                            GroupRequest_InviteName = GroupRequest_extdata["InviteName"].ToString();
                            pluginManagment.CallFunction("GroupAddRequest", 1, GetTimeStamp(), GroupRequest_GroupId, GroupRequest_Who, Marshal.StringToHGlobalAnsi(""), "");
                            LogHelper.WriteLine($"加群请求事件 {GroupRequest_WhoName}({GroupRequest_Who})" +
                                $" {(GroupRequest_InviteUin != 0 ? $"受{GroupRequest_InviteName}({GroupRequest_InviteUin})邀请" : "")}" +
                                $"加入群{GroupRequest_GroupName}({GroupRequest_GroupId})");
                            break;
                        case "ON_EVENT_FRIEND_ADD"://好友请求事件 _eventRequest_AddFriend id=11
                            request.type = "FriendRequest";
                            request.json = ((JSONMessage)fn).MessageText;
                            Dll.AddRequestToSave(request);

                            string FriendRequest_Content;
                            long FriendRequest_UserID;
                            JToken FriendRequest_data = events["CurrentPacket"]["Data"]["EventData"];
                            FriendRequest_Content = FriendRequest_data["Content"].ToString();
                            FriendRequest_UserID = Convert.ToInt64(FriendRequest_data["UserID"].ToString());
                            pluginManagment.CallFunction("FriendRequest", 1, GetTimeStamp(), FriendRequest_UserID, Marshal.StringToHGlobalAnsi(FriendRequest_Content), "");
                            LogHelper.WriteLine($"好友请求事件 QQ号:{FriendRequest_UserID} 验证信息:{FriendRequest_Content}");
                            break;
                        case "ON_EVENT_NOTIFY_PUSHADDFRD"://好友添加完成事件 _eventFriend_Add id=10
                            long FriendAdded_UserID;
                            JToken FriendAdded_data = events["CurrentPacket"]["Data"]["EventData"];
                            FriendAdded_UserID = Convert.ToInt64(FriendAdded_data["UserID"].ToString());
                            pluginManagment.CallFunction("FriendAdded", 1, GetTimeStamp(), FriendAdded_UserID);
                            LogHelper.WriteLine($"好友添加完成事件 与{FriendAdded_UserID}成为了好友");
                            break;
                        case "ON_EVENT_GROUP_SHUT"://群禁言事件 _eventSystem_GroupBan id=8
                            long GroupShut_groupid, GroupShut_qqid, GroupShut_shuttime;
                            JToken GroupShut_data = events["CurrentPacket"]["Data"]["EventData"];
                            GroupShut_groupid = Convert.ToInt64(GroupShut_data["GroupID"].ToString());
                            GroupShut_qqid = Convert.ToInt64(GroupShut_data["UserID"].ToString());
                            GroupShut_shuttime = Convert.ToInt64(GroupShut_data["ShutTime"].ToString());
                            pluginManagment.CallFunction("GroupBan", (GroupShut_shuttime == 0 ? 1 : 2), GetTimeStamp(), GroupShut_groupid, 0, GroupShut_qqid);
                            LogHelper.WriteLine($"群 {GroupShut_groupid} UserID {GroupShut_qqid} 禁言时间 {GroupShut_shuttime}秒");
                            break;
                    }
                }); task.Start();
            });
            // make the socket.io connection
            while (true)
            {
                Console.ReadLine();
            }
        }
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        static void SocketOpened(object sender, EventArgs e)
        {
            LogHelper.WriteLine("SocketOpened\r\n");
        }

        static void SocketError(object sender, ErrorEventArgs e)
        {
            LogHelper.WriteLine("socket client error:");
            LogHelper.WriteLine(e.Message);
        }

        static void SocketConnectionClosed(object sender, EventArgs e)
        {
            LogHelper.WriteLine("WebSocketConnection was terminated!");
        }

        static void SocketMessage(object sender, MessageEventArgs e)
        {
            // uncomment to show any non-registered messages
            if (string.IsNullOrEmpty(e.Message.Event))
                LogHelper.WriteLine("Generic SocketMessage: {0}", e.Message.MessageText);
            else
                LogHelper.WriteLine("Generic SocketMessage: {0} : {1}", e.Message.Event, e.Message.Json.ToJsonString());
        }
        public static void Close()
        {
            if (socket != null)
            {
                socket.Opened -= SocketOpened;
                socket.Message -= SocketMessage;
                socket.SocketConnectionClosed -= SocketConnectionClosed;
                socket.Error -= SocketError;
                socket.Dispose(); // close & dispose of socket client
            }
        }
    }
}
