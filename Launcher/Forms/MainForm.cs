using Deserizition;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class MainForm : Form
    {
        static LogForm logForm = new LogForm();
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
            this.Left = 1800;this.Top = 907;
            this.TransparencyKey = Color.Gray;
            this.BackColor = Color.Gray;
            CtrlRoundPictureBox RoundpictureBox = new CtrlRoundPictureBox
            {
                Size=new Size(43,43),
                Image = Image.FromFile(@"E:\图\Phone\QQ图片20200905184122.jpg"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Left = 0,
                Top = 0
            };
            RoundpictureBox.MouseDown += pictureBox_Main_MouseDown;
            this.Controls.Add(RoundpictureBox);
            RoundpictureBox.BringToFront();

            logForm.Visible = false;
            logForm.Text = $"运行日志 - {Save.curentQQ}";
            //logForm.Show();
        }
        private void pictureBox_Main_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            if (e.Button == MouseButtons.Right)
                RightContext.Show();
        }
        public class CtrlRoundPictureBox : PictureBox
        {
            protected override void OnCreateControl()
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(this.ClientRectangle);
                Region region = new Region(gp);
                this.Region = region;
                gp = null;
                region = null;
                base.OnCreateControl();
            }
        }

        private void 日志toolStripMenuItem_Click(object sender, EventArgs e)
        {
            logForm.Visible = true;
        }

        private void 退出toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
