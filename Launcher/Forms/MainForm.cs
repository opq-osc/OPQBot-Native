using Deserizition;
using SocketIOClient;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class MainForm : Form
    {
        static LogForm logForm = new LogForm();
        public Client socket;
        public MainForm()
        {
            InitializeComponent();
        }
        #region 拖动无窗体的控件
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        {
            //移动窗口到右下角
            this.Left = 1800; this.Top = 907;
            //设置窗口透明色, 实现窗口背景透明
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
            //实例化圆形图片框, 实现圆形的头像
            RoundPictureBox RoundpictureBox = new RoundPictureBox
            {
                Size = new Size(43, 43),
                Image = Image.FromFile(@"E:\图\Phone\QQ图片20200905184122.jpg"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Left = -1,
                Top = 0,
                ContextMenuStrip=RightContext
            };
            //添加拖动事件
            RoundpictureBox.MouseDown += pictureBox_Main_MouseDown;
            //显示控件, 置顶
            this.Controls.Add(RoundpictureBox);
            RoundpictureBox.BringToFront();
            //隐藏日志窗口, 但不关闭
            logForm.Text = $"运行日志 - {Save.curentQQ}";
            logForm.Show();
            logForm.Visible = false;
            logForm.socket = socket;
            //socket.Dispose(); //此处dispose是否会影响后面？
        }
        private void pictureBox_Main_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        //来自CSDN
        public class RoundPictureBox : PictureBox
        {
            protected override void OnCreateControl()
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(this.ClientRectangle);
                Region region = new Region(gp);
                this.Region = region;
                base.OnCreateControl();
            }
        }

        private void 日志toolStripMenuItem_Click(object sender, EventArgs e)
        {
            logForm.Visible = true;
            logForm.Show();
            //将窗口从后台抬出
            logForm.TopMost = false;
            logForm.TopMost = true;
            logForm.TopMost = logForm.formTopMost;
        }

        private void 退出toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
