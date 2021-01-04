using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deserizition;
using static Launcher.PluginManagment;

namespace Launcher.Forms
{
    public partial class PluginTester : Form
    {
        public PluginTester(string[] args)
        {
            if (args.Length > 0)
            {
                string tmp = String.Empty;
                foreach (var item in args)
                {
                    tmp += item;
                }
                BeingTestedPlugin = MainForm.pluginManagment.Plugins.Find(x => x.appinfo.Name == tmp);
                Save.TestPluginsList.Add(tmp);
            }
            InitializeComponent();
        }
        static Plugin BeingTestedPlugin { get; set; }
        static Encoding GB18030 = Encoding.GetEncoding("GB18030");
        private void PluginTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("确认退出吗？关闭测试窗口将会导致插件被移出测试窗口，并且测试记录将会丢失"
                , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = false;
            }
            else
            {
                Save.TestPluginsList.Remove(BeingTestedPlugin.appinfo.Name);
            }
        }

        private void SendMsg_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MsgToSend.Text) is false)
            {
                ChatTextBox.SelectionColor = Color.Blue;
                ChatTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] 插件接受消息: {MsgToSend.Text}\n");
                string order = MsgToSend.Text;
                Task task = new Task(()=>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    CallPluginEvent(order);
                    sw.Stop();
                    ChatTextBox.Invoke(new MethodInvoker(()=>
                    {
                        ChatTextBox.SelectionColor = Color.Black;
                        ChatTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] 处理耗时: {sw.ElapsedMilliseconds} ms\n");
                    }));
                });
                task.Start();
                MsgToSend.Text = "";
            }
        }
        private void CallPluginEvent(string msg)
        {
            var b = Encoding.UTF8.GetBytes(msg);
            msg = GB18030.GetString(Encoding.Convert(Encoding.UTF8, GB18030, b));
            byte[] messageBytes = GB18030.GetBytes(msg + "\0");
            var messageIntptr = Marshal.AllocHGlobal(messageBytes.Length);
            Marshal.Copy(messageBytes, 0, messageIntptr, messageBytes.Length);
            int result = BeingTestedPlugin.dll.CallFunction("GroupMsg", 2, Save.MsgList.Count + 1, Convert.ToInt64(GroupID.Text),Convert.ToInt64(QQID.Text),
                     "", messageIntptr, 0);
            ChatTextBox.Invoke(new MethodInvoker(() =>
            {
                ChatTextBox.SelectionColor = Color.Black;
                if (result == 1)
                {
                    ChatTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] 由测试插件结束消息处理\n");
                }
                else
                {
                    ChatTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] 插件放行了处理\n");
                }
            }));
            Marshal.FreeHGlobal(messageIntptr);
            GC.Collect();
        }
        private void PluginTester_Load(object sender, EventArgs e)
        {
            PluginName.Text = BeingTestedPlugin.appinfo.Name;
            Save.TestPluginChatter = ChatTextBox;
        }

        private void GroupID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode < Keys.D0 && e.KeyCode > Keys.D9)
            {
                e.Handled = false;
            }
        }

        private void ChatTextBox_TextChanged(object sender, EventArgs e)
        {
            ChatTextBox.Focus();
            ChatTextBox.Select(ChatTextBox.TextLength, 0);
            ChatTextBox.ScrollToCaret();
        }
    }
}
