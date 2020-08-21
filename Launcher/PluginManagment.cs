using Launcher.Sdk.Cqp.Enum;
using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Launcher
{
    public class PluginManagment
    {
        public List<Plugin> Plugins = new List<Plugin>();
        public class Plugin
        {
            public IntPtr iLib;
            public AppInfo appinfo;
            public JObject json;
            public Dll dll;
            public Plugin(IntPtr iLib, AppInfo appinfo, JObject json, Dll dll)
            {
                this.iLib = iLib;
                this.appinfo = appinfo;
                this.json = json;
                this.dll = dll;
            }
        }
        public void Load()
        {
            NotifyIconHelper.Init();
            string path = @"data\plugins";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            int count = 0;
            foreach (var item in directoryInfo.GetFiles().Where(x => x.Extension == ".dll"))
            {
                if (Load(item.FullName))
                    count++;
            }
            LogHelper.WriteLine(CQLogLevel.Info, "插件载入", $"一共加载了{count}个插件");
        }
        public bool Load(string filepath)
        {
            FileInfo plugininfo = new FileInfo(filepath);
            if (!File.Exists(plugininfo.FullName.Replace(".dll", ".json")))
            {
                LogHelper.WriteLine(CQLogLevel.Error, "插件载入", $"插件 {plugininfo.Name} 加载失败,原因:缺少json文件");
                return false;
            }
            JObject json = JObject.Parse(File.ReadAllText(plugininfo.FullName.Replace(".dll", ".json")));
            int authcode = new Random().Next();
            Dll dll = new Dll();
            if (!Directory.Exists(@"data\tmp"))
                Directory.CreateDirectory(@"data\tmp");
            string destpath = @"data\tmp\" + plugininfo.Name;
            File.Copy(plugininfo.FullName, destpath, true);
            File.Copy(plugininfo.FullName.Replace(".dll", ".json"), destpath.Replace(".dll", ".json"), true);
            IntPtr iLib = dll.Load(destpath, json);
            if (iLib == (IntPtr)0)
            {
                LogHelper.WriteLine(CQLogLevel.Error, "插件载入", $"插件 {plugininfo.Name} 加载失败,返回句柄为空,GetLastError={Dll.GetLastError()}");
                return false;
            }
            dll.DoInitialize(authcode);
            KeyValuePair<int, string> appInfotext = dll.GetAppInfo();
            AppInfo appInfo = new AppInfo(appInfotext.Value, 0, appInfotext.Key
                , json["name"].ToString(), json["version"].ToString(), Convert.ToInt32(json["version_id"].ToString())
                , json["author"].ToString(), json["description"].ToString(), authcode);
            Plugins.Add(new Plugin(iLib, appInfo, json, dll));
            LogHelper.WriteLine(CQLogLevel.InfoSuccess, "插件载入", $"插件 {appInfo.Name} 加载成功");
            cq_start(Marshal.StringToHGlobalAnsi(destpath), authcode);
            NotifyIconHelper.LoadMenu(json);
            return true;
        }
        public void UnLoad(Plugin plugin)
        {
            try
            {
                plugin.dll.UnLoad();
                NotifyIconHelper.RemoveMenu(plugin.appinfo.Name);
                Plugins.Remove(plugin);
                LogHelper.WriteLine(CQLogLevel.InfoSuccess, "插件卸载", $"插件 {plugin.appinfo.Name} 卸载成功");
                plugin = null; GC.Collect();
            }
            catch (Exception e)
            {
                LogHelper.WriteLine(CQLogLevel.Error, "插件卸载", e.Message, e.StackTrace);
            }
        }
        public void UnLoad()
        {
            for (int i = 0; i < Plugins.Count; i++)
            {
                Plugin item = Plugins[0];
                UnLoad(item);
            }
        }
        public void Init()
        {
            if(Directory.Exists(@"data\tmp"))
                //Directory.Delete(@"data\tmp", true);
            Load();
            LogHelper.WriteLine("遍历启动事件……");
            CallFunction("StartUp");
            CallFunction("Enable");
            NotifyIconHelper.ShowNotifyIcon();
        }
        [DllImport("CQP.dll", EntryPoint = "cq_start")]
        private static extern bool cq_start(IntPtr path, int authcode);

        [HandleProcessCorruptedStateExceptions]
        public void CallFunction(string ApiName, params object[] args)
        {
            foreach (var item in Plugins)
            {
                Dll dll = item.dll;
                if (!dll.HasFunction(ApiName, item.json)) continue;
                try
                {
                    if (dll.CallFunction(ApiName, args) == 1) return;
                }
                catch (Exception e)
                {
                    LogHelper.WriteLine(CQLogLevel.Error, $"插件 {item.appinfo.Name} {ApiName} 函数发生错误，错误信息:{e.Message} {e.StackTrace}");
                }
            }
        }
    }
}
