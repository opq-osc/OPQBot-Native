﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deserizition;
using Jie2GG.Tool.Http;
using Launcher.Sdk.Cqp.Expand;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;

namespace Launcher.Forms
{
    public partial class MainForm : Form
    {
        #region --公开成员--
        /// <summary>
        /// 显示窗口标志
        /// </summary>
        public bool ShowFlag = true;
        /// <summary>
        /// 窗口置顶标志
        /// </summary>
        public bool TopFlag = true;
        /// <summary>
        /// 公有 与服务器连接 SocketIO对象
        /// </summary>
        public SocketIO socket;
        #endregion
        #region --静态私有成员--
        /// <summary>
        /// 用于本类中对日志窗口的操作的 窗口对象
        /// </summary>
        static LogForm logForm = new LogForm();
        /// <summary>
        /// 标志窗口是否载入完成的变量
        /// </summary>
        static bool Loaded = false;
        /// <summary>
        /// 置顶维护时钟
        /// </summary>
        static System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
        static Encoding GB18030 = Encoding.GetEncoding("GB18030");
        #endregion
        #region --静态公开成员--
        /// <summary>
        /// 公开窗口对象
        /// </summary>
        public static MainForm FormBackup;
        /// <summary>
        /// 插件管理模块
        /// </summary>
        public static PluginManagment pluginManagment;
        /// <summary>
        /// 程序的主设置,包括插件的开启状态以及窗口位置 是否显示窗口 窗口是否置顶 配置
        /// </summary>
        public static JObject AppConfig = new JObject();
        #endregion
        public MainForm()
        {
            InitializeComponent();
        }
        #region 拖动无窗体的控件
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        {
            FormBackup = this;
            Save.FriendsList = JsonConvert.DeserializeObject<FriendsList>(SendRequest($@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetQQUserList&timeout=10", "{\"StartIndex\":0}"));
            Save.GroupList = JsonConvert.DeserializeObject<GroupList>(SendRequest($@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetGroupList&timeout=10", "{\"NextToken\":\"\"}"));

            Thread getGroupInfo_Thread = new Thread(()=>
            {
                foreach(var item in Save.GroupList.TroopList)
                {
                    item.GroupMemberList = JsonConvert.DeserializeObject<GroupMemberList>(SendRequest($@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetGroupUserList&timeout=10", $"{{\"GroupUin\":{item.GroupId},\"LastUin\":0}}"));
                }
            });
            getGroupInfo_Thread.Start();

            //根据登录时的QQ号 获取QQ昵称
            string name = Save.FriendsList.Friendlist.FirstOrDefault(x => x.FriendUin== Save.curentQQ)?.NickName.ToString();
            Save.name = name;
            if (!Directory.Exists("conf"))
            {
                Directory.CreateDirectory("conf");
            }
            if (File.Exists(LogHelper.GetLogFilePath()) is false)
            {
                LogHelper.CreateDB();
            }
            //读取窗口相关配置
            if (File.Exists(@"conf\states.json"))
            {
                AppConfig = JObject.Parse(File.ReadAllText(@"conf\states.json"));
            }
            //配置文件为空或不包含配置相关键
            if (AppConfig.Count == 0 || !AppConfig.ContainsKey("Main"))
            {
                JObject config = new JObject
                {
                    new JProperty("FormX",Left),
                    new JProperty("FormY",Top),
                    new JProperty("ShowWindow",true),
                    new JProperty("TopMost",true),
                    new JProperty("ExpandWindow",false)
                };
                AppConfig.Add(new JProperty("Main", config));
                File.WriteAllText(@"conf\states.json", AppConfig.ToString());
            }
            else//存在并读取配置
            {
                Left = Convert.ToInt32(AppConfig["Main"]["FormX"].ToString());
                Top = Convert.ToInt32(AppConfig["Main"]["FormY"].ToString());
                //不直接给Visable赋值是因为外部调用Show函数会覆盖对Visable的赋值
                //所以在调用Show之后需要用标志位恢复对Visable的变化值
                ShowFlag = bool.Parse(AppConfig["Main"]["ShowWindow"].ToString());
                TopFlag = bool.Parse(AppConfig["Main"]["TopMost"].ToString());
                ExpandWindow(bool.Parse(AppConfig["Main"]["ExpandWindow"].ToString()));
            }
            //设置窗口透明色, 实现窗口背景透明
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
            //隐藏日志窗口, 但不关闭
            logForm.Text = $"运行日志 - {Save.curentQQ}";
            logForm.Show();
            logForm.Visible = false;
            //初始化托盘
            NotifyIconHelper._NotifyIcon = notifyIcon;
            NotifyIconHelper.Init();
            NotifyIconHelper.ShowNotifyIcon();
            //载入插件
            pluginManagment = new PluginManagment();
            Thread thread = new Thread(() =>
            {
                pluginManagment.Init();
            });
            thread.Start();
            //将托盘右键菜单复制一份
            pictureBox_Main.ContextMenu = notifyIcon.ContextMenu;
            //实例化圆形图片框, 实现圆形的头像
            HttpWebClient http = new HttpWebClient() { TimeOut = 3000 };
            byte[] data = { };
            //默认头像,防止网络问题造成空头像出现
            Image image = Image.FromFile(@"conf\DefaultPic.jpeg");
            try
            {
                data = http.DownloadData($"http://q1.qlogo.cn/g?b=qq&nk={Save.curentQQ}&s=640");
                MemoryStream ms = new MemoryStream(data);
                if (ms.Length > 0)//下载成功
                    image = Image.FromStream(ms);
                else//下载失败
                    image = Image.FromFile(@"conf\DefaultPic.jpeg");
            }
            catch
            {
                //网络异常,图片使用默认头像
                LogHelper.WriteLog("下载头像超时，重新启动程序可能解决这个问题");
            }

            RoundPictureBox RoundpictureBox = new RoundPictureBox
            {
                Size = new Size(43, 43),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Left = -1,
                Top = 0,
                ContextMenu = notifyIcon.ContextMenu
            };
            if (image != null)
                RoundpictureBox.Image = image;
            //添加拖动事件
            RoundpictureBox.MouseDown += pictureBox_Main_MouseDown;
            //显示控件, 置顶
            this.Controls.Add(RoundpictureBox);
            RoundpictureBox.BringToFront();
            RoundpictureBox.MouseDoubleClick += RoundpictureBox_MouseDoubleClick;
            Loaded = true;
            Save.LoginStatus = true;
            //事件处理
            SocketHandler();
            //置顶维护时钟,每60秒将窗口重新置顶
            Timer.Interval = 60000;
            Timer.Tick += CheckTopMost;
            Timer.Start();
        }

        private void ExpandWindow(bool flag)
        {
            if (flag)
            {
                Size = new Size(44, 44);
                pictureBox_Main.Visible = false;
            }
            else
            {
                Size = new Size(102, 44);
                pictureBox_Main.Visible = true;
            }
        }

        private void RoundpictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExpandWindow(pictureBox_Main.Visible);
            AppConfig["Main"]["ExpandWindow"] = !pictureBox_Main.Visible;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }

        /// <summary>
        /// 窗口置顶维护事件
        /// </summary>
        private void CheckTopMost(object sender, EventArgs e)
        {
            bool flag = NotifyIconHelper._NotifyIcon.ContextMenu.MenuItems[6].Checked;
            //获取窗口是否需要置顶
            if (flag is true)
            {
                FormBackup.TopMost = false;
                Thread.Sleep(100);
                FormBackup.TopMost = true;
            }
        }
        /// <summary>
        /// 拖动窗体
        /// </summary>
        private void pictureBox_Main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            }
        }
        /// <summary>
        /// 圆形图片框, 来自CSDN
        /// </summary>
        public class RoundPictureBox : PictureBox
        {
            protected override void OnCreateControl()
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(this.ClientRectangle);
                Region region = new Region(gp);
                this.Region = region;
                base.OnCreateControl();
            }
        }
        public static void CallLogForm()
        {
            logForm.Visible = true;
            logForm.Show();
            //将窗口从后台抬出
            logForm.TopMost = false;
            logForm.TopMost = true;
            logForm.TopMost = logForm.formTopMost;
        }
        /// <summary>
        /// 发送请求子事件
        /// </summary>
        /// <param name="url">发送请求的URL</param>
        /// <param name="data">负载</param>
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
            return http.UploadString(url, data.ToString());
        }
        /// <summary>
        /// 分发获取到的消息到相对于的事件
        /// </summary>
        void SocketHandler()
        {
            //收到群消息的回调事件
            socket.On("OnGroupMsgs", (fn) =>
            {
                Save.TryCount = 0;
                GroupMessageHandler(fn.GetValue<object>().ToString());
            });
            //收到好友消息的回调事件
            socket.On("OnFriendMsgs", (fn) =>
            {
                Save.TryCount = 0;
                FriendMessageHandler(fn.GetValue<object>().ToString());
            });
            //统一事件管理如好友进群事件 好友请求事件 退群等事件集合
            socket.On("OnEvents", (fn) =>
            {
                OnEventsHandler(fn.GetValue<object>().ToString());
            });
        }
        /// <summary>
        /// 私聊消息处理事件
        /// </summary>
        private static void FriendMessageHandler(string fn)
        {
            Task task = new Task(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ReceiveMessage PrivateMessage = JsonConvert.DeserializeObject<ReceiveMessage>(fn);
                string message = ProgressMessage.Start(PrivateMessage);
                var b = Encoding.UTF8.GetBytes(message);
                message = GB18030.GetString(Encoding.Convert(Encoding.UTF8, GB18030, b));
                int msgID = PrivateMessage.CurrentPacket.Data.MsgSeq;

                ReceiveMessage.Data data = PrivateMessage.CurrentPacket.Data;
                if (PrivateMessage.CurrentPacket.Data.FromUin == Save.curentQQ && !Save.ReceiveSelfMsg)
                {
                    Dll.AddMsgToSave(new Deserizition.Message(msgID, data.MsgRandom, data.MsgSeq, data.FromUin, data.FromGroupId, data.MsgTime, message, data.TempUin));
                    return;
                }
                int logid = LogHelper.WriteLog(LogLevel.InfoReceive, "OPQBot框架", "[↓]收到好友消息", $"QQ:{data.FromUin} {message}", "处理中...");
                var c = new Deserizition.Message(msgID, data.MsgRandom, data.MsgSeq, data.FromUin, data.FromGroupId, data.MsgTime, message, data.TempUin);
                Dll.AddMsgToSave(c);
                int pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.PrivateMsg, 11, msgID, data.FromUin, Marshal.StringToHGlobalAnsi(message), 0);
                stopwatch.Stop();
                string updatemsg = $"√ {stopwatch.ElapsedMilliseconds / (double)1000:f2} s";
                if (pluginid > 0)
                {
                    updatemsg += $"(由 {pluginManagment.Plugins[pluginid - 1].appinfo.Name} 结束消息处理)";
                }
                LogHelper.UpdateLogStatus(logid, updatemsg);
            }); task.Start();
        }
        /// <summary>
        /// 群消息处理事件
        /// </summary>
        private static void GroupMessageHandler(string fn)
        {
            Task task = new Task(() =>
            {
                string msg = fn;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ReceiveMessage groupMessage = JsonConvert.DeserializeObject<ReceiveMessage>(msg);
                ReceiveMessage.Data data = groupMessage.CurrentPacket.Data;
                //群文件事件
                if (groupMessage.CurrentPacket.Data.MsgType == "GroupFileMsg")
                {
                    JObject fileupload = JObject.Parse(data.Content);
                    MemoryStream stream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(stream);
                    BinaryWriterExpand.Write_Ex(binaryWriter, fileupload["FileID"].ToString());
                    BinaryWriterExpand.Write_Ex(binaryWriter, fileupload["FileName"].ToString());
                    BinaryWriterExpand.Write_Ex(binaryWriter, Convert.ToInt64(fileupload["FileSize"].ToString()));
                    BinaryWriterExpand.Write_Ex(binaryWriter, 0);
                    pluginManagment.CallFunction(FunctionEnums.Functions.Upload, 1, GetTimeStamp(), data.FromGroupId,
                                                 data.FromUserId, Convert.ToBase64String(stream.ToArray()));
                    stopwatch.Stop();
                    LogHelper.WriteLog(LogLevel.InfoReceive, "OPQBot框架", "文件上传", $"来源群:{data.FromGroupId}({data.FromGroupName}) 来源QQ:{data.FromUserId}({data.FromNickName}) " +
                        $"文件名:{fileupload["FileName"]} 大小:{Convert.ToDouble(fileupload["FileSize"]) / 1000}KB FileID:{fileupload["FileID"]}", $"√ {stopwatch.ElapsedMilliseconds / (double)1000:f2} s");
                    return;
                }
                string message = ProgressMessage.Start(groupMessage);
                var b = Encoding.UTF8.GetBytes(message);
                message = GB18030.GetString(Encoding.Convert(Encoding.UTF8, GB18030, b));

                //表示自己发送出去的消息, 写入消息列表
                int msgID = groupMessage.CurrentPacket.Data.MsgSeq;
                if (groupMessage.CurrentPacket.Data.FromUserId == Save.curentQQ && !Save.ReceiveSelfMsg)
                {
                    Dll.AddMsgToSave(new Deserizition.Message(msgID, data.MsgRandom, data.MsgSeq, data.FromUin, data.FromGroupId, data.MsgTime, message, data.TempUin));
                    return;
                }
                int logid = LogHelper.WriteLog(LogLevel.InfoReceive, "OPQBot框架", "[↓]收到消息", $"群:{data.FromGroupId}({data.FromGroupName}) QQ:{data.FromUserId}({data.FromNickName}) {message}", "处理中...");
                var c = new Deserizition.Message(msgID, data.MsgRandom, data.MsgSeq, data.FromUin, data.FromGroupId, data.MsgTime, message, data.TempUin);
                Dll.AddMsgToSave(c);//保存消息到消息列表
                byte[] messageBytes = GB18030.GetBytes(message + "\0");
                var messageIntptr = Marshal.AllocHGlobal(messageBytes.Length);
                Marshal.Copy(messageBytes, 0, messageIntptr, messageBytes.Length);
                //调用插件功能
                int pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.GroupMsg, 2, msgID, data.FromGroupId, data.FromUserId,
                      "", messageIntptr, 0);
                Marshal.FreeHGlobal(messageIntptr);
                GC.Collect();
                stopwatch.Stop();
                string updatemsg = $"√ {stopwatch.ElapsedMilliseconds / (double)1000:f2} s";
                if (pluginid > 0)
                {
                    updatemsg += $"(由 {pluginManagment.Plugins[pluginid - 1].appinfo.Name} 结束消息处理)";
                }
                LogHelper.UpdateLogStatus(logid, updatemsg);
            }); task.Start();
        }
        /// <summary>
        /// 消息事件之外的事件处理
        /// </summary>
        private static void OnEventsHandler(string fn)
        {
            Task task = new Task(() =>
            {
                JObject events = JObject.Parse(fn);
                Requests request = new Requests();
                Stopwatch sw = new Stopwatch();
                int pluginid = 0;
                string logMsg = string.Empty;
                sw.Start();
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
                        if (GroupJoin_JoinUin != Save.curentQQ)
                            pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.GroupMemberIncrease, 1, GetTimeStamp(), GroupJoin_JoinGroup, 10000, GroupJoin_JoinUin);
                        logMsg = $"入群事件 群{GroupJoin_JoinGroup}加入{GroupJoin_JoinUserName}({GroupJoin_JoinUin})";
                        break;
                    case "ON_EVENT_GROUP_EXIT"://退群事件 _eventSystem_GroupMemberDecrease id=6
                        long GroupExit_FromUin, GroupExit_UserID;
                        JToken GroupExit_data = events["CurrentPacket"]["Data"];
                        GroupExit_FromUin = Convert.ToInt64(GroupExit_data["EventMsg"]["FromUin"].ToString());
                        GroupExit_UserID = Convert.ToInt64(GroupExit_data["EventData"]["UserID"].ToString());
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.GroupMemberDecrease, 1, GetTimeStamp(), GroupExit_FromUin, 10000, GroupExit_UserID);
                        logMsg = $"退群事件 {GroupExit_UserID}退出群{GroupExit_FromUin}";
                        break;
                    case "ON_EVENT_GROUP_ADMIN"://群管变动事件 _eventSystem_GroupAdmin id=5
                        long GroupAdmin_GroupID, GroupAdmin_UserID, GroupAdmin_Flag;
                        JToken GroupAdmin_data = events["CurrentPacket"]["Data"]["EventData"];
                        GroupAdmin_GroupID = Convert.ToInt64(GroupAdmin_data["GroupID"].ToString());
                        GroupAdmin_UserID = Convert.ToInt64(GroupAdmin_data["UserID"].ToString());
                        GroupAdmin_Flag = Convert.ToInt64(GroupAdmin_data["Flag"].ToString());
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.AdminChange, 1, GetTimeStamp(), GroupAdmin_GroupID, GroupAdmin_UserID);
                        logMsg = $"群管理员变动事件 {GroupAdmin_UserID}{(GroupAdmin_Flag == 1 ? "升为" : "消去")}群{GroupAdmin_GroupID}的管理员";
                        break;
                    case "ON_EVENT_GROUP_ADMINSYSNOTIFY"://加群请求事件 _eventRequest_AddGroup id=12
                        request.type = "GroupRequest";
                        request.json = fn;
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
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.GroupAddRequest, 1, GetTimeStamp(), GroupRequest_GroupId, GroupRequest_Who, Marshal.StringToHGlobalAnsi(""), "");
                        logMsg = $"加群请求事件 {GroupRequest_WhoName}({GroupRequest_Who})" +
                            $" {(GroupRequest_InviteUin != 0 ? $"受{GroupRequest_InviteName}({GroupRequest_InviteUin})邀请" : "")}" +
                            $"加入群{GroupRequest_GroupName}({GroupRequest_GroupId})";
                        break;
                    case "ON_EVENT_FRIEND_ADD"://好友请求事件 _eventRequest_AddFriend id=11
                        request.type = "FriendRequest";
                        request.json = fn;
                        Dll.AddRequestToSave(request);

                        string FriendRequest_Content;
                        long FriendRequest_UserID;
                        JToken FriendRequest_data = events["CurrentPacket"]["Data"]["EventData"];
                        FriendRequest_Content = FriendRequest_data["Content"].ToString();
                        FriendRequest_UserID = Convert.ToInt64(FriendRequest_data["UserID"].ToString());
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.FriendRequest, 1, GetTimeStamp(), FriendRequest_UserID, Marshal.StringToHGlobalAnsi(FriendRequest_Content), "");
                        logMsg = $"好友请求事件 QQ号:{FriendRequest_UserID} 验证信息:{FriendRequest_Content}";
                        break;
                    case "ON_EVENT_NOTIFY_PUSHADDFRD"://好友添加完成事件 _eventFriend_Add id=10
                        long FriendAdded_UserID;
                        JToken FriendAdded_data = events["CurrentPacket"]["Data"]["EventData"];
                        FriendAdded_UserID = Convert.ToInt64(FriendAdded_data["UserID"].ToString());
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.FriendAdded, 1, GetTimeStamp(), FriendAdded_UserID);
                        logMsg = $"好友添加完成事件 与{FriendAdded_UserID}成为了好友";
                        break;
                    case "ON_EVENT_GROUP_SHUT"://群禁言事件 _eventSystem_GroupBan id=8
                        long GroupShut_groupid, GroupShut_qqid, GroupShut_shuttime;
                        JToken GroupShut_data = events["CurrentPacket"]["Data"]["EventData"];
                        GroupShut_groupid = Convert.ToInt64(GroupShut_data["GroupID"].ToString());
                        GroupShut_qqid = Convert.ToInt64(GroupShut_data["UserID"].ToString());
                        GroupShut_shuttime = Convert.ToInt64(GroupShut_data["ShutTime"].ToString());
                        pluginid = pluginManagment.CallFunction(FunctionEnums.Functions.GroupBan, (GroupShut_shuttime == 0 ? 1 : 2)
                            , GetTimeStamp(), GroupShut_groupid, 10001, GroupShut_qqid, GroupShut_shuttime);
                        logMsg = $"群 {GroupShut_groupid} UserID {GroupShut_qqid} 禁言时间 {GroupShut_shuttime}秒";
                        break;
                    case "ON_EVENT_QQ_NETWORK_CHANGE":
                        Save.TryCount++;
                        logMsg = $"与服务器连接断开，第 {Save.TryCount} 次尝试重连";
                        break;
                    case "ON_EVENT_GROUP_REVOKE":
                        long GroupID, QQID, MsgID;
                        GroupID = Convert.ToInt64(events["CurrentPacket"]["Data"]["EventData"]["GroupID"].ToString());
                        QQID = Convert.ToInt64(events["CurrentPacket"]["Data"]["EventData"]["UserID"].ToString());
                        MsgID = Convert.ToInt64(events["CurrentPacket"]["Data"]["EventData"]["MsgSeq"].ToString());
                        string msg = Save.MsgList.Find(x => x.MsgId == MsgID)?.text;
                        if (string.IsNullOrWhiteSpace(msg))
                        {
                            logMsg = $"群 {GroupID} 成员 {QQID} 撤回了一条消息，消息ID：{MsgID}，消息内容捕获失败";
                            break;
                        }
                        logMsg = $"群 {GroupID} 成员 {QQID} 撤回了一条消息，消息ID：{MsgID}，内容：{msg}";
                        break;
                }
                sw.Stop();
                string updatemsg = $"√ {sw.ElapsedMilliseconds / (double)1000:f2} s";
                if (pluginid > 0)
                {
                    updatemsg += $"(由 {pluginManagment.Plugins[pluginid - 1].appinfo.Name} 结束消息处理)";
                }
                if (string.IsNullOrWhiteSpace(logMsg) is false)
                    LogHelper.WriteLog(logMsg, updatemsg);
                else
                    LogHelper.WriteLog(LogLevel.Debug, "待捕获的事件", events.ToString());
            }); task.Start();
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        /// <summary>
        /// 窗体移动事件,将位置写入配置
        /// </summary>
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (Loaded is false)
                return;
            AppConfig["Main"]["FormX"] = Left;
            AppConfig["Main"]["FormY"] = Top;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }

        #region --托盘按钮对应窗口事件--
        public static void HideWindow()
        {
            FormBackup.Visible = false;
            AppConfig["Main"]["ShowWindow"] = false;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }
        public static void ShowWindow()
        {
            FormBackup.Visible = true;
            AppConfig["Main"]["ShowWindow"] = true;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }
        public static void TopMost_Enable()
        {
            FormBackup.TopMost = true;
            AppConfig["Main"]["TopMost"] = true;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }
        public static void TopMost_Disabled()
        {
            FormBackup.TopMost = false;
            AppConfig["Main"]["TopMost"] = false;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }
        #endregion
    }
}
