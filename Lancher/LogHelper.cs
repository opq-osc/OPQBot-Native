using Lancher.Sdk.Cqp.Enum;
using System;
using System.Runtime.InteropServices;

namespace Lancher
{
    public static class LogHelper
    {
        public static void WriteLine(params string[] messages)
        {
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            WriteLine(CQLogLevel.Info, "提示", msg);
        }
        public static void WriteLine(CQLogLevel level,string type, params string[] messages)
        {
            string msg = string.Empty;
            foreach (var item in messages)
                msg += item;
            switch (level)
            {
                case CQLogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case CQLogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CQLogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case CQLogLevel.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case CQLogLevel.InfoSuccess:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case CQLogLevel.InfoSend:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CQLogLevel.InfoReceive:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case CQLogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][OPQBot框架 {type}]{msg}");
        }
    }
}
