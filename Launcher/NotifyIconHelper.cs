using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Launcher
{
    public static class NotifyIconHelper
    {
        static NotifyIcon _NotifyIcon = new NotifyIcon
        {
            Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("Launcher.Resources.Icon.ico")),//图标
            Visible = false,
            Text = "OPQBot 酷Q兼容框架",//鼠标移上去的提示文本
            ContextMenu = new ContextMenu()
        };
        public static void Init()
        {
            _NotifyIcon.ContextMenu = new ContextMenu();
            ContextMenu menu = _NotifyIcon.ContextMenu;
            menu.MenuItems.Add(new MenuItem() { Text = "插件菜单", Name = "PluginMenu" });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(new MenuItem() { Text = "重载应用", Tag = "ReLoad" });
            menu.MenuItems.Add(new MenuItem() { Text = "退出", Tag = "Quit" });
            for (int i = 2; i < 4; i++)
            {
                MenuItem item = menu.MenuItems[i];
                item.Click += MenuItem_Click;
            }
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
            menu.MenuItems.Add(menuItem);//加入一级子菜单
        }

        private delegate int Type_Menu();//窗口事件均为无参数
        private static Type_Menu menu;

        private static void MenuItem_Click(object sender, EventArgs e)
        {
            switch ((sender as MenuItem).Tag)
            {
                case "Quit":
                    Quit();
                    return;
                case "ReLoad":
                    ReLoad();
                    return;
            }
            KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(sender as MenuItem).Tag;
            var c = Program.pluginManagment.Plugins.Find(x => x.appinfo.Name == pair.Key);//从已加载的插件寻找这个名称的插件
            string menuname = string.Empty;
            foreach (var item in JArray.Parse(c.json["menu"].ToString()))//遍历此插件的json的menu节点,寻找窗口函数
            {
                if (item["name"].ToString() == pair.Value)
                { menuname = item["function"].ToString(); break; }
            }
            menu = (Type_Menu)c.dll.Invoke(menuname, typeof(Type_Menu));//将函数转换委托
            menu();//调用
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
            _NotifyIcon.ContextMenu.MenuItems[0].MenuItems.Remove(item);
        }
        public static void Quit()
        {
            Program.pluginManagment.CallFunction("Exit");
            NotifyIconHelper.HideNotifyIcon();
            Environment.Exit(0);
        }
        public static void ReLoad()
        {
            Program.pluginManagment.UnLoad();
            HideNotifyIcon();
            string path = Application.ExecutablePath;//获取可执行文件路径
            Process.Start(path);//再次运行程序
            Environment.Exit(0);//关闭当前程序
            //Program.pluginManagment.Init();
        }
    }
}
