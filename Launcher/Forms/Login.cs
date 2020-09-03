using Deserizition;
using Native.Tool.IniConfig;
using Native.Tool.IniConfig.Linq;
using System;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private static IniConfig ini = new IniConfig("Config.ini");
        private void Login_Load(object sender, EventArgs e)
        {
            Init();
            textBox_QQ.Text = Save.curentQQ.ToString();
            textBox_URL.Text = Save.url;
        }
        private static void Init()
        {
            ini.Load();
            try
            {
                Save.curentQQ = ini.Object["Config"]["QQ"].GetValueOrDefault((long)0);
                Save.url = ini.Object["Config"]["url"].GetValueOrDefault("http://127.0.0.1:8888/");
            }
            catch
            {
                ini.Clear();
                ini.Object.Add(new ISection("Config"));
                ini.Object["Config"].Add("QQ", 0);
                ini.Object["Config"].Add("url", "");
                Save.curentQQ = 0;
                Save.url = "";
                ini.Save();
            }
        }

        private void button_Link_Click(object sender, EventArgs e)
        {
            ini.Object["Config"]["QQ"] = new IValue(textBox_QQ.Text);
            ini.Object["Config"]["url"] = new IValue(textBox_URL.Text);
            ini.Save();
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }
}
