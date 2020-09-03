namespace Launcher.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.RightContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.插件菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重载应用toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.RightContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.ContextMenuStrip = this.RightContext;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 44);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // RightContext
            // 
            this.RightContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.插件菜单ToolStripMenuItem,
            this.日志toolStripMenuItem,
            this.toolStripSeparator1,
            this.重载应用toolStripMenuItem,
            this.退出toolStripMenuItem});
            this.RightContext.Name = "RIghtContext";
            this.RightContext.Size = new System.Drawing.Size(181, 120);
            // 
            // 插件菜单ToolStripMenuItem
            // 
            this.插件菜单ToolStripMenuItem.Name = "插件菜单ToolStripMenuItem";
            this.插件菜单ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.插件菜单ToolStripMenuItem.Text = "应用";
            // 
            // 日志toolStripMenuItem
            // 
            this.日志toolStripMenuItem.Name = "日志toolStripMenuItem";
            this.日志toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.日志toolStripMenuItem.Text = "日志";
            // 
            // 重载应用toolStripMenuItem
            // 
            this.重载应用toolStripMenuItem.Name = "重载应用toolStripMenuItem";
            this.重载应用toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.重载应用toolStripMenuItem.Text = "重载应用";
            // 
            // 退出toolStripMenuItem
            // 
            this.退出toolStripMenuItem.Name = "退出toolStripMenuItem";
            this.退出toolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出toolStripMenuItem.Text = "退出";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(102, 44);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "OPQBot-Native 兼容框架";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.RightContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip RightContext;
        private System.Windows.Forms.ToolStripMenuItem 插件菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日志toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重载应用toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出toolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}