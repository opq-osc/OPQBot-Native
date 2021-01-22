using System;
using System.Windows.Forms;
using Deserizition;
using Jie2GG.Tool.IniConfig;
using Jie2GG.Tool.IniConfig.Linq;
using SocketIOClient;

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
            if (ini.Object["Config"]["AutoLogin"].GetValueOrDefault(false))
            {
                checkBox_AutoLogin.Checked = true;
                button_Link.PerformClick(); 
            }
        }
        private void Init()
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
                ini.Object["Config"].Add("url", "http://127.0.0.1:8888/");
                ini.Object["Config"].Add("AutoLogin", false);
                Save.curentQQ = 0;
                Save.url = "";
                ini.Save();
            }
        }
        private void button_Link_Click(object sender, EventArgs e)
        {
            button_Link.Enabled = false;
            ini.Object["Config"]["QQ"] = new IValue(textBox_QQ.Text);
            ini.Object["Config"]["url"] = new IValue(textBox_URL.Text);
            ini.Object["Config"]["AutoLogin"] = new IValue(checkBox_AutoLogin.Checked);
            ini.Save();
            Save.curentQQ = Convert.ToInt64(textBox_QQ.Text);
            Save.url = textBox_URL.Text;
            Client socket = new Client(Save.url);
            socket.Error += Client_Error;
            socket.Connect();
            socket.On("connect", (fn) =>
            {
                socket.Emit("GetWebConn",Save.curentQQ.ToString());
                if (Save.LoginStatus)
                {
                    LogHelper.WriteLog("重新连接到服务器");
                    return;
                }
                this.Invoke(new MethodInvoker(() =>
                {
                    MainForm mainForm = new MainForm();
                    mainForm.socket = socket;
                    mainForm.Show();
                    mainForm.Visible= mainForm.ShowFlag;
                    mainForm.TopMost = mainForm.TopFlag;
                    this.Hide();
                    button_Link.Enabled = true;
                }));
                socket.Error -= Client_Error;
            });
        }

        private void Client_Error(object sender, ErrorEventArgs e)
        {
            MessageBox.Show("连接失败，请检查连接地址是否填写正确");
            Client client = sender as Client;
            client.Dispose();
            button_Link.Invoke(new MethodInvoker(()=> { button_Link.Enabled = true; }));
        }
    }
}
