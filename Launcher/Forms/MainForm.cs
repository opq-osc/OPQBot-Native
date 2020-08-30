using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        [DllImport("user32.dll")]
        public extern static IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public extern static bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        public static uint LWA_COLORKEY = 0x00000001;
        public static uint LWA_ALPHA = 0x00000002;

        [DllImport("user32.dll")]
        public extern static uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        public extern static uint GetWindowLong(IntPtr hwnd, int nIndex);

        public enum WindowStyle : int { GWL_EXSTYLE = -20 }
        public enum ExWindowStyle : uint { WS_EX_LAYERED = 0x00080000 }
        private void MainForm_Load(object sender, EventArgs e)
        {
            //圆形窗体
            //GraphicsPath path = new GraphicsPath();
            //path.AddEllipse(0, 0, 60, 60);
            //Graphics g = CreateGraphics();
            //g.DrawEllipse(new Pen(Color.Black, 2), 0, 0, 60, 60);
            //Region = new Region(path);
            this.TransparencyKey = Color.Red;
            this.BackColor = Color.Red;
        }
        private void SetWindowTransparent(byte bAlpha)
        {
            try
            {
                SetWindowLong(this.Handle, (int)WindowStyle.GWL_EXSTYLE, GetWindowLong(this.Handle, (int)WindowStyle.GWL_EXSTYLE) | (uint)ExWindowStyle.WS_EX_LAYERED);
                SetLayeredWindowAttributes(this.Handle, 0, bAlpha, LWA_COLORKEY | LWA_ALPHA);
            }
            catch { }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
