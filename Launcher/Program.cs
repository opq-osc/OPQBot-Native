using Launcher.Forms;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;

namespace Launcher
{
    public static class Program
    {
        public static Client socket;
        public static PluginManagment pluginManagment;

        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        static void Main()
        {
            Process[] process = Process.GetProcessesByName("Launcher");
            if (process.Length != 1)
            {
                MessageBox.Show("已经启动了一个程序");
                return;
            }
            Application.EnableVisualStyles();
            //异常捕获
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);            
            Application.ThreadException += Application_ThreadException;
            //未处理的异常捕获
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                if(MessageBox.Show($"发生错误，错误信息{ex}\n\n需要重启框架？", "错误"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    pluginManagment.ReLoad();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                if( MessageBox.Show($"发生错误，错误信息{e}\n\n需要重启框架？", "错误"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    pluginManagment.ReLoad();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
