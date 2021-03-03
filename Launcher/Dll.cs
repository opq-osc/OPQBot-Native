using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Deserizition;
using Newtonsoft.Json.Linq;

namespace Launcher
{
    [Serializable]
    public class Dll : IDisposable
    {
        #region ----dll函数 委托原型声明部分----
        [DllImport("kernel32.dll")]
        public extern static IntPtr LoadLibrary(String path);

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);

        [DllImport("kernel32.dll")]
        public extern static int GetLastError();

        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        [DllImport("CQP.dll", EntryPoint = "AddMsgToSave")]
        public extern static bool AddMsgToSave(Deserizition.Message msg);

        [DllImport("CQP.dll", EntryPoint = "AddRequestToSave")]
        public extern static bool AddRequestToSave(Deserizition.Requests request);

        private IntPtr hLib;
        #endregion

        public IntPtr Load(string filepath, JObject json)
        {
            if (hLib == null || hLib == (IntPtr)0)
            {
                try { hLib = LoadLibrary(filepath); }
                catch
                {
                    LogHelper.WriteLog(LogLevel.Error, "Error", $"插件 {filepath} 载入失败,LoadLibrary :GetLastError={GetLastError()}");
                }
            }
            if (hLib != (IntPtr)0)
            {
                return hLib;
            }
            return (IntPtr)0;
        }
        public void UnLoad()
        {
            Dispose();
        }
        public void Dispose()
        {
            Proxy.FreeLibrary(hLib);
        }
        /// <summary>
        /// 将要执行的函数转换为委托
        /// </summary>
        /// <param name="APIName">函数名称</param>
        /// <param name="t">需要转换成委托的类型</param>
        public Delegate Invoke(string APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            if (api == (IntPtr)0)
                return null;
            return Marshal.GetDelegateForFunctionPointer(api, t);
        }
        public int DoInitialize(int authcode)
        {
            Proxy.Initialize(hLib, authcode);
            return 0;
        }
        [HandleProcessCorruptedStateExceptions]
        public KeyValuePair<int, string> GetAppInfo()
        {
            string appinfo = Marshal.PtrToStringAnsi(Proxy.AppInfo(hLib));
            string[] pair = appinfo.Split(',');
            if (pair.Length != 2)
                throw new Exception("获取AppInfo信息失败");
            KeyValuePair<int, string> valuePair = new KeyValuePair<int, string>(Convert.ToInt32(pair[0]), pair[1]);
            return valuePair;
        }
        public int CallFunction(FunctionEnums.Functions ApiName, params object[] args)
        {
            return Proxy.CallFunction(hLib, ApiName, args);
        }
    }
    public static class Proxy
    {
        public static Dictionary<IntPtr, Transform> DelegateSave { get; set; } = new Dictionary<IntPtr, Transform>();
        public class Transform : MarshalByRefObject
        {
            public Transform(AppDomain currentDomain)
            {
                CurrentDomain = currentDomain;
            }
            public AppDomain CurrentDomain { get; set; }

            private delegate IntPtr Type_AppInfo();
            private delegate int Type_Initialize(int authcode);
            private delegate int Type_PrivateMsg(int subType, int msgId, long fromQQ, IntPtr msg, int font);
            private delegate int Type_GroupMsg(int subType, int msgId, long fromGroup, long fromQQ, string fromAnonymous, IntPtr msg, int font);
            private delegate int Type_Upload(int subType, int sendTime, long fromGroup, long fromQQ, string file);
            private delegate int Type_AdminChange(int subType, int sendTime, long fromGroup, long beingOperateQQ);
            private delegate int Type_GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ);
            private delegate int Type_GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ);
            private delegate int Type_GroupBan(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ, long duration);
            private delegate int Type_FriendAdded(int subType, int sendTime, long fromQQ);
            private delegate int Type_FriendRequest(int subType, int sendTime, long fromQQ, IntPtr msg, string responseFlag);
            private delegate int Type_GroupAddRequest(int subType, int sendTime, long fromGroup, long fromQQ, IntPtr msg, string responseFlag);
            private delegate int Type_Startup();
            private delegate int Type_Exit();
            private delegate int Type_Enable();
            private delegate int Type_Disable();

            private Type_AppInfo AppInfo;
            private Type_Initialize Initialize;
            private Type_PrivateMsg PrivateMsg;
            private Type_GroupMsg GroupMsg;
            private Type_Upload Upload;
            private Type_AdminChange AdminChange;
            private Type_GroupMemberDecrease GroupMemberDecrease;
            private Type_GroupMemberIncrease GroupMemberIncrease;
            private Type_GroupBan GroupBan;
            private Type_FriendAdded FriendAdded;
            private Type_FriendRequest FriendRequest;
            private Type_GroupAddRequest GroupAddRequest;
            private Type_Startup Startup;
            private Type_Exit Exit;
            private Type_Enable Enable;
            private Type_Disable Disable;

            public void Init(IntPtr hLib, string jsonStr)
            {
                CurrentDomain.DoCallBack(() =>
                {
                    JObject json = JObject.Parse(jsonStr);
                    Initialize = (Type_Initialize)Invoke(hLib, "Initialize", typeof(Type_Initialize));
                    AppInfo = (Type_AppInfo)Invoke(hLib, "AppInfo", typeof(Type_AppInfo));
                    foreach (var item in JArray.Parse(json["event"].ToString()))
                    {
                        switch (item["id"].ToString())
                        {
                            case "1":
                                PrivateMsg = (Type_PrivateMsg)Invoke(hLib, item["function"].ToString(), typeof(Type_PrivateMsg));
                                break;
                            case "2":
                                GroupMsg = (Type_GroupMsg)Invoke(hLib, item["function"].ToString(), typeof(Type_GroupMsg));
                                break;
                            case "4":
                                Upload = (Type_Upload)Invoke(hLib, item["function"].ToString(), typeof(Type_Upload));
                                break;
                            case "5":
                                AdminChange = (Type_AdminChange)Invoke(hLib, item["function"].ToString(), typeof(Type_AdminChange));
                                break;
                            case "6":
                                GroupMemberDecrease = (Type_GroupMemberDecrease)Invoke(hLib, item["function"].ToString(), typeof(Type_GroupMemberDecrease));
                                break;
                            case "7":
                                GroupMemberIncrease = (Type_GroupMemberIncrease)Invoke(hLib, item["function"].ToString(), typeof(Type_GroupMemberIncrease));
                                break;
                            case "8":
                                GroupBan = (Type_GroupBan)Invoke(hLib, item["function"].ToString(), typeof(Type_GroupBan));
                                break;
                            case "10":
                                FriendAdded = (Type_FriendAdded)Invoke(hLib, item["function"].ToString(), typeof(Type_FriendAdded));
                                break;
                            case "11":
                                FriendRequest = (Type_FriendRequest)Invoke(hLib, item["function"].ToString(), typeof(Type_FriendRequest));
                                break;
                            case "12":
                                GroupAddRequest = (Type_GroupAddRequest)Invoke(hLib, item["function"].ToString(), typeof(Type_GroupAddRequest));
                                break;
                            case "1001":
                                Startup = (Type_Startup)Invoke(hLib, item["function"].ToString(), typeof(Type_Startup));
                                break;
                            case "1002":
                                Exit = (Type_Exit)Invoke(hLib, item["function"].ToString(), typeof(Type_Exit));
                                break;
                            case "1003":
                                Enable = (Type_Enable)Invoke(hLib, item["function"].ToString(), typeof(Type_Enable));
                                break;
                            case "1004":
                                Disable = (Type_Disable)Invoke(hLib, item["function"].ToString(), typeof(Type_Disable));
                                break;
                        }
                    }
                });
            }
            [DllImport("kernel32.dll")]
            private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
            [DllImport("kernel32.dll")]
            private extern static bool FreeLibrary(IntPtr lib);

            /// <summary>
            /// 将要执行的函数转换为委托
            /// </summary>
            /// <param name="APIName">函数名称</param>
            /// <param name="t">需要转换成委托的类型</param>
            public Delegate Invoke(IntPtr hLib, string APIName, Type t)
            {
                Delegate returnValue = null;
                CurrentDomain.DoCallBack(() =>
                {
                    IntPtr api = GetProcAddress(hLib, APIName);
                    if (api == (IntPtr)0)
                    {
                        returnValue = null; 
                        return; 
                    }
                    returnValue = Marshal.GetDelegateForFunctionPointer(api, t);
                });
                return returnValue;
            }
            public int CallFunction(FunctionEnums.Functions ApiName, params object[] args)
            {
                int returnValue = 0;
                CurrentDomain.DoCallBack(() =>
                {
                    switch (ApiName)
                    {
                        case FunctionEnums.Functions.PrivateMsg:
                            if (PrivateMsg == null)
                            { returnValue = -1; break; }
                            returnValue = PrivateMsg(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), (IntPtr)args[3], 1);
                            break;
                        case FunctionEnums.Functions.GroupMsg:
                            if (GroupMsg == null)
                            { returnValue = -1; break; }
                            returnValue = GroupMsg(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3])
                                , args[4].ToString(), (IntPtr)args[5], 1);
                            break;
                        case FunctionEnums.Functions.Upload:
                            if (Upload == null)
                            { returnValue = -1; break; }
                            returnValue = Upload(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), args[4].ToString());
                            break;
                        case FunctionEnums.Functions.AdminChange:
                            if (AdminChange == null)
                            { returnValue = -1; break; }
                            returnValue = AdminChange(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]));
                            break;
                        case FunctionEnums.Functions.GroupMemberDecrease:
                            if (GroupMemberDecrease == null)
                            { returnValue = -1; break; }
                            returnValue = GroupMemberDecrease(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]));
                            break;
                        case FunctionEnums.Functions.GroupMemberIncrease:
                            if (GroupMemberIncrease == null)
                            { returnValue = -1; break; }
                            returnValue = GroupMemberIncrease(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]));
                            break;
                        case FunctionEnums.Functions.GroupBan:
                            if (GroupBan == null)
                            { returnValue = -1; break; }
                            returnValue = GroupBan(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]), Convert.ToInt64(args[5]));
                            break;
                        case FunctionEnums.Functions.FriendAdded:
                            if (FriendAdded == null)
                            { returnValue = -1; break; }
                            returnValue = FriendAdded(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]));
                            break;
                        case FunctionEnums.Functions.FriendRequest:
                            if (FriendRequest == null)
                            { returnValue = -1; break; }
                            returnValue = FriendRequest(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Marshal.StringToHGlobalAnsi(args[3].ToString()), args[4].ToString());
                            break;
                        case FunctionEnums.Functions.GroupAddRequest:
                            if (GroupAddRequest == null)
                            { returnValue = -1; break; }
                            returnValue = GroupAddRequest(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Marshal.StringToHGlobalAnsi(args[4].ToString()), args[5].ToString());
                            break;
                        case FunctionEnums.Functions.StartUp:
                            if (Startup == null)
                            { returnValue = -1; break; }
                            returnValue = Startup();
                            break;
                        case FunctionEnums.Functions.Exit:
                            if (Exit == null)
                            { returnValue = -1; break; }
                            returnValue = Exit();
                            break;
                        case FunctionEnums.Functions.Enable:
                            if (Enable == null)
                            { returnValue = -1; break; }
                            returnValue = Enable();
                            break;
                        case FunctionEnums.Functions.Disable:
                            if (Disable == null)
                            { returnValue = -1; break; }
                            returnValue = Disable();
                            break;
                        default:
                            LogHelper.WriteLog(LogLevel.Error, "事件分发", "未找到或者未实现的事件");
                            returnValue = -1;
                            break;
                    }
                });
                return returnValue;
            }
            public IntPtr CallAppinfo()
            {
                IntPtr returnvalue = (IntPtr)0;
                CurrentDomain.DoCallBack(() => { returnvalue = AppInfo(); });
                return returnvalue;
            }
            public int CallInitialize(int authCode)
            {
                int returnvalue = 0;
                CurrentDomain.DoCallBack(() => { returnvalue = Initialize(authCode); });
                return returnvalue;
            }
            public void CallFreeLibrary(IntPtr hLib)
            {
                CurrentDomain.DoCallBack(() =>
                {
                    FreeLibrary(hLib);
                });
            }
        }
        public static int CallFunction(IntPtr dll, FunctionEnums.Functions api, params object[] args)
        {
            return DelegateSave[dll].CallFunction(api, args);
        }
        public static IntPtr AppInfo(IntPtr dll)
        {
            return DelegateSave[dll].CallAppinfo();
        }
        public static int Initialize(IntPtr dll, int authCode)
        {
            return DelegateSave[dll].CallInitialize(authCode);
        }

        public static void Init(IntPtr hLib, JObject json)
        {
            var c = PluginManagment.AppDomainSave[hLib];
            Transform transform = (Transform)c.CreateInstanceAndUnwrap(typeof(Transform).Assembly.FullName
                , typeof(Transform).FullName
                , false
                , BindingFlags.CreateInstance
                , null
                , new object[] { c }
                , null, null);
            transform.Init(hLib, json.ToString());
            DelegateSave.Add(hLib, transform);
        }
        public static void FreeLibrary(IntPtr dll)
        {
            DelegateSave[dll].CallFreeLibrary(dll);
            DelegateSave.Remove(dll);
        }
    }
}
