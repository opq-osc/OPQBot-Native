using System.IO.Pipes;
using System.IO;
using Deserizition;
using System.Diagnostics;
using System;

namespace Launcher.Pipe
{
    public class NamedPipeClient
    {
        public static PluginManagment PluginManagment = new PluginManagment();
        public NamedPipeClient()
        {
            if (PluginManagment.Load($@"data\tmp\{Save.NamedPipeName}.dll") is false)
            {
                Environment.Exit(-1);
            }
        }
        public static bool EndFlag { get; set; } = false;
        public void ReceiveMsg()
        {
            using (NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", Save.NamedPipeName, PipeDirection.InOut))
            {
                // Connect to the pipe or wait until the pipe is available.
                Debug.WriteLine($"尝试通过 {Save.NamedPipeName} 名称连接到管道服务器...");
                pipeClient.Connect();

                Debug.WriteLine("成功连接到管道服务器, 开始接受并处理消息");
                Debug.WriteLine("当前服务器有 {0} 个共享名称的管道服务器",
                   pipeClient.NumberOfServerInstances);
                StreamReader sr = new StreamReader(pipeClient);
                string msg;
                while (true)
                {
                    msg = sr.ReadLine();
                    Debug.WriteLine($"来自服务器的消息: {msg}");

                    if (EndFlag is true)
                    {
                        Debug.WriteLine($"主动放弃管道连接，客户端正在关闭...");
                        break;
                    }
                }
            }
        }
        public static int HandleMsg(FunctionEnums.Functions ApiName, params object[] args)
        {
            return 0;
        }
    }
}
