﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Deserizition;
using Launcher.Forms;
using Newtonsoft.Json.Linq;

namespace Launcher
{
    /// <summary>
    /// 托盘帮助类
    /// </summary>
    public static class NotifyIconHelper
    {
        public static NotifyIcon _NotifyIcon { get; set; }
        public static void Init()
        {
            _NotifyIcon.Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("Launcher.Resources.Icon.ico"));
            _NotifyIcon.ContextMenu = new ContextMenu();
            ContextMenu menu = _NotifyIcon.ContextMenu;
            menu.MenuItems.Add(new MenuItem() { Text = Save.name, Name = "UserName" });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(new MenuItem() { Text = "应用", Name = "PluginMenu" });
            menu.MenuItems.Add(new MenuItem() { Text = "日志", Tag = "LogForm" });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(new MenuItem() { Text = "显示悬浮窗", Tag = "Displaywindow", Checked= MainForm.FormBackup.ShowFlag });
            menu.MenuItems.Add(new MenuItem() { Text = "窗口置顶", Tag = "TopMost", Checked = MainForm.FormBackup.TopFlag });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(new MenuItem() { Text = "重载应用", Tag = "ReLoad" });
            menu.MenuItems.Add(new MenuItem() { Text = "退出", Tag = "Quit" });

            menu.MenuItems[3].Click += MenuItem_Click;
            menu.MenuItems[5].Click += MenuItem_Click;
            menu.MenuItems[6].Click += MenuItem_Click;
            menu.MenuItems[8].Click += MenuItem_Click;
            menu.MenuItems[9].Click += MenuItem_Click;
        }
        public static void LoadMenu(JObject json)//初始化,遍历json的menu节点
        {
            MenuItem menu = _NotifyIcon.ContextMenu.MenuItems.Find("PluginMenu", false).First();
            MenuItem menuItem = new MenuItem//一级菜单,插件的名称
            {
                Name = json["name"].ToString(),
                Text = json["name"].ToString()
            };
            if (!json.ContainsKey("menu"))
                return;
            foreach (var item in JArray.Parse(json["menu"].ToString()))
            {
                MenuItem childmenu = new MenuItem//二级菜单,窗口的名称
                {
                    Text = item["name"].ToString(),
                    Tag = new KeyValuePair<string, string>(json["name"].ToString(), item["name"].ToString())//插件名称与窗口函数名称,保存于这个菜单的tag中
                };
                menuItem.MenuItems.Add(childmenu);//加入二级子菜单
                childmenu.Click += MenuItem_Click;
            }
            menu.MenuItems.Add(0, menuItem);//加入一级子菜单
        }

        private delegate int Type_Menu();//窗口事件均为无参数
        private static Type_Menu menu;

        private static void MenuItem_Click(object sender, EventArgs e)
        {
            var targetItem = sender as MenuItem;
            try
            {
                switch (targetItem.Tag)
                {
                    case "Quit":
                        Quit();
                        return;
                    case "ReLoad":
                        MainForm.pluginManagment.ReLoad();
                        return;
                    case "LogForm":
                        MainForm.CallLogForm();
                        return;
                    case "Displaywindow":
                        if (targetItem.Checked)
                        {
                            targetItem.Checked = false;
                            MainForm.HideWindow();
                        }
                        else
                        {
                            targetItem.Checked = true;
                            MainForm.ShowWindow();
                        }
                        return;
                    case "TopMost":
                        if (targetItem.Checked)
                        {
                            targetItem.Checked = false;
                            MainForm.TopMost_Disabled();
                        }
                        else
                        {
                            targetItem.Checked = true;
                            MainForm.TopMost_Enable();
                        }
                        return;
                }
                KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(sender as MenuItem).Tag;
                PluginManagment.Plugin c = MainForm.pluginManagment.Plugins.Find(x => x.appinfo.Name == pair.Key);//从已加载的插件寻找这个名称的插件
                if(c.Enable is false)
                {
                    MessageBox.Show("插件未启用, 请打开菜单之前启用此插件", "无法调用菜单", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string menuname = string.Empty;
                foreach (var item in JArray.Parse(JObject.Parse(c.json)["menu"].ToString()))//遍历此插件的json的menu节点,寻找窗口函数
                {
                    if (item["name"].ToString() == pair.Value)
                    { menuname = item["function"].ToString(); break; }
                }
                menu = (Type_Menu)c.dll.Invoke(menuname, typeof(Type_Menu));//将函数转换委托
                menu();//调用
            }
            catch (Exception exc)
            {
                MessageBox.Show($"拉起菜单发生错误，错误信息:{exc.Message}\n{exc.StackTrace}", "菜单错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void ShowNotifyIcon()
        {
            _NotifyIcon.Visible = true;
        }
        public static void HideNotifyIcon()
        {
            _NotifyIcon.Visible = false;
        }
        public static void RemoveMenu(string pluginName)
        {
            var item = _NotifyIcon.ContextMenu.MenuItems.Find(pluginName, true).First();
            _NotifyIcon.ContextMenu.MenuItems[2].MenuItems.Remove(item);
        }
        public static void ClearAppMenu()
        {
            _NotifyIcon.ContextMenu.MenuItems[2].MenuItems.Clear();
        }
        public static void Quit()
        {
            MainForm.pluginManagment.CallFunction(FunctionEnums.Functions.Disable);
            MainForm.pluginManagment.CallFunction(FunctionEnums.Functions.Exit);
            MainForm.pluginManagment.UnLoad();
            HideNotifyIcon();
            Environment.Exit(0);
        }
        public static void AddManageMenu()
        {
            MenuItem menu = _NotifyIcon.ContextMenu.MenuItems.Find("PluginMenu", false).First();
            menu.MenuItems.Add("-");
            MenuItem manage = new MenuItem { Text = "插件管理", Tag = "PluginManage" };
            manage.Click += Manage_Click;
            menu.MenuItems.Add(manage);
        }
        [STAThread]
        private static void Manage_Click(object sender, EventArgs e)
        {
            PluginManageForm form = new PluginManageForm();
            form.Show();
        }
        public static void ReStart()
        {
            HideNotifyIcon();
            Application.Restart();
        } 
    }
}
