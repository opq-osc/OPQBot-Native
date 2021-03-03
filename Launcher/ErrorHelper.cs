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
        public static TaskDialogResult ShowErrorDialog(string msg)
        {
            TaskDialogOptions config = new TaskDialogOptions
            {
                Title = $"OPQBot-Native {Application.ProductVersion}",
                MainInstruction = "OPQBot-Native 发生错误",
                Content = $"很抱歉，应用发生错误，需要重新载入应用。\n{msg}",
                CommandButtons = new string[] { "复制错误详情信息", "重新载入应用", "忽略此次错误\n如果此问题频繁出现，可停用所有应用便于排查", "关闭 OPQBot-Native" },
                MainIcon = VistaTaskDialogIcon.BigError
            };
            //System.Media.SystemSounds.Hand.Play();
            var res = TaskDialog.Show(config);
            switch (res.CommandButtonResult)
            {
                case 0:
                    Clipboard.SetText(msg);
                    return TaskDialogResult.Copy;
                case 2:
                    return TaskDialogResult.Ignore;
                case 1:
                    return TaskDialogResult.ReloadApp;
                case 3:
                    return TaskDialogResult.Exit;
                default:
                    return TaskDialogResult.Exit;
            }
        }
        public enum TaskDialogResult
        {
            Copy,
            Ignore,
            ReloadApp,
            Exit
        }
    }
}
