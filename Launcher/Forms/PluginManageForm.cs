using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// 权限转中文名称字典
        /// </summary>
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
            //版本号
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
                //由于插件表内顺序与listview中的顺序是一样的,所以可以直接下标获取
                var plugin = MainForm.pluginManagment.Plugins[listView_PluginList.SelectedItems[0].Index];
                button_Disable.Text = plugin.Enable ? "停用" : "启用";
                string desc = $"{plugin.appinfo.Name} ({plugin.appinfo.Id})";
                //Length 中中文字符也是1,不合适,需要换成byte[]
                var b = Encoding.Default.GetBytes(desc);
                if (b.Length >= 40)//长度溢出控制
                {
                    List<byte> res = new List<byte>();
                    for (int i = 0; i < 37; i++)
                        res.Add(b[i]);
                    desc = Encoding.Default.GetString(res.ToArray()) + "...";
                }
                groupBox_Desc.Text = desc;
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
                plugin.dll.CallFunction("Disable");
            }
            else
            {
                plugin.Enable = true;
                listBoxItem.ForeColor = Color.Black;
                listBoxItem.SubItems[0].Text = (plugin.Enable ? "" : "[未启用] ") + plugin.appinfo.Name;
                button_Disable.Text = "停用";
                plugin.dll.CallFunction("Enable");
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
                //筛选插件父菜单
                var res = menu.MenuItems.Find("PluginMenu", false)[0].MenuItems.Find(plugin.appinfo.Name, false)[0];
                ContextMenu contextMenu = new ContextMenu();
                foreach(MenuItem item in res.MenuItems)
                {
                    var b = item.CloneMenu();//必须要用clone,单纯用add会将MenuItem与原Menu的连接切断
                    b.Tag = item.Tag;//复制过来的item依旧拥有click事件,但是失去了tag
                    contextMenu.MenuItems.Add(b);
                }    
                //PointToClient 将鼠标位置转换为相对于控件的位置
                //MousePosition 控件中的属性
                contextMenu.Show(button_Menu,button_Menu.PointToClient(MousePosition));
            }
        }

        private void button_Dev_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("确认进入插件测试模式吗？测试状态下，此插件将无法处理消息"
                , "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string pluginName = plugins[listView_PluginList.SelectedItems[0].Index].appinfo.Name;
                if (Deserizition.Save.TestPluginsList.Any(x => x == pluginName))
                {
                    MessageBox.Show("此插件已经处在测试模式下，不可重复添加");
                    return;
                }
                PluginTester pluginTester = new PluginTester(new string[] { pluginName });
                pluginTester.Show();
            }
        }
    }
}
