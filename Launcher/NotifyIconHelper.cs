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
            Icon = new Icon(@"E:\FFOutput\45e961f6ec64b0f340cb1d1870f95515.ico"),
            Visible = false,
            Text = "OPQBot 酷Q兼容框架",
            ContextMenu = new ContextMenu()            
        };
        public static void Init(JObject json)
        {
            ContextMenu menu = _NotifyIcon.ContextMenu;
            MenuItem menuItem = new MenuItem
            {
                Text = json["name"].ToString()
            };
            foreach (var item in JArray.Parse(json["menu"].ToString()))
            {
                MenuItem childmenu = new MenuItem
                {
                    Text = item["name"].ToString(),
                    Tag = new KeyValuePair<string, string>(json["name"].ToString(), item["name"].ToString())
                };
                menuItem.MenuItems.Add(childmenu);
                childmenu.Click += MenuItem_Click;
            }
            menu.MenuItems.Add(menuItem);
            _NotifyIcon.ContextMenu = menu;
        }

        private delegate int Type_Menu();
        private static Type_Menu menu;

        private static void MenuItem_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, string> pair = (KeyValuePair<string, string>)(sender as MenuItem).Tag;
            var c = Program.pluginManagment.Plugins.Find(x => x.appinfo.Name == pair.Key);
            string menuname = string.Empty;
            foreach(var item in JArray.Parse(c.json["menu"].ToString()))
            {
                if(item["name"].ToString()==pair.Value)
                { menuname = item["function"].ToString();break; }
            }
            menu=(Type_Menu) c.dll.Invoke(menuname, typeof(Type_Menu));
            menu();
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
