namespace Launcher.Forms
{
    partial class Login
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_QQ = new System.Windows.Forms.TextBox();
            this.textBox_URL = new System.Windows.Forms.TextBox();
            this.button_Link = new System.Windows.Forms.Button();
            this.checkBox_AutoLogin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "OPQBot登录QQ号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "OPQBot连接地址";
            // 
            // textBox_QQ
            // 
            this.textBox_QQ.Location = new System.Drawing.Point(114, 21);
            this.textBox_QQ.Name = "textBox_QQ";
            this.textBox_QQ.Size = new System.Drawing.Size(212, 21);
            this.textBox_QQ.TabIndex = 2;
            // 
            // textBox_URL
            // 
            this.textBox_URL.Location = new System.Drawing.Point(114, 54);
            this.textBox_URL.Name = "textBox_URL";
            this.textBox_URL.Size = new System.Drawing.Size(212, 21);
            this.textBox_URL.TabIndex = 3;
            // 
            // button_Link
            // 
            this.button_Link.Location = new System.Drawing.Point(139, 90);
            this.button_Link.Name = "button_Link";
            this.button_Link.Size = new System.Drawing.Size(75, 23);
            this.button_Link.TabIndex = 4;
            this.button_Link.Text = "连接";
            this.button_Link.UseVisualStyleBackColor = true;
            this.button_Link.Click += new System.EventHandler(this.button_Link_Click);
            // 
            // checkBox_AutoLogin
            // 
            this.checkBox_AutoLogin.AutoSize = true;
            this.checkBox_AutoLogin.Location = new System.Drawing.Point(220, 94);
            this.checkBox_AutoLogin.Name = "checkBox_AutoLogin";
            this.checkBox_AutoLogin.Size = new System.Drawing.Size(72, 16);
            this.checkBox_AutoLogin.TabIndex = 5;
            this.checkBox_AutoLogin.Text = "自动登录";
            this.checkBox_AutoLogin.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 125);
            this.Controls.Add(this.checkBox_AutoLogin);
            this.Controls.Add(this.button_Link);
            this.Controls.Add(this.textBox_URL);
            this.Controls.Add(this.textBox_QQ);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "连接配置";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_QQ;
        private System.Windows.Forms.TextBox textBox_URL;
        private System.Windows.Forms.Button button_Link;
        private System.Windows.Forms.CheckBox checkBox_AutoLogin;
    }
}