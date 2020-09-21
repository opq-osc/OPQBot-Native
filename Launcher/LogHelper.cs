using Deserizition;
using Launcher.Sdk.Cqp.Enum;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
    public static class LogHelper
    {
        public static void WriteLine(params string[] messages)
        {
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            if(!Save.formFlag)
                WriteLine(CQLogLevel.Info, "提示", msg);
            else
                LogWriter(Save.logListView, CQLogLevel.Info, "OPQBot框架", "提示","...", messages);
        }
        //写日志方法
        //TODO：写入Sqlite
        public static void LogWriter(ListView listView, CQLogLevel level, string logOrigin, string type, string status, params string[] messages)
        {
            Color LogColor = GetLogColor(level);
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.SubItems[0].Text = DateTime.Now.ToString("今天 HH:mm:ss");
            listViewItem.SubItems.Add(logOrigin);
            listViewItem.SubItems.Add(type);
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            listViewItem.SubItems.Add(msg);
            listViewItem.SubItems.Add(status);
            listViewItem.ForeColor = LogColor;
            listView.Invoke(new MethodInvoker(() => { listView.Items.Add(listViewItem); })); 
        }
        private static Color GetLogColor(CQLogLevel level)
        {
            Color LogColor;
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
                default:
                    LogColor = Color.Black;
                    break;
            }

            return LogColor;
        }

        public static void WriteLine(CQLogLevel level,string type, params string[] messages)
        {
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            switch (level)
            {
                case CQLogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case CQLogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CQLogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case CQLogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case CQLogLevel.InfoSuccess:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case CQLogLevel.InfoSend:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CQLogLevel.InfoReceive:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case CQLogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            if (!Save.formFlag)
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][OPQBot框架 {type}]{msg}");
            else
                LogWriter(Save.logListView, level, "OPQBot框架", type,"...", messages);
        }
    }
}
