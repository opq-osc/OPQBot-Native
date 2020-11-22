using Deserizition;
using Launcher.Sdk.Cqp.Enum;
using Launcher.Sdk.Cqp.Expand;
using Native.Tool.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using SocketIOClient.Messages;
using System;
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

namespace Launcher.Forms
{
    public partial class MainForm : Form
    {
        static LogForm logForm = new LogForm();
        public Client socket;
        public static PluginManagment pluginManagment;
        private static int TryCount = 0;
        public static JObject AppConfig = new JObject();
        static bool Loaded = false;
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
            string name = (JObject.Parse(
                SendRequest($@"{Save.url}v1/LuaApiCaller?qq={Save.curentQQ}&funcname=GetQQUserList&timeout=10", "{\"StartIndex\":0}")
                )["Friendlist"] as JArray).Where
                (x => x["FriendUin"].ToString() == Save.curentQQ.ToString())
                .FirstOrDefault()["NickName"].ToString();
            Save.name = name;
            if (!Directory.Exists("conf"))
            {
                Directory.CreateDirectory("conf");
            }
            if (File.Exists(@"conf\states.json"))
            {
                AppConfig = JObject.Parse(File.ReadAllText(@"conf\states.json"));
            }
            if (AppConfig.Count == 0 || !AppConfig.ContainsKey("Main"))
            {
                JObject config = new JObject
                {
                    new JProperty("FormX",Left),
                    new JProperty("FormY",Top)
                };
                AppConfig.Add(new JProperty("Main", config));
                File.WriteAllText(@"conf\states.json", AppConfig.ToString());

            }
            else
            {
                Left = Convert.ToInt32(AppConfig["Main"]["FormX"].ToString());
                Top = Convert.ToInt32(AppConfig["Main"]["FormY"].ToString());
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
            notifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            //载入插件
            pluginManagment = new PluginManagment();
            Thread thread = new Thread(() => { pluginManagment.Init(); });
            thread.Start();
            //将托盘右键菜单复制一份
            pictureBox_Main.ContextMenu = notifyIcon.ContextMenu;
            //实例化圆形图片框, 实现圆形的头像
            HttpWebClient http = new HttpWebClient() { TimeOut = 3000 };
            byte[] data = { };
            Image image = Image.FromFile(@"conf\DefaultPic.jpeg");
            try
            {
                data = http.DownloadData($"http://q1.qlogo.cn/g?b=qq&nk={Save.curentQQ}&s=640");
                MemoryStream ms = new MemoryStream(data);
                if (ms.Length > 0)
                    image = Image.FromStream(ms);
                else
                    image = Image.FromFile(@"conf\DefaultPic.jpeg");
            }
            catch
            {
                CoreHelper.WriteLine("下载头像超时，重新启动程序可能解决这个问题");
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
            Loaded = true;
            //事件处理
            SocketHandler();
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            this.TopMost = false;
            this.TopMost = true;
        }

        private void pictureBox_Main_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        //圆形图片框, 来自CSDN
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
        private void 退出toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            return http.UploadString(url, data.ToString());
        }
        void SocketHandler()
        {
            //收到群消息的回调事件
            socket.On("OnGroupMsgs", (fn) =>
            {
                TryCount = 0;
                GroupMessageHandler(fn);
            });
            //收到好友消息的回调事件
            socket.On("OnFriendMsgs", (fn) =>
            {
                TryCount = 0;
                FriendMessageHandler(fn);
            });
            //统一事件管理如好友进群事件 好友请求事件 退群等事件集合
            socket.On("OnEvents", (fn) =>
            {
                OnEventsHandler(fn);
            });
        }
        private static void FriendMessageHandler(IMessage fn)
        {
            Task task = new Task(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ReceiveMessage groupMessage = JsonConvert.DeserializeObject<ReceiveMessage>(((JSONMessage)fn).MessageText);
                string message = ProgressMessage.Start(groupMessage);
                ReceiveMessage.Data data = groupMessage.CurrentPacket.Data;
                if (groupMessage.CurrentPacket.Data.FromUin == Save.curentQQ)
                {
                    Dll.AddMsgToSave(new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message));
                    return;
                }
                LogHelper.WriteLine(CQLogLevel.InfoReceive, "[↓]收到消息", $"来源QQ:{data.FromUin} {message}");
                var c = new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message);
                Dll.AddMsgToSave(c);
                pluginManagment.CallFunction("PrivateMsg", 11, Save.MsgList.Count + 1, data.FromUin, Marshal.StringToHGlobalAnsi(message), 0);
                stopwatch.Stop();
                LogHelper.WriteLine($"耗时 {stopwatch.ElapsedMilliseconds} ms");
            }); task.Start();
        }
        private static void GroupMessageHandler(IMessage fn)
        {
            Task task = new Task(() =>
            {
                string msg = ((JSONMessage)fn).MessageText;
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
                    pluginManagment.CallFunction("Upload", 1, GetTimeStamp(),
      data.FromGroupId, data.FromUserId, Convert.ToBase64String(stream.ToArray()));
                    LogHelper.WriteLine(CQLogLevel.InfoReceive, "文件上传", $"来源群:{data.FromGroupId}({data.FromGroupName}) 来源QQ:{data.FromUserId}({data.FromNickName}) " +
                        $"文件名:{fileupload["FileName"]} 大小:{Convert.ToDouble(fileupload["FileSize"]) / 1000}KB FileID:{fileupload["FileID"]}");
                    return;
                }
                string message = ProgressMessage.Start(groupMessage);
                //表示自己发送出去的消息, 写入消息列表
                if (groupMessage.CurrentPacket.Data.FromUserId == Save.curentQQ)
                {
                    Dll.AddMsgToSave(new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message));
                    return;
                }
                LogHelper.WriteLine(CQLogLevel.InfoReceive, "[↓]收到消息", $"来源群:{data.FromGroupId}({data.FromGroupName}) 来源QQ:{data.FromUserId}({data.FromNickName}) {message}");
                var c = new Deserizition.Message(Save.MsgList.Count + 1, data.MsgRandom, data.MsgSeq, data.FromGroupId, data.MsgTime, message);
                Dll.AddMsgToSave(c);//保存消息到消息列表
                //调用插件功能
                pluginManagment.CallFunction("GroupMsg", 2, Save.MsgList.Count + 1, data.FromGroupId, data.FromUserId,
                      "", Marshal.StringToHGlobalAnsi(message), 0);
                stopwatch.Stop();
                LogHelper.WriteLine($"耗时 {stopwatch.ElapsedMilliseconds} ms");
            }); task.Start();
        }
        private static void OnEventsHandler(IMessage fn)
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
                        if (GroupJoin_JoinUin != Save.curentQQ)
                            pluginManagment.CallFunction("GroupMemberIncrease", 1, GetTimeStamp(), GroupJoin_JoinGroup, 10000, GroupJoin_JoinUin);
                        LogHelper.WriteLine($"入群事件 群{GroupJoin_JoinGroup}加入{GroupJoin_JoinUserName}({GroupJoin_JoinUin})");
                        break;
                    case "ON_EVENT_GROUP_EXIT"://退群事件 _eventSystem_GroupMemberDecrease id=6
                        long GroupExit_FromUin, GroupExit_UserID;
                        JToken GroupExit_data = events["CurrentPacket"]["Data"];
                        GroupExit_FromUin = Convert.ToInt64(GroupExit_data["EventMsg"]["FromUin"].ToString());
                        GroupExit_UserID = Convert.ToInt64(GroupExit_data["EventData"]["UserID"].ToString());
                        pluginManagment.CallFunction("GroupMemberDecrease", 1, GetTimeStamp(), GroupExit_FromUin, 10000, GroupExit_UserID);
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
                        pluginManagment.CallFunction("GroupBan", (GroupShut_shuttime == 0 ? 1 : 2)
                            , GetTimeStamp(), GroupShut_groupid, 0, GroupShut_qqid, GroupShut_shuttime);
                        LogHelper.WriteLine($"群 {GroupShut_groupid} UserID {GroupShut_qqid} 禁言时间 {GroupShut_shuttime}秒");
                        break;
                    case "ON_EVENT_QQ_NETWORK_CHANGE":
                        TryCount++;
                        LogHelper.WriteLine($"与服务器连接断开，第{TryCount}次尝试重连");
                        break;
                }
            }); task.Start();
        }
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (Loaded is false)
                return;
            AppConfig["Main"]["FormX"] = Left;
            AppConfig["Main"]["FormY"] = Top;
            File.WriteAllText(@"conf\states.json", AppConfig.ToString());
        }
    }
}
