using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher
{
    public static class NotifyIconHelper
    {
        static NotifyIcon _NotifyIcon = new NotifyIcon
        {
            Icon = new Icon(@"E:\FFOutput\45e961f6ec64b0f340cb1d1870f95515.ico"),//图标
            Visible = false,
            Text = "OPQBot 酷Q兼容框架",//鼠标移上去的提示文本
            ContextMenu = new ContextMenu()
        };
        public static void Init(JObject json)//初始化,遍历json的menu节点
        {
            ContextMenu menu = _NotifyIcon.ContextMenu;
            MenuItem menuItem = new MenuItem//一级菜单,插件的名称
            {
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
            _NotifyIcon.ContextMenu = menu;//覆盖托盘菜单
        }

        private delegate int Type_Menu();//窗口事件均为无参数
        private static Type_Menu menu;

        private static void MenuItem_Click(object sender, EventArgs e)
        {
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
    }
}
