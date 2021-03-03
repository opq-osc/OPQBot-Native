using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace Deserizition
{
    public enum LogLevel
    {
        /// <summary>
        /// 调试.		颜色: 灰色
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 信息.		颜色: 黑色
        /// </summary>
        Info = 10,
        /// <summary>
        /// 信息 (成功)	颜色: 紫色
        /// </summary>
        InfoSuccess = 11,
        /// <summary>
        /// 信息 (接收)	颜色: 蓝色
        /// </summary>
        InfoReceive = 12,
        /// <summary>
        /// 信息 (发送)	颜色: 绿色
        /// </summary>
        InfoSend = 13,
        /// <summary>
        /// 警告			颜色: 橙色
        /// </summary>
        Warning = 20,
        /// <summary>
        /// 错误			颜色: 红色
        /// </summary>
        Error = 30,
        /// <summary>
        /// 致命错误		颜色: 深红色
        /// </summary>
        Fatal = 40
    }
    public static class LogHelper
    {
        public static string GetLogFileName()
        {
            var fileinfo = new DirectoryInfo($@"logs\{Save.curentQQ}").GetFiles("*.db");
            string filename = "";
            foreach (var item in fileinfo)
            {
                if (item.Name.StartsWith($"logv2_{DateTime.Now:yyMM}"))
                {
                    filename = item.Name;
                    break;
                }
            }
            return string.IsNullOrWhiteSpace(filename) ? $"logv2_{DateTime.Now:yyMMdd}.db" : filename;
        }
        public static string GetLogFilePath()
        {
            if (Directory.Exists($@"logs\{Save.curentQQ}") is false)
                Directory.CreateDirectory($@"logs\{Save.curentQQ}");
            return Path.Combine(Environment.CurrentDirectory, $@"logs\{Save.curentQQ}", GetLogFileName());
        }
        private static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"data source={GetLogFilePath()}",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            return db;
        }
        public static void CreateDB()
        {
            using (var db = GetInstance())
            {
                string DBPath = GetLogFilePath();
                db.DbMaintenance.CreateDatabase(DBPath);
                db.CodeFirst.InitTables(typeof(LogModel));
            }
            WriteLog(LogLevel.InfoSuccess, "运行日志", $"日志数据库初始化完毕{DateTime.Now:yyMMdd}。");
        }
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        public static DateTime TimeStamp2DateTime(long Timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Timestamp);
        }
        public static string GetTimeStampString(long Timestamp)
        {
            DateTime time = TimeStamp2DateTime(Timestamp);
            StringBuilder sb = new StringBuilder();
            //sb.Append($"{(time.AddDays(1).Day == DateTime.Now.Day ? "昨天" : "今天")} ");
            sb.Append($"{time:MM/dd HH:mm:ss}");
            return sb.ToString();
        }
        public static int WriteLog(LogLevel level, string logOrigin, string type, string messages, string status = "")
        {
            LogModel model = new LogModel
            {
                detail = messages,
                id = 0,
                source = logOrigin,
                priority = (int)level,
                name = type,
                time = GetTimeStamp(),
                status = status
            };
            return WriteLog(model);
        }
        public static int WriteLog(LogModel model)
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseTran(() =>
                {
                    db.Insertable(model).ExecuteCommand();
                });
                if (result.IsSuccess is false)
                {
                    throw new Exception("执行错误，发生位置 WriteLog " + result.ErrorMessage);
                }
            }
            int logid = GetLastLog().id;
            SendBroadcast(GetLogBroadcastContent(logid));
            return logid;
        }
        private static string GetLogBroadcastContent(int logid)
        {
            JObject json = new JObject
            {
                new JProperty("Type","Log"),
                new JProperty("QQ",Save.curentQQ),
                new JProperty("LogID",logid),
            };
            return json.ToString();
        }
        private static string GetUpdateBroadcastContent(int logid, string msg)
        {
            JObject json = new JObject
            {
                new JProperty("Type","Update"),
                new JProperty("QQ",Save.curentQQ),
                new JProperty("LogID",logid),
                new JProperty("Msg",msg)
            };
            return json.ToString();
        }
        private static void SendBroadcast(string msg)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
               ProtocolType.Udp);
            IPEndPoint iep1 = new IPEndPoint(IPAddress.Broadcast, Save.BoardCastPort);//255.255.255.255
            byte[] data = Encoding.UTF8.GetBytes(msg);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            sock.SendTo(data, iep1);
            sock.Close();
        }
        public static int WriteLog(int level, string logOrigin, string type, string messages, string status = "")
        {
            LogLevel loglevel = (LogLevel)Enum.Parse(typeof(LogLevel), Enum.GetName(typeof(LogLevel), level));
            return WriteLog(loglevel, logOrigin, type, messages, status);
        }
        public static int WriteLog(LogLevel level, string type, string message, string status = "")
        {
            return WriteLog(level, "OPQBot框架", type, message, status);
        }
        /// <summary>
        /// 以info为等级，"OPQBot框架"为来源，"提示"为类型写出一条日志
        /// </summary>
        /// <param name="messages">日志内容</param>
        public static int WriteLog(string messages, string status = "")
        {
            return WriteLog(LogLevel.Info, "OPQBot框架", "提示", messages, status);
        }

        public static LogModel GetLogByID(int id)
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseTran(() =>
                {
                    return db.Queryable<LogModel>().First(x => x.id == id);
                });
                if (result.IsSuccess)
                {
                    return result.Data;
                }
                else
                {
                    throw new Exception("执行错误，发生位置 GetLogByID " + result.ErrorMessage);
                }
            }
        }
        public static void UpdateLogStatus(int id, string status)
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseTran(() =>
                {
                    db.Updateable<LogModel>().SetColumns(x => x.status == status).Where(x => x.id == id)
                      .ExecuteCommand();
                });
                if (result.IsSuccess)
                {
                    SendBroadcast(GetUpdateBroadcastContent(id, status));
                }
                else
                {
                    throw new Exception("执行错误，发生位置 UpdateLogStatus " + result.ErrorMessage);
                }
            }
        }
        public static List<LogModel> GetDisplayLogs(int priority)
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseTran(() =>
                {
                    var c = db.SqlQueryable<LogModel>($"select * from log where priority>= {priority} order by id desc limit {Save.LogerMaxCount}").ToList();
                    c.Reverse();
                    return c;
                });
                if (result.IsSuccess)
                {
                    return result.Data;
                }
                else
                {
                    throw new Exception("执行错误，发生位置 UpdateLogStatus " + result.ErrorMessage);
                }
            }
        }
        public static LogModel GetLastLog()
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseTran(() =>
                {
                    return db.SqlQueryable<LogModel>("select * from log order by id desc limit 1").First();
                });
                if (result.IsSuccess)
                {
                    return result.Data;
                }
                else
                {
                    throw new Exception("执行错误，发生位置 GetLastLog " + result.ErrorMessage);
                }
            }
        }
    }
}
