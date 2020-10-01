using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deserizition
{
    public enum LogLevel
    {
        /// <summary>
        /// 调试.		颜色: 灰色
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 信息.		颜色: 黑色
        /// </summary>
        Info = 10,
        /// <summary>
        /// 信息 (成功)	颜色: 紫色
        /// </summary>
        InfoSuccess = 11,
        /// <summary>
        /// 信息 (接收)	颜色: 蓝色
        /// </summary>
        InfoReceive = 12,
        /// <summary>
        /// 信息 (发送)	颜色: 绿色
        /// </summary>
        InfoSend = 13,
        /// <summary>
        /// 警告			颜色: 橙色
        /// </summary>
        Warning = 20,
        /// <summary>
        /// 错误			颜色: 红色
        /// </summary>
        Error = 30,
        /// <summary>
        /// 致命错误		颜色: 深红色
        /// </summary>
        Fatal = 40
    }
    /// <summary>
    /// 公用帮助类,实现:日志
    /// </summary>
    public static class CoreHelper
    {
        /// <summary>
        /// 以info为等级，"OPQBot框架"为来源，"提示"为类型写出一条日志
        /// </summary>
        /// <param name="messages">日志内容</param>
        public static void WriteLine(string messages)
        {
            if (!Save.formFlag)
                WriteLine("OPQBot框架",(int)LogLevel.Info, "提示", messages);
            else
                LogWriter(Save.logListView,(int)LogLevel.Info, "OPQBot框架", "提示", "...", messages);
        }
        //TODO：写入Sqlite
        /// <summary>
        /// 向指定的ListView输出一条指定内容的日志
        /// </summary>
        /// <param name="listView">日志窗口的ListView</param>
        /// <param name="level">日志等级</param>
        /// <param name="logOrigin">消息来源</param>
        /// <param name="type">类型</param>
        /// <param name="status">状态(留空)</param>
        /// <param name="messages">日志内容</param>
        public static void LogWriter(ListView listView, int level, string logOrigin, string type, string status, params string[] messages)
        {
            LogLevel loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), Enum.GetName(typeof(LogLevel), level));
            Color LogColor = GetLogColor(loglevel);
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.SubItems[0].Text = DateTime.Now.ToString("今天 HH:mm:ss");//时间
            listViewItem.SubItems.Add(logOrigin);
            listViewItem.SubItems.Add(type);
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            listViewItem.SubItems.Add(msg);
            listViewItem.SubItems.Add(status);
            listViewItem.ForeColor = LogColor;//消息颜色    
            listView.Invoke(new MethodInvoker(() => {
                listView.Items.Add(listViewItem);
                if (Save.AutoScroll)//日志自动滚动
                {
                    listView.EnsureVisible(listView.Items.Count - 1);
                    listViewItem.Selected = true;
                }
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
        /// <summary>
        /// 反正套娃就对了
        /// </summary>
        /// <param name="pluginname"></param>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <param name="messages"></param>
        public static void WriteLine(string pluginname,int level, string type, string messages)
        {
            LogLevel loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), Enum.GetName(typeof(LogLevel), level));
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            switch (loglevel)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogLevel.InfoSuccess:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogLevel.InfoSend:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.InfoReceive:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            if (!Save.formFlag)
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][OPQBot框架 {type}]{msg}");
            else
                LogWriter(Save.logListView, (int)level,string.IsNullOrEmpty(pluginname)?"OPQBot框架":pluginname, type, "...", messages);
        }

    }
}
