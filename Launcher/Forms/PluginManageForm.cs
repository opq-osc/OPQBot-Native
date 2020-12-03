using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class PluginManageForm : Form
    {
        public PluginManageForm()
        {
            InitializeComponent();
        }
        List<PluginManagment.Plugin> plugins = new List<PluginManagment.Plugin>();
        static readonly Dictionary<int, string> ChineseName = new Dictionary<int, string>() {
            {20,"[敏感]取Cookies"},
            {30,"接收语音"},
            {101,"发送群消息"},
            {103,"发送讨论组消息"},
            {106,"发送私聊消息"},
            {110,"[敏感]发送赞"},
            {120,"置群员移除"},
            {121,"置群员禁言"},
            {122,"置群管理员"},
            {123,"置全群禁言"},
            {124,"置匿名群员禁言"},
            {125,"置群匿名设置"},
            {126,"置群成员名片"},
            {127,"[敏感]置群退出"},
            {128,"置群成员专属头衔"},
            {130,"取群成员信息"},
            {131,"取陌生人信息"},
            {132,"取群信息"},
            {140,"置讨论组退出"},
            {150,"置好友添加请求"},
            {151,"置群添加请求"},
            {160,"取群成员列表"},
            {161,"取群列表"},
            {162,"取好友列表"},
            {180,"撤回消息"},
        };
        private void PluginManageForm_Load(object sender, EventArgs e)
        {
            label_MainVersion.Text = $"{Application.ProductVersion}（开发版本）";
            plugins = MainForm.pluginManagment.Plugins;
            foreach (var item in plugins)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.ForeColor = item.Enable ? Color.Black : Color.Gray;
                listViewItem.SubItems[0].Text = (item.Enable ? "" : "[未启用] ") + item.appinfo.Name;
                listViewItem.SubItems.Add(item.appinfo.Version.ToString());
                listViewItem.SubItems.Add(item.appinfo.Author);
                listView_PluginList.Items.Add(listViewItem);
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView_PluginList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_Auth.Items.Clear();
            if (listView_PluginList.SelectedItems.Count != 0)
            {
                groupBox_Desc.Visible = true;
                var plugin = MainForm.pluginManagment.Plugins[listView_PluginList.SelectedItems[0].Index];
                button_Disable.Text = plugin.Enable ? "停用" : "启用";
                string desc = $"{plugin.appinfo.Name} ({plugin.appinfo.Id})";
                var b = Encoding.Default.GetBytes(desc);
                if (b.Length >= 40)
                {
                    List<byte> res = new List<byte>();
                    for (int i = 0; i < 37; i++)
                        res.Add(b[i]);
                    desc = Encoding.Default.GetString(res.ToArray()) + "...";
                }
                groupBox_Desc.Text = desc;
                //ShowPluginInfo(plugins.Find(x => x.appinfo.Name == listView_PluginList.SelectedItems[0].Text));
                ShowPluginInfo(plugin);
            }
            else
            {
                groupBox_Desc.Visible = false;
            }
        }
        private void ShowPluginInfo(PluginManagment.Plugin plugin)
        {
            AppInfo appinfo = plugin.appinfo;
            label_Author.Text = appinfo.Author;
            label_Version.Text = appinfo.Version.ToString();
            label_Description.Text = appinfo.Description;
            JObject json = plugin.json;
            label_Auth.Text = $"需要以下权限（{JArray.Parse(json["auth"].ToString()).Count}个）";
            foreach (var item in (JArray)json["auth"])
                listBox_Auth.Items.Add(ChineseName[Convert.ToInt32(item.ToString())]);
        }

        private void button_Reload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认重启框架吗？", "框架提出了一个疑问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                MainForm.pluginManagment.ReLoad();
        }

        private void button_Disable_Click(object sender, EventArgs e)
        {
            if (listView_PluginList.SelectedItems.Count == 0)
                return;
            var plugin = MainForm.pluginManagment.Plugins[listView_PluginList.SelectedItems[0].Index];
            var listBoxItem = listView_PluginList.SelectedItems[0];
            if (plugin.Enable)
            {
                plugin.Enable = false;
                listBoxItem.ForeColor = Color.Gray;
                listBoxItem.SubItems[0].Text = (plugin.Enable ? "" : "[未启用] ") + plugin.appinfo.Name;
                button_Disable.Text = "启用";
            }
            else
            {
                plugin.Enable = true;
                listBoxItem.ForeColor = Color.Black;
                listBoxItem.SubItems[0].Text = (plugin.Enable ? "" : "[未启用] ") + plugin.appinfo.Name;
                button_Disable.Text = "停用";
            }
            MainForm.pluginManagment.FlipPluginState(plugin);
        }

        private void button_AppDir_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "data", "plugins");
            Process.Start(path);
        }

        private void button_Menu_Click(object sender, EventArgs e)
        {
            if (listView_PluginList.SelectedItems.Count != 0)
            {
                var plugin = MainForm.pluginManagment.Plugins[listView_PluginList.SelectedItems[0].Index];
                ContextMenu menu = NotifyIconHelper._NotifyIcon.ContextMenu;
                var res = menu.MenuItems.Find("PluginMenu", false)[0].MenuItems.Find(plugin.appinfo.Name, false)[0];
                ContextMenu contextMenu = new ContextMenu();
                foreach(MenuItem item in res.MenuItems)
                {
                    var b = item.CloneMenu();
                    b.Tag = item.Tag;
                    contextMenu.MenuItems.Add(b);
                }    
                contextMenu.Show(button_Menu,button_Menu.PointToClient(MousePosition));
            }
        }
    }
}
