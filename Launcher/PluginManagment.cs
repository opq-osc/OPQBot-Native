using Launcher.Sdk.Cqp.Enum;
using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string path = @"data\plugins";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            int count = 0;
            foreach (var item in directoryInfo.GetFiles().Where(x => x.Extension == ".dll"))
            {
                if (!File.Exists(item.FullName.Replace(".dll", ".json")))
                {
                    LogHelper.WriteLine(CQLogLevel.Error, "插件载入", $"插件 {item.Name} 加载失败,原因:缺少json文件");
                    continue;
                }
                JsonLoadSettings loadsetting = new JsonLoadSettings
                {
                    CommentHandling = CommentHandling.Ignore
                };
                JObject json = JObject.Parse(File.ReadAllText(item.FullName.Replace(".dll", ".json")), loadsetting);
                int authcode = new Random().Next();
                Dll dll = new Dll();
                IntPtr iLib = dll.Load(item.FullName, json);
                if (iLib == (IntPtr)0)
                {
                    LogHelper.WriteLine(CQLogLevel.Error, "插件载入", $"插件 {item.Name} 加载失败,返回句柄为空,GetLastError={Dll.GetLastError()}");
                    continue;
                }
                dll.DoInitialize(authcode);
                KeyValuePair<int, string> appInfotext = dll.GetAppInfo();
                AppInfo appInfo = new AppInfo(appInfotext.Value, 0, appInfotext.Key
                    , json["name"].ToString(), json["version"].ToString(), Convert.ToInt32(json["version_id"].ToString())
                    , json["author"].ToString(), json["description"].ToString(), authcode);
                Plugins.Add(new Plugin(iLib, appInfo, json, dll));
                LogHelper.WriteLine(CQLogLevel.InfoSuccess, "插件载入", $"插件 {appInfo.Name} 加载成功");
                cq_start(Marshal.StringToHGlobalAnsi(item.FullName), authcode);
                NotifyIconHelper.Init(json);
                count++;
            }
            LogHelper.WriteLine(CQLogLevel.Info, "插件载入", $"一共加载了{count}个插件");
        }
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr lib);
        [DllImport("CQP.dll", EntryPoint = "cq_start")]
        private static extern bool cq_start(IntPtr path, int authcode);

        public bool UnLoad(IntPtr hLib)
        {
            try
            {
                if (hLib != null)
                {
                    FreeLibrary(hLib);
                    var p = Plugins.Find(x => x.iLib == hLib);
                    LogHelper.WriteLine(CQLogLevel.InfoSuccess, "插件卸载", $"插件 {p.appinfo.Name}({p.appinfo.Id}) 卸载成功");
                    Plugins.Remove(p);
                    return true;
                }
                else
                {
                    LogHelper.WriteLine(CQLogLevel.Error, "插件卸载", "hLib is null!");
                    return false;
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLine(CQLogLevel.Error, "插件卸载", e.Message, e.StackTrace);
                return false;
            }
        }
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
        public void CallMenu()
        {
            var item = Plugins.Find(x => x.appinfo.Name.Contains("水银"));
            Dll dll = item.dll;
            dll.CallMenuA();
        }
    }
}
