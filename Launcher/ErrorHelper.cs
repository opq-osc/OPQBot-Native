using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskDialogInterop;

namespace Launcher
{
    public static class ErrorHelper
    {
        /// <summary>
        /// 显示TaskDialog风格窗口
        /// </summary>
        /// <param name="msg">需要折叠的错误</param>
        /// <returns>true 表示重载 false 表示退出</returns>
        public static TaskDialogResult ShowErrorDialog(string msg, bool Startable=true)
        {
            string[] buttons = new string[] { };
            string content;
            if (Startable)
            {
                content = $"很抱歉，应用发生错误，但是这个错误可以被忽略。\n{msg}";
                buttons = new string[] { "复制错误详情信息", "重新载入应用", "忽略此次错误\n如果此问题频繁出现，可停用所有应用便于排查", "关闭 OPQBot-Native" };
            }
            else
            {
                content = $"很抱歉，应用发生错误，需要关闭框架后重新启动。\n{msg}";
                buttons = new string[] { "复制错误详情信息\n之后会关闭程序", "重启 OPQBot-Native", "退出 OPQBot-Native" };
            }
            TaskDialogOptions config = new TaskDialogOptions
            {
                Title = $"OPQBot-Native {Application.ProductVersion}",
                MainInstruction = "OPQBot-Native 发生错误",
                Content = content,
                CommandButtons = buttons,
                MainIcon = VistaTaskDialogIcon.BigError,                
            };
            System.Media.SystemSounds.Hand.Play();
            var res = TaskDialog.Show(config);
            switch (res.CommandButtonResult)
            {
                case 0:
                    Clipboard.SetText(msg);
                    return TaskDialogResult.Copy;
                case 1:
                    if (Startable)
                        return TaskDialogResult.ReloadApp;
                    else
                        return TaskDialogResult.Restart;
                case 2:
                    if (Startable)
                        return TaskDialogResult.Ignore;
                    else
                        return TaskDialogResult.Exit;
                case 3:
                    return TaskDialogResult.Exit;
                default:
                    return TaskDialogResult.Ignore;
            }
        }
        public enum TaskDialogResult
        {
            Copy,
            Ignore,
            ReloadApp,
            Exit,
            Restart
        }
    }
}
