using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;
using Deserizition;
using Launcher.Forms;
using Launcher.Pipe;

namespace Launcher
{
    public static class Program
    {
        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        static void Main(params string[] args)
        {
            Process[] process = Process.GetProcessesByName("Launcher");
            if (args.Length != 0 && args[0] == "-r")
            {
                int initialNum = process.Length;
                if (initialNum != 1)
                {
                    while (Process.GetProcessesByName("Launcher").Length != initialNum - 1)
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
            else if(args.Length==3 && args[0]=="-m" && !string.IsNullOrWhiteSpace(args[1]))
            {
                Save.NamedPipeName = args[1];
                Save.PipeType = PipeType.Client;
                Save.AuthCode = Convert.ToInt32(args[2]);
            }
            else
            {
                if (process.Length != 1)
                {
                    MessageBox.Show("已经启动了一个程序");
                    return;
                }
            }            
            if(Save.MutiProcessMode is false)
            {
                Save.PipeType = PipeType.NoPipe;
            }
            else if (args.Length != 3 || args[0] != "-m")
            {
                Save.PipeType = PipeType.Server;
            }
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            //异常捕获
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            //未处理的异常捕获
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            switch (Save.PipeType)
            {
                case PipeType.NoPipe:
                case PipeType.Server:                                     
                    Application.Run(new Login());
                    break;
                case PipeType.Client://进程分离模式中不需要窗口，使用命名管道传递消息
                    NamedPipeClient client = new NamedPipeClient();
                    client.ReceiveMsg();
                    break;
            }
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var b = ErrorHelper.ShowErrorDialog($"{ex.Message}\n{ex.StackTrace}");
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

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
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
