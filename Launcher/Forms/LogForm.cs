using Deserizition;
using Launcher.Sdk.Cqp.Enum;
using System;
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
        public bool formTopMost=false;
        #endregion

        private void LogForm_Load(object sender, EventArgs e)
        {
            ListView_log = listView_LogMain;
            comboBox_LogLevel.SelectedIndex = 1;
            LogHelper.LogWriter(ListView_log, CQLogLevel.Info,"OPQBot框架","提示","...","成功连接到服务器");
            Save.formFlag = true;
            Save.logListView = listView_LogMain;
        }

        private void comboBox_LogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                Thread thread = new Thread(()=> 
                {
                    label_Desc.Invoke(new MethodInvoker(() => { label_Desc.Visible = true; }));
                    Thread.Sleep(2000);
                    label_Desc.Invoke(new MethodInvoker(() => { label_Desc.Visible = false; }));
                });
                thread.Start();
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
