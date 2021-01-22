using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Deserizition;
using Newtonsoft.Json.Linq;

namespace Launcher
{
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

        [DllImport("Kernel32")]
        private extern static bool CloseHandle(IntPtr handle);

        private IntPtr hLib;

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
                Initialize = (Type_Initialize)Invoke("Initialize", typeof(Type_Initialize));
                AppInfo = (Type_AppInfo)Invoke("AppInfo", typeof(Type_AppInfo));
                foreach (var item in JArray.Parse(json["event"].ToString()))
                {
                    switch (item["id"].ToString())
                    {
                        case "1":
                            PrivateMsg = (Type_PrivateMsg)Invoke(item["function"].ToString(), typeof(Type_PrivateMsg));
                            break;
                        case "2":
                            GroupMsg = (Type_GroupMsg)Invoke(item["function"].ToString(), typeof(Type_GroupMsg));
                            break;
                        case "4":
                            Upload = (Type_Upload)Invoke(item["function"].ToString(), typeof(Type_Upload));
                            break;
                        case "5":
                            AdminChange = (Type_AdminChange)Invoke(item["function"].ToString(), typeof(Type_AdminChange));
                            break;
                        case "6":
                            GroupMemberDecrease = (Type_GroupMemberDecrease)Invoke(item["function"].ToString(), typeof(Type_GroupMemberDecrease));
                            break;
                        case "7":
                            GroupMemberIncrease = (Type_GroupMemberIncrease)Invoke(item["function"].ToString(), typeof(Type_GroupMemberIncrease));
                            break;
                        case "8":
                            GroupBan = (Type_GroupBan)Invoke(item["function"].ToString(), typeof(Type_GroupBan));
                            break;
                        case "10":
                            FriendAdded = (Type_FriendAdded)Invoke(item["function"].ToString(), typeof(Type_FriendAdded));
                            break;
                        case "11":
                            FriendRequest = (Type_FriendRequest)Invoke(item["function"].ToString(), typeof(Type_FriendRequest));
                            break;
                        case "12":
                            GroupAddRequest = (Type_GroupAddRequest)Invoke(item["function"].ToString(), typeof(Type_GroupAddRequest));
                            break;
                        case "1001":
                            Startup = (Type_Startup)Invoke(item["function"].ToString(), typeof(Type_Startup));
                            break;
                        case "1002":
                            Exit = (Type_Exit)Invoke(item["function"].ToString(), typeof(Type_Exit));
                            break;
                        case "1003":
                            Enable = (Type_Enable)Invoke(item["function"].ToString(), typeof(Type_Enable));
                            break;
                        case "1004":
                            Disable = (Type_Disable)Invoke(item["function"].ToString(), typeof(Type_Disable));
                            break;
                    }
                }
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
            Console.WriteLine("调用IDisposable接口");
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Dll()
        {
            Console.WriteLine("调用析构");
            Dispose(false);
        }
        private void Dispose(bool flag)
        {
            if (flag)
            {
                AppInfo = null;
                Initialize = null;
                PrivateMsg = null;
                GroupMsg = null;
                Upload = null;
                AdminChange = null;
                GroupMemberDecrease = null;
                GroupMemberIncrease = null;
                GroupBan = null;
                FriendAdded = null;
                FriendRequest = null;
                GroupAddRequest = null;
                Startup = null;
                Exit = null;
                Enable = null;
                Disable = null;
            }
            if (hLib != IntPtr.Zero)
            {
                IntPtr c;
                do
                {
                    if (!FreeLibrary(hLib))
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    c = GetProcAddress(hLib, "AppInfo");
                } while (c != IntPtr.Zero);
                hLib = IntPtr.Zero;
            }
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
            Initialize(authcode);
            return 0;
        }
        [HandleProcessCorruptedStateExceptions]
        public KeyValuePair<int, string> GetAppInfo()
        {
            string appinfo = Marshal.PtrToStringAnsi(AppInfo());
            string[] pair = appinfo.Split(',');
            if (pair.Length != 2)
                throw new Exception("获取AppInfo信息失败");
            KeyValuePair<int, string> valuePair = new KeyValuePair<int, string>(Convert.ToInt32(pair[0]), pair[1]);
            return valuePair;
        }
        public int CallFunction(FunctionEnums.Functions ApiName, params object[] args)
        {
            switch (ApiName)
            {
                case FunctionEnums.Functions.PrivateMsg:
                    if (PrivateMsg == null)
                        return -1;
                    return PrivateMsg(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), (IntPtr)args[3], 1);
                case FunctionEnums.Functions.GroupMsg:
                    if (GroupMsg == null)
                        return -1;
                    return GroupMsg(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3])
                        , args[4].ToString(), (IntPtr)args[5], 1);
                case FunctionEnums.Functions.Upload:
                    if (Upload == null)
                        return -1;
                    return Upload(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), args[4].ToString());
                case FunctionEnums.Functions.AdminChange:
                    if (AdminChange == null)
                        return -1;
                    return AdminChange(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]));
                case FunctionEnums.Functions.GroupMemberDecrease:
                    if (GroupMemberDecrease == null)
                        return -1;
                    return GroupMemberDecrease(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]));
                case FunctionEnums.Functions.GroupMemberIncrease:
                    if (GroupMemberIncrease == null)
                        return -1;
                    return GroupMemberIncrease(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]));
                case FunctionEnums.Functions.GroupBan:
                    if (GroupBan == null)
                        return -1;
                    return GroupBan(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Convert.ToInt64(args[4]), Convert.ToInt64(args[5]));
                case FunctionEnums.Functions.FriendAdded:
                    if (FriendAdded == null)
                        return -1;
                    return FriendAdded(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]));
                case FunctionEnums.Functions.FriendRequest:
                    if (FriendRequest == null)
                        return -1;
                    return FriendRequest(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Marshal.StringToHGlobalAnsi(args[3].ToString()), args[4].ToString());
                case FunctionEnums.Functions.GroupAddRequest:
                    if (GroupAddRequest == null)
                        return -1;
                    return GroupAddRequest(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), Convert.ToInt64(args[2]), Convert.ToInt64(args[3]), Marshal.StringToHGlobalAnsi(args[4].ToString()), args[5].ToString());
                case FunctionEnums.Functions.StartUp:
                    if (Startup == null)
                        return -1;
                    return Startup();
                case FunctionEnums.Functions.Exit:
                    if (Exit == null)
                        return -1;
                    return Exit();
                case FunctionEnums.Functions.Enable:
                    if (Enable == null)
                        return -1;
                    return Enable();
                case FunctionEnums.Functions.Disable:
                    if (Disable == null)
                        return -1;
                    return Disable();
                default:
                    LogHelper.WriteLog(LogLevel.Error, "事件分发", "未找到或者未实现的事件");
                    return -1;
            }
        }
    }
}
