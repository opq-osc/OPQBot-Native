using System;
using System.Text;
using System.IO.Pipes;
using System.Diagnostics;
using System.IO;

namespace Launcher.Pipe
{
    public class NamedPipeServer
    {
        private static NamedPipeServerStream pipeServer { get; set; }
        /// <summary>
        /// 服务端的构造函数
        /// </summary>
        /// <param name="pipeName">命名管道名称</param>
        public NamedPipeServer(string pipeName)
        {
            pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
            Debug.WriteLine($"名称为 {pipeName} 的管道服务器已创建");
            Connect();
        }
        public void Connect()
        {
            // Wait for a client to connect
            Debug.WriteLine("等待客户端连接...");
            pipeServer.WaitForConnection();
            Debug.WriteLine("客户端已连接，开始发送消息");
        }
        public void Disconnect()
        {
            pipeServer.Disconnect();
            pipeServer.Dispose();
            GC.Collect();
        }
        public int SendMsg(string msg)
        {
            if (!pipeServer.IsConnected)
            {
                throw new IOException("管道已关闭或未连接，无法发送消息");
            }
            Encoding GB18030 = Encoding.GetEncoding("GB18030");
            try
            {
                StreamWriter sw = new StreamWriter(pipeServer);
                sw.AutoFlush = true;
                byte[] s = Encoding.Convert(Encoding.Default, GB18030, Encoding.Default.GetBytes(msg));
                sw.WriteLine(GB18030.GetString(s));
                return 0;
            }
            catch (IOException e)
            {
                Debug.WriteLine($"发生错误: {e.Message}\n{e.StackTrace}");
                throw e;
            }
        }
    }
}
