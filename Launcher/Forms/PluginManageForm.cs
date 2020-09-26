using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        static readonly Dictionary<int, string> ChineseName =new Dictionary<int, string>() {
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
            plugins = MainForm.pluginManagment.Plugins;
            foreach(var item in plugins)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems[0].Text = item.appinfo.Name;
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
                ShowPluginInfo(plugins.Find(x=>x.appinfo.Name==listView_PluginList.SelectedItems[0].Text));
            }
        }
        private void ShowPluginInfo(PluginManagment.Plugin plugin)
        {
            AppInfo appinfo = plugin.appinfo;
            label_Author.Text = appinfo.Author;
            label_Version.Text = appinfo.Version.ToString();
            label_Description.Text = appinfo.Description;
            JObject json = plugin.json;
            label_Auth.Text = $"需要以下权限({JArray.Parse(json["auth"].ToString()).Count})个";
            foreach (var item in (JArray)json["auth"])
                listBox_Auth.Items.Add(ChineseName[Convert.ToInt32(item.ToString())]);
        }

        private void button_Reload_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认重启框架吗？", "框架提出了一个疑问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                MainForm.pluginManagment.ReLoad();
        }
    }
}
