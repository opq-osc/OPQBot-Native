using Launcher.Sdk.Cqp.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class LogForm : Form
    {
        public LogForm()
        {            
            InitializeComponent();
        }
        public static ListView ListView_log;
        public static void LogWriter(ListView listView,CQLogLevel level, string logOrigin,string type, string status, params string[] messages)
        {
            Color LogColor=Color.Black;
            switch (level)
            {
                case CQLogLevel.Debug:
                    LogColor = Color.Gray;
                    break;
                case CQLogLevel.Error:
                    LogColor = Color.Red;
                    break;
                case CQLogLevel.Info:
                    LogColor = Color.Black;
                    break;
                case CQLogLevel.Fatal:
                    LogColor = Color.DarkRed;
                    break;
                case CQLogLevel.InfoSuccess:
                    LogColor = Color.Magenta;
                    break;
                case CQLogLevel.InfoSend:
                    LogColor = Color.Green;
                    break;
                case CQLogLevel.InfoReceive:
                    LogColor = Color.Blue;
                    break;
                case CQLogLevel.Warning:
                    LogColor = Color.FromArgb(255, 165, 0);
                    break;
            }
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.SubItems.Add(DateTime.Now.ToString("MM/dd HH: mm: ss"));
            listViewItem.SubItems.Add(logOrigin);
            listViewItem.SubItems.Add(type);
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            listViewItem.SubItems.Add(msg);
            listViewItem.SubItems.Add(status);
            listViewItem.ForeColor = LogColor;
            listView.Items.Add(listViewItem);
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            ListView_log = listView_LogMain;
            comboBox_LogLevel.SelectedIndex = 1;
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
    }
}
