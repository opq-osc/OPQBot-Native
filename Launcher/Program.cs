using Deserizition;
using Launcher.Forms;
using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        static void Main(params string[] args)
        {
            Process[] process = Process.GetProcessesByName("Launcher");
            if (args.Length != 0 && args[0] == "-r")
            {
                int initialNum = process.Length;
                while(Process.GetProcessesByName("Launcher").Length!=initialNum-1)
                {
                    Thread.Sleep(1000);
                }
            }
            else
            {
                if (process.Length != 1)
                {
                    //MessageBox.Show("已经启动了一个程序");
                    //return;
                }
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
        #region --TEST--
        static void M1ain()
        {
            Load(@"E:\酷Q机器人插件开发\OPQBot-Native\data\plugins\me.cqp.luohuaming.Setu.dll");
            UnLoad(Plugins[0]);
            GC.Collect();
            Console.ReadLine();
        }
        static List<PluginManagment.Plugin> Plugins = new List<PluginManagment.Plugin>();
        /// <summary>
        /// 以绝对路径路径载入拥有同名json的dll插件
        /// </summary>
        /// <param name="filepath">插件dll的绝对路径</param>
        /// <returns>载入是否成功</returns>
        public static bool Load(string filepath)
        {
            FileInfo plugininfo = new FileInfo(filepath);
            JObject json = JObject.Parse(File.ReadAllText(plugininfo.FullName.Replace(".dll", ".json")));
            Dll dll = new Dll();
            IntPtr iLib = dll.Load(filepath, json);//将dll插件LoadLibrary,并进行函数委托的实例化

            if (iLib == (IntPtr)0)
            {
                Console.WriteLine($"插件 {plugininfo.Name} 加载失败,返回句柄为空,GetLastError={Dll.GetLastError()}");
                return false;
            }
            KeyValuePair<int, string> appInfotext = dll.GetAppInfo();
            AppInfo appInfo = new AppInfo(appInfotext.Value, 0, appInfotext.Key
                , json["name"].ToString(), json["version"].ToString(), Convert.ToInt32(json["version_id"].ToString())
                , json["author"].ToString(), json["description"].ToString(), 0);
            Plugins.Add(new PluginManagment.Plugin(iLib, appInfo, json, dll, false));

            Console.WriteLine($"插件 {appInfo.Name} 加载成功");
            return true;
        }
        /// <summary>
        /// 卸载插件，执行被卸载事件，从菜单移除此插件的菜单
        /// </summary>
        /// <param name="plugin"></param>
        public static void UnLoad(PluginManagment.Plugin plugin)
        {
            try
            {
                plugin.dll.UnLoad();
                Console.WriteLine($"插件 {plugin.appinfo.Name} 卸载成功");
                plugin = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }

        #endregion
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
                if (b==ErrorHelper.TaskDialogResult.ReloadApp)
                {
                    MainForm.pluginManagment.ReLoad();
                }
                else if(b==ErrorHelper.TaskDialogResult.Exit)
                {
                    Environment.Exit(0);
                }
            }
        }        
    }
}
