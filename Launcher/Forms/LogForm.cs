using Deserizition;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }
        #region --字段--
        public static ListView ListView_log;
        public bool formTopMost = false;
        public LogLevel LogPriority = LogLevel.Info;
        private static List<LogModel> LogLists;
        #endregion

        private void LogForm_Load(object sender, EventArgs e)
        {
            ListView_log = listView_LogMain;
            comboBox_LogLevel.SelectedIndex = 1;
            LogHelper.WriteLog(LogLevel.Info, "提示", "成功连接到服务器");
            LoadLogs();
            Thread thread = new Thread(() => { ListenLogBoardCast(); });
            thread.Start();
        }

        private void LoadLogs()
        {
            listView_LogMain.Items.Clear();
            LogLists = LogHelper.GetDisplayLogs((int)LogPriority);
            foreach (var item in LogLists)
            {
                AddItem2ListView(item);
            }
        }

        private void ListenLogBoardCast(int port = 28634)
        {
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);
                sock.Bind(iep);
                EndPoint ep = iep;
                Debug.WriteLine($"开启广播侦听，端口{port}...");
                //Ready to receive…
                while (true)
                {
                    byte[] data = new byte[1024];
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.UTF8.GetString(data, 0, recv);
                    Debug.WriteLine("received: {0} from: {1}", stringData, ep.ToString());
                    JObject json = JObject.Parse(stringData);
                    if (json["QQ"].Value<long>() != Save.curentQQ)
                        continue;
                    switch (json["Type"].ToString())
                    {
                        case "Log":
                            AddLog(LogHelper.GetLogByID(Convert.ToInt32(json["LogID"].ToString())));
                            break;
                        case "Update":
                            UpdateItemStatus(Convert.ToInt32(json["LogID"].ToString()), json["Msg"].ToString());
                            break;
                    }
                }
            }
        }

        private void AddItem2ListView(LogModel item)
        {
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.SubItems[0].Text = LogHelper.GetTimeStampString(item.time);
            listViewItem.SubItems.Add(item.source);
            listViewItem.SubItems.Add(item.name);
            listViewItem.SubItems.Add(item.detail);
            listViewItem.SubItems.Add(item.status);
            listViewItem.ForeColor = GetLogColor(item.priority);//消息颜色
            listView_LogMain.Invoke(new MethodInvoker(() =>
            {
                listView_LogMain.Items.Add(listViewItem);
                if (checkBox_Update.Checked)//日志自动滚动
                {
                    listView_LogMain.EnsureVisible(listView_LogMain.Items.Count - 1);
                    listViewItem.Selected = true;
                }
            }));            
        }

        private void AddLog(LogModel model)
        {
            if (model == null || model.priority<(int)LogPriority)
                return;
            LogLists.Add(model);
            AddItem2ListView(model);
            if (LogLists.Count >= Save.LogerMaxCount)
            {
                LogLists.RemoveAt(0);
                listView_LogMain.Invoke(new MethodInvoker(() => { listView_LogMain.Items.RemoveAt(0); }));
            }
        }

        private void UpdateItemStatus(int id, string msg)
        {
            LogModel item = LogLists.Find(x => x.id == id);
            item.status = msg;
            listView_LogMain.Invoke(new MethodInvoker(() =>
            {
                listView_LogMain.Items[LogLists.IndexOf(item)].SubItems[4].Text = msg;
            }));
        }

        /// <summary>
        /// 获取日志文本颜色
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <returns></returns>
        private static Color GetLogColor(LogLevel level)
        {
            Color LogColor;
            switch (level)
            {
                case LogLevel.Debug:
                    LogColor = Color.Gray;
                    break;
                case LogLevel.Error:
                    LogColor = Color.Red;
                    break;
                case LogLevel.Info:
                    LogColor = Color.Black;
                    break;
                case LogLevel.Fatal:
                    LogColor = Color.DarkRed;
                    break;
                case LogLevel.InfoSuccess:
                    LogColor = Color.Magenta;
                    break;
                case LogLevel.InfoSend:
                    LogColor = Color.Green;
                    break;
                case LogLevel.InfoReceive:
                    LogColor = Color.Blue;
                    break;
                case LogLevel.Warning:
                    LogColor = Color.FromArgb(255, 165, 0);
                    break;
                default:
                    LogColor = Color.Black;
                    break;
            }
            return LogColor;
        }
        private static Color GetLogColor(int value)
        {
            LogLevel loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), Enum.GetName(typeof(LogLevel), value));
            return GetLogColor(loglevel);
        }

        private LogLevel GetLogPriority(int selectIndex)
        {
            switch (selectIndex)
            {
                case 0:
                    return LogLevel.Debug;
                case 1:
                    return LogLevel.Info;
                case 2:
                    return LogLevel.Warning;
                case 3:
                    return LogLevel.Error;
                case 4:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Info;
            }
        }

        private void comboBox_LogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogPriority = GetLogPriority((sender as ComboBox).SelectedIndex);
            LoadLogs();
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox_Update_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                Thread thread = new Thread(() =>
                {
                    label_Desc.Invoke(new MethodInvoker(() => { label_Desc.Visible = true; }));
                    Save.AutoScroll = true;
                    listView_LogMain.EnsureVisible(listView_LogMain.Items.Count - 1);
                    listView_LogMain.Items[listView_LogMain.Items.Count - 1].Selected = true;
                    Thread.Sleep(2000);
                    label_Desc.Invoke(new MethodInvoker(() => { label_Desc.Visible = false; }));
                });
                thread.Start();
            }
            else
            {
                Save.AutoScroll = false;
            }
        }

        private void checkBox_AboveAll_CheckedChanged(object sender, EventArgs e)
        {
            formTopMost = (sender as CheckBox).Checked;
            this.TopMost = formTopMost;
        }

        private void listView_LogMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (listView_LogMain.SelectedItems.Count != 0 && e.Button == MouseButtons.Right)
            {
                Thread thread = new Thread(() =>
                {
                    label_Desc.Invoke(new MethodInvoker(() =>
                    {
                        label_Desc.Text = "已复制日志内容";
                        label_Desc.Visible = true;
                        string text = listView_LogMain.SelectedItems[0].SubItems[3].Text;
                        Clipboard.SetText(text);
                    }));
                    Thread.Sleep(2000);
                    label_Desc.Invoke(new MethodInvoker(() => { label_Desc.Text = "日志列表将实时滚动"; label_Desc.Visible = false; }));
                });
                thread.Start();
            }
        }

        private void listView_LogMain_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            string itemInfor = e.Item.SubItems[3].Text;
            toolTip.SetToolTip((e.Item).ListView, itemInfor);
        }
    }
}
