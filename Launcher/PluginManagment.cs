using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Deserizition;
using Launcher.Forms;
using Launcher.Pipe;
using Launcher.Sdk.Cqp.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Launcher
{
    /// <summary>
    /// 管理CQ插件的类
    /// </summary>
    public class PluginManagment
    {
        public List<Plugin> Plugins = new List<Plugin>();
        public class Plugin
        {
            /// <summary>
            /// 内存句柄
            /// </summary>
            public IntPtr iLib;
            /// <summary>
            /// 插件的AppInfo
            /// </summary>
            public AppInfo appinfo;
            /// <summary>
            /// 插件的json部分,包含名称、描述、函数入口以及窗口名称部分
            /// </summary>
            public JObject json;
            public Dll dll;
            /// <summary>
            /// 标记插件是否启用
            /// </summary>
            public bool Enable;
            public Plugin(IntPtr iLib, AppInfo appinfo, JObject json, Dll dll, bool enable, string namedpipe)
            {
                this.iLib = iLib;
                this.appinfo = appinfo;
                this.json = json;
                this.dll = dll;
                this.Enable = enable;
                if(string.IsNullOrWhiteSpace(Save.NamedPipeName) && Save.MutiProcessMode)
                {
                    this.NamedPipe = new NamedPipeServer(namedpipe);
                }
            }
            public NamedPipeServer NamedPipe;
        }
        /// <summary>
        /// 从 data\plugins 文件夹下载入所有拥有同名json的dll插件，不包含子文件夹
        /// </summary>
        public void Load()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string path = Path.Combine(Environment.CurrentDirectory, "data", "plugins");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            int count = 0;
            foreach (var item in directoryInfo.GetFiles().Where(x => x.Extension == ".dll"))
            {
                if (Load(item.FullName))
                    count++;
            }
            sw.Stop();
            LogHelper.WriteLog(LogLevel.Info, "插件载入", $"一共加载了{count}个插件",$"√ {sw.ElapsedMilliseconds} ms");
            NotifyIconHelper.AddManageMenu();
        }
        /// <summary>
        /// 以绝对路径路径载入拥有同名json的dll插件
        /// </summary>
        /// <param name="filepath">插件dll的绝对路径</param>
        /// <returns>载入是否成功</returns>
        public bool Load(string filepath)
        {
            FileInfo plugininfo = new FileInfo(filepath);
            if (!File.Exists(plugininfo.FullName.Replace(".dll", ".json")))
            {
                switch (Save.PipeType)
                {
                    case PipeType.Server:
                        break;
                    case PipeType.Client:
                        CQ_addLog(0, 30, Marshal.StringToHGlobalAnsi("插件载入")
                                            , Marshal.StringToHGlobalAnsi($"插件 {plugininfo.Name} 加载失败,原因:缺少json文件"));
                        break;
                    case PipeType.NoPipe:
                        LogHelper.WriteLog(LogLevel.Error, "插件载入", $"插件 {plugininfo.Name} 加载失败,原因:缺少json文件");
                        break;
                }
                return false;
            }
            JObject json = JObject.Parse(File.ReadAllText(plugininfo.FullName.Replace(".dll", ".json")));
            int authcode;
            switch (Save.PipeType)
            {                
                case PipeType.Client:
                    authcode = Save.AuthCode;
                    break;
                case PipeType.Server:
                case PipeType.NoPipe:
                default:
                    authcode = new Random().Next();
                    break;
            }
            Dll dll = new Dll();
            if (!Directory.Exists(@"data\tmp"))
                Directory.CreateDirectory(@"data\tmp");
            //复制需要载入的插件至临时文件夹,可直接覆盖原dll便于插件重载
            string destpath = @"data\tmp\" + plugininfo.Name;
            if (Save.PipeType != PipeType.Client)
            {
                File.Copy(plugininfo.FullName, destpath, true);
                //复制它的json
                File.Copy(plugininfo.FullName.Replace(".dll", ".json"), destpath.Replace(".dll", ".json"), true);
            }
            IntPtr iLib = dll.Load(destpath, json);//将dll插件LoadLibrary,并进行函数委托的实例化;
            if (iLib == (IntPtr)0)
            {
                switch (Save.PipeType)
                {
                    case PipeType.Server:
                        break;
                    case PipeType.Client:
                        CQ_addLog(0, 30, Marshal.StringToHGlobalAnsi("插件载入")
                                , Marshal.StringToHGlobalAnsi($"插件 {plugininfo.Name} 加载失败,返回句柄为空,GetLastError={Dll.GetLastError()}"));
                        break;
                    case PipeType.NoPipe:
                        LogHelper.WriteLog(LogLevel.Error, "插件载入", $"插件 {plugininfo.Name} 加载失败,返回句柄为空,GetLastError={Dll.GetLastError()}");
                        break;
                }
            }
            //执行插件的init,分配一个authcode
            dll.DoInitialize(authcode);
            //获取插件的appinfo,返回示例 9,me.cqp.luohuaming.Sign,分别为ApiVer以及AppID
            KeyValuePair<int, string> appInfotext = dll.GetAppInfo();
            AppInfo appInfo = new AppInfo(appInfotext.Value, 0, appInfotext.Key
                , json["name"].ToString(), json["version"].ToString(), Convert.ToInt32(json["version_id"].ToString())
                , json["author"].ToString(), json["description"].ToString(), authcode);
            bool enabled = GetPluginState(appInfo);//获取插件启用状态
            if (Save.PipeType==PipeType.Server)
            {
                Process.Start(typeof(MainForm).Assembly.Location, $"-m {appInfotext.Value} {authcode}");
            }
            //保存至插件列表
            Plugins.Add(new Plugin(iLib, appInfo, json, dll, enabled, appInfotext.Value));
            switch (Save.PipeType)
            {
                case PipeType.Server:
                    break;
                case PipeType.Client:
                    CQ_addLog(authcode, 11, Marshal.StringToHGlobalAnsi("插件载入")
                            , Marshal.StringToHGlobalAnsi($"插件 {appInfo.Name} 加载成功"));
                    break;
                case PipeType.NoPipe:
                    LogHelper.WriteLog(LogLevel.InfoSuccess, "插件载入", $"插件 {appInfo.Name} 加载成功");
                    cq_start(Marshal.StringToHGlobalAnsi(destpath), authcode);
                    break;
            }
            //将它的窗口写入托盘右键菜单
            NotifyIconHelper.LoadMenu(json);
            return true;
        }
        /// <summary>
        /// 翻转插件启用状态
        /// </summary>
        public void FlipPluginState(Plugin plugin)
        {
            var c = MainForm.AppConfig["states"]
                            .Where(x => x["Name"].ToString() == plugin.appinfo.Id)
                            .FirstOrDefault();
            c["Enabled"] = c["Enabled"].Value<int>() == 1 ? 0 : 1;
            File.WriteAllText(@"conf\states.json", MainForm.AppConfig.ToString());
        }
        /// <summary>
        /// 从配置获取插件启用状态
        /// </summary>
        /// <param name="appInfo">需要获取的Appinfo</param>
        private bool GetPluginState(AppInfo appInfo)
        {
            //没有states键,新建一个
            if (!(MainForm.AppConfig["states"] is JArray statesArray))
            {
                var b = new JProperty("states", new JArray());
                MainForm.AppConfig.Add(b);
                File.WriteAllText(@"conf\states.json", MainForm.AppConfig.ToString());
                statesArray = MainForm.AppConfig["states"] as JArray;
            }
            //没有此插件的配置,新建一个并返回true
            if (!statesArray.Any(x => x["Name"].ToString() == appInfo.Id))
            {
                JObject plugin = new JObject()
                {
                    new JProperty("Name",appInfo.Id),
                    new JProperty("Enabled",1)
                };
                statesArray.Add(plugin);
                File.WriteAllText(@"conf\states.json", MainForm.AppConfig.ToString());
                return true;
            }
            else
            {
                return statesArray.Where(x => x["Name"].ToString() == appInfo.Id).First()["Enabled"]
                    .Value<int>() == 1;
            }
        }
        /// <summary>
        /// 卸载插件，执行被卸载事件，从菜单移除此插件的菜单
        /// </summary>
        /// <param name="plugin"></param>
        public void UnLoad(Plugin plugin)
        {
            try
            {
                plugin.dll.CallFunction(FunctionEnums.Functions.Disable);
                plugin.dll.CallFunction(FunctionEnums.Functions.Exit);
                plugin.dll.UnLoad();
                NotifyIconHelper.RemoveMenu(plugin.appinfo.Name);
                Plugins.Remove(plugin);
                LogHelper.WriteLog(LogLevel.InfoSuccess, "插件卸载", $"插件 {plugin.appinfo.Name} 卸载成功");
                plugin = null; GC.Collect();
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(LogLevel.Error, "插件卸载", e.Message, e.StackTrace);
            }
        }
        /// <summary>
        /// 插件全部卸载
        /// </summary>
        public void UnLoad()
        {
            LogHelper.WriteLog("开始卸载插件...");
            int max = Plugins.Count;
            for (int i = 0; i < max; i++)
            {
                Plugin item = Plugins[0];
                UnLoad(item);
                item.dll.Dispose();
                GC.Collect();
            }
            LogHelper.WriteLog("插件卸载完毕，框架正在退出...");
        }
        //写在构造函数是不是还好点?
        /// <summary>
        /// 成员初始化，用于删除上次运行的临时目录、加载插件以及执行启动事件
        /// </summary>
        public void Init()
        {
            if (Directory.Exists(@"data\tmp"))
                Directory.Delete(@"data\tmp", true);
            Dll.LoadLibrary("CQP.dll");
            if (MainForm.AppConfig.Count == 0)
            {
                MainForm.AppConfig.Add(new JProperty("states", new JArray()));
                File.WriteAllText(@"conf\states.json", MainForm.AppConfig.ToString());
            }
            Load();
            LogHelper.WriteLog("遍历启动事件……");
            CallFunction(FunctionEnums.Functions.StartUp);
            CallFunction(FunctionEnums.Functions.Enable);
        }
        [DllImport("CQP.dll", EntryPoint = "cq_start")]
        private static extern bool cq_start(IntPtr path, int authcode);
        [DllImport("CQP.dll", EntryPoint = "CQ_addLog")]
        private static extern int CQ_addLog(int authCode, int priority, IntPtr type, IntPtr msg);
        /// <summary>
        /// 核心方法调用，将前端处理的数据传递给插件对应事件处理，尝试捕获非托管插件的异常
        /// </summary>
        /// <param name="ApiName">调用的事件名称，前端统一名称，或许应该写成枚举</param>
        /// <param name="args">参数表</param>
        [HandleProcessCorruptedStateExceptions]
        public void CallFunction(FunctionEnums.Functions ApiName, params object[] args)
        {
            JObject json = new JObject
            {
                new JProperty("ApiName",JsonConvert.SerializeObject(ApiName)),
                new JProperty("Args",JsonConvert.SerializeObject(args))
            };
            //遍历插件列表,遇到标记消息阻断则跳出
            foreach (var item in Plugins)
            {
                Dll dll = item.dll;
                //先看此插件是否已禁用
                if (item.Enable is false) continue;
                if (Save.TestPluginsList.Any(x => x == item.appinfo.Name))
                {
                    Debug.WriteLine($"{item.appinfo.Name} 插件测试中，忽略消息投递");
                    continue;
                }
                try
                {
                    int result;
                    if (Save.MutiProcessMode is true)
                    {
                        result = item.NamedPipe.SendMsg(json.ToString());
                    }
                    else
                    {
                        result = dll.CallFunction(ApiName, args);
                    }
                    //调用函数, 返回 1 表示消息阻塞, 跳出后续
                    if (result == 1)
                    {
                        LogHelper.WriteLog($"由 {item.appinfo.Name} 结束消息处理");
                        return;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.WriteLog(LogLevel.Error, "函数执行异常", $"插件 {item.appinfo.Name} {ApiName} 函数发生错误，错误信息:{e.Message} {e.StackTrace}");
                    var b = ErrorHelper.ShowErrorDialog($"错误模块：{item.appinfo.Name}\n{ApiName} 函数发生错误，错误信息:\n{e.Message} {e.StackTrace}");
                    switch (b)
                    {
                        case ErrorHelper.TaskDialogResult.ReloadApp:
                            ReLoad();
                            break;
                        case ErrorHelper.TaskDialogResult.Exit:
                            NotifyIconHelper.HideNotifyIcon();
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 重载应用
        /// 未找到去除文件占用的方法，只能重启程序来实现重载了
        /// </summary>
        public void ReLoad()
        {
            UnLoad();
            NotifyIconHelper.HideNotifyIcon();
            //Load();
            string path = typeof(MainForm).Assembly.Location;//获取可执行文件路径
            if (Save.IgnoreProcessChecking)
            {
                Process.Start(path, "-i");
            }
            else
            {
                Process.Start(path, "-r");//再次运行程序
            }
            Environment.Exit(0);//关闭当前程序
        }
    }
}
