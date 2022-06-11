﻿using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;
using Deserizition;
using Launcher.Forms;

namespace Launcher
{
    public static class Program
    {
        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        static void Main(params string[] args)
        {
            Process[] process = Process.GetProcessesByName("OPQBot-Native");
            if (args.Length != 0 && args[0] == "-r")
            {
                int initialNum = process.Length;
                if (initialNum != 1)
                {
                    while (Process.GetProcessesByName("OPQBot-Native").Length != initialNum - 1)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            else if(args.Length != 0 && args[0] == "-i")
            {
                Save.IgnoreProcessChecking = true;
                //Do nothing. Ignore Process Checking
            }
            else
            {
                if (process.Length != 1)
                {
                    MessageBox.Show("已经启动了一个程序");
                    return;
                }
            }
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            //异常捕获
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            //未处理的异常捕获
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Run(new Login());
        }
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var b = ErrorHelper.ShowErrorDialog($"{ex.Message}\n{ex.StackTrace}", false);
                if (b == ErrorHelper.TaskDialogResult.ReloadApp)
                {
                    MainForm.pluginManagment.ReLoad();
                }
                else if (b == ErrorHelper.TaskDialogResult.Exit)
                {
                    Environment.Exit(0);
                }
                else if (b == ErrorHelper.TaskDialogResult.Restart)
                {
                    string path = typeof(MainForm).Assembly.Location;
                    Process.Start(path, $"-r");
                    NotifyIconHelper.HideNotifyIcon();
                    Environment.Exit(0);
                }
            }
        }

        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                var b = ErrorHelper.ShowErrorDialog($"{e.Exception.Message}\n{e.Exception.StackTrace}");
                if (b == ErrorHelper.TaskDialogResult.ReloadApp)
                {
                    MainForm.pluginManagment.ReLoad();
                }
                else if (b == ErrorHelper.TaskDialogResult.Exit)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
